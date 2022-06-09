using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astelutza : MonoBehaviour
{
    public GameObject AsteaMatritza, Astea;
    public GameObject tzinta, cautator;
    public float timpIncepere = 0.1f;
    private bool pornit = false;
    Gridul grid;

    int pctCurent = 0;
    public float viteza = 55f, marja = 0.01f, distanta = 0.01f;
    Rigidbody2D rb;


    private void Start()
    {
        cautator = new GameObject();
        tzinta = new GameObject();
        //instantiere A*grid pentru acest inamic
        AsteaMatritza = Resources.Load("Prefabs/A_grid") as GameObject;
        Astea = Instantiate(AsteaMatritza, this.transform.parent.transform);

        //Astea = this.transform.parent.Find("A*grid").transform;
        grid = Astea.GetComponent<Gridul>();
        rb = GetComponent<Rigidbody2D>();
        tzinta = GameObject.FindWithTag("Player");
        cautator = this.gameObject;
        Invoke("Incepe", timpIncepere);
    }

    void Incepe()
    {
        pornit = true;
    }

    void Update()
    {
        if (pornit)
            GasesteCalea(cautator.transform.position, tzinta.transform.position);
    }

    private void FixedUpdate()
    {
        Vector2 p = tzinta.transform.position;
        if ((rb.position.x >= (p.x - marja) && rb.position.x <= (p.x + marja)) && (rb.position.y >= (p.y - marja) && rb.position.y <= (p.y + marja))) { return; }

        if (grid.carare != null)
        {
            if (grid.carare.Count != 0)
            {
                if (rb.position != (Vector2)tzinta.transform.position && pctCurent < grid.carare.Count && pctCurent >= 0)
                {
                    Vector2 wp = grid.carare[pctCurent].poz;
                    //Debug.Log("Pozitie tzinta: " + wp);
                    if ((rb.position != wp))
                    {
                        float distantaCurenta = Vector2.Distance(rb.position, tzinta.transform.position);
                        if (distantaCurenta >= distanta)
                        {
                            rb.position = Vector2.MoveTowards(rb.position, wp, viteza * Time.deltaTime);
                        }
                        else
                        {
                            pctCurent++;
                            if (pctCurent > grid.carare.Count) { pctCurent = 0; }
                        }
                    }
                    else
                    {
                        pctCurent++;
                        if (pctCurent > grid.carare.Count) { pctCurent = 0; }
                    }
                }
            }
        }

    }

    //algoritm de cautare A* de la pct A la pct B
    void GasesteCalea(Vector3 pozA, Vector3 pozB)
    {
        Nod nodA = grid.coordLume_to_Nod(pozA);
        Nod nodB = grid.coordLume_to_Nod(pozB);
        if (nodA == null || nodB == null) { return; }

        //initializare lista pt nodurile care trebuie sa fie evaluate
        List<Nod> OPEN = new List<Nod>();
        //initializare lista pt nodurile care au fost deja evaluate
        List<Nod> CLOSED = new List<Nod>();

        OPEN.Add(nodA);

        //cautarea path-ului cel mai scurt
        while (OPEN.Count > 0)
        {
            //cautare NOD in OPEN cu cel mai mic cost F
            Nod curent = OPEN[0];
            foreach (Nod a in OPEN)
            {
                //de optimizat
                if (a.Fcost < curent.Fcost || (a.Fcost == curent.Fcost && a.Hcost < curent.Hcost))
                {
                    curent = a;
                }
            }

            //dupa evaluare mutam NOD in CLOSED
            OPEN.Remove(curent);
            CLOSED.Add(curent);

            //daca NOD coincide cu tzinta, atunci ne oprim
            if (curent == nodB)
            {
                reiaCalea(nodA, curent);
                return;
            }

            foreach (Nod vecin in grid.iaVecinii(curent))
            {
                //verificare daca este traversibil sau daca vecinul se afla deja in CLOSED
                if (!vecin.traversabil || CLOSED.Contains(vecin))
                {
                    continue;
                }

                //verificare daca calea catre vecin e mai scurta sau daca nu este in OPEN
                int nouCostMiscareCatreVecin = curent.Gcost + iaDistanta(curent, vecin);
                if (nouCostMiscareCatreVecin < curent.Gcost || !OPEN.Contains(vecin))
                {
                    //calculul costurilor
                    vecin.Gcost = nouCostMiscareCatreVecin;
                    vecin.Hcost = iaDistanta(vecin, nodB);
                    vecin.parinte = curent;

                    if (!OPEN.Contains(vecin))
                    {
                        OPEN.Add(vecin);
                    }
                }
            }
        }
    }

    //functie calcul distanta dintre 2 puncte
    public int iaDistanta(Nod A, Nod B)
    {
        int dstX = Mathf.Abs(A.gridX - B.gridX);
        int dstY = Mathf.Abs(A.gridY - B.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    //functie de reluarea a cararii, de la final la inceput
    //salvam intr-o lista apoi o intoarcem pe dos
    public void reiaCalea(Nod startNod, Nod finalNod)
    {
        List<Nod> calea = new List<Nod>();
        Nod nodCurent = finalNod;

        while (nodCurent != startNod)
        {
            calea.Add(nodCurent);
            nodCurent = nodCurent.parinte;
        }

        calea.Reverse();
        grid.carare = calea;
    }


}
