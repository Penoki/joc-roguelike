using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astelutza : MonoBehaviour
{
    public Transform tzinta, cautator, Astea;
    Gridul grid;

    int pctCurent = 0;
    public float viteza = 55f, marja = 0.01f, distanta = 0.01f;
    Rigidbody2D rb;

    bool atinsFinal = false;

    private void Start()
    {
        grid = Astea.GetComponent<Gridul>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GasesteCalea(cautator.position, tzinta.position);
    }

    private void FixedUpdate()
    {
        Vector2 p = tzinta.position;
        if ((rb.position.x >= (p.x - marja) && rb.position.x <= (p.x + marja)) && (rb.position.y >= (p.y - marja) && rb.position.y <= (p.y + marja))) { atinsFinal = true; return; }
        else { atinsFinal = false; }

        if (grid.carare != null)
        {
            if (grid.carare.Count != 0)
            {
                if (rb.position != (Vector2)tzinta.position)
                {
                    Vector2 wp = grid.carare[pctCurent].poz;
                    if ((rb.position != wp))
                    {
                        float distantaCurenta = Vector2.Distance(rb.position, tzinta.position);
                        if (distantaCurenta >= distanta)
                        {
                            rb.position = Vector2.MoveTowards(rb.position, wp, viteza * Time.deltaTime);
                        }

                        /*
                        float x, y;
                        if (rb.position.x <= wp.x + marja && rb.position.x >= wp.x - marja)
                        {
                            x = 0f;
                        }
                        else if (rb.position.x < wp.x)
                        {
                            x = 0.01f;
                        }
                        else
                        {
                            x = -0.01f;
                        }

                        if (rb.position.y <= wp.y + marja && rb.position.y >= wp.y - marja)
                        {
                            y = 0f;
                        }
                        else if (rb.position.y < wp.y)
                        {
                            y = 0.01f;
                        }
                        else
                        {
                            y = -0.01f;
                        }

                        rb.MovePosition(rb.position + new Vector2(x, y) * viteza * Time.fixedDeltaTime);
                        
                    }*/
                        else
                        {
                            pctCurent++;
                            if (pctCurent > grid.carare.Count) { pctCurent = 0; }
                        }
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
