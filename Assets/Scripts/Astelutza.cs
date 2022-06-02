using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astelutza : MonoBehaviour
{
    public Transform tzinta, cautator, Astea;
    Gridul grid;

    public float viteza = 400f;
    public float distantaUrmPct = 0.1f;

    int pctCurent = 0;
    bool atinsFinal = false;

    Rigidbody2D rb;

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
        if (grid.carare == null)
            return;

        
        Vector2 directie = ((Vector2)grid.Nod_to_coordLume(grid.carare[pctCurent]) - rb.position).normalized;
        Vector2 forta = directie * viteza * Time.deltaTime;

        rb.AddForce(forta);

        float distanta = Vector2.Distance(rb.position, grid.Nod_to_coordLume(grid.carare[pctCurent]));

        if (distanta < distantaUrmPct)
        {
            pctCurent++;
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
