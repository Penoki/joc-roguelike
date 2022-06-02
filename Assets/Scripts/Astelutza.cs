using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astelutza : MonoBehaviour
{
    /*
    private List<Nod> OPEN = new List<Nod>();       //nodurile care trebuie sa fie evaluate
    private List<Nod> CLOSED = new List<Nod>();     //nodurile care au fost deja evaluate
    public GameObject tzinta, gridParinte;          //jucatorul in cazul de fata
    public Rigidbody2D rb;
    public int[,] grilajCurent;                     //camera cu obstacole
    private Vector2 ultimaPozMatrice;
    const int N = 7;

    private void Start()
    {
        gridParinte = this.transform.parent.gameObject;
        gridParinte.GetComponent<detaliiIncapere>().ActualizareGrilaCamera();
        ActualizareCamera();
        //pornire cautare
        Astea();
    }

    private void Update()
    {
        //ActualizareCamera();
        //Astea();
    }

    private void ActualizareCamera()
    {
        grilajCurent = gridParinte.GetComponent<detaliiIncapere>().grilajCamera;
        //plasare pozitie start cu 2
        grilajCurent[conv_y_IN_i(Mathf.FloorToInt(rb.position.y)), conv_x_IN_j(Mathf.FloorToInt(rb.position.x))] = 2;
        //plasare pozitia tzintei cu 3
        grilajCurent[conv_y_IN_i(Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.y)), conv_x_IN_j(Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.x))] = 3;
    }

    private void Astea()
    {
        //initializare start si final
        Nod start = new Nod(new Vector2Int(conv_y_IN_i(Mathf.FloorToInt(rb.position.y)), conv_x_IN_j(Mathf.FloorToInt(rb.position.x))));
        Nod final = new Nod(new Vector2Int(Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.y), Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.x)));

        //adauga in open pentru evaluare
        OPEN.Add(start);

        //cautarea path-ului cel mai scurt
        while (OPEN.Count > 0)
        {
            //cautare NOD in OPEN cu cel mai mic cost F
            Nod curent = OPEN[0];
            foreach (Nod a in OPEN)
            {
                if (a.Fcost < curent.Fcost || (a.Fcost == curent.Fcost && a.Hcost < curent.Hcost))
                {
                    curent = a;
                }
            }
            //dupa evaluare mutam NOD in CLOSED
            OPEN.Remove(curent);
            CLOSED.Add(curent);

            //daca NOD coincide cu tzinta, atunci ne oprim
            if (curent.poz == final.poz)
            {
                reiaCalea(start, curent);
                return;
            }

            foreach (Nod vecin in iaVecinii(curent))
            {
                //verificare daca este traversibil sau daca vecinul se afla deja in CLOSED
                if (grilajCurent[vecin.poz.x, vecin.poz.y] == 1 || CLOSED.Contains(vecin))
                {
                    continue;
                }

                //verificare daca calea catre vecin e mai scurta sau daca nu este in OPEN
                int nouCostMiscareCatreVecin = curent.Gcost + iaDistanta(curent, vecin);
                if (nouCostMiscareCatreVecin < curent.Gcost || !OPEN.Contains(vecin))
                {
                    //calculul costurilor
                    vecin.Gcost = nouCostMiscareCatreVecin;
                    vecin.Hcost = iaDistanta(vecin, final);
                    vecin.parinte = curent;

                    if (!OPEN.Contains(vecin))
                    {
                        OPEN.Add(vecin);
                    }
                }
            }

        }
    }

    //functie de reluarea a cararii
    public void reiaCalea(Nod startNod, Nod finalNod)
    {
        List<Nod> calea = new List<Nod>();
        Nod nodCurent = finalNod;

        while(nodCurent!= startNod)
        {
            calea.Add(nodCurent);
            nodCurent = nodCurent.parinte;
        }

        calea.Reverse();


    }

    //functie calcul distanta dintre 2 puncte
    public int iaDistanta(Nod A, Nod B)
    {
        int dstX = Mathf.Abs(A.poz.x - B.poz.x);
        int dstY = Mathf.Abs(A.poz.y - B.poz.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    //functie care returneaza vecinii unui NOD
    public List<Nod> iaVecinii(Nod nodul)
    {
        List<Nod> vecini = new List<Nod>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int verifI = nodul.poz.x + i;
                int verifJ = nodul.poz.y + j;

                if (verifI >= 0 && verifI < N && verifJ >= 0 && verifJ < N)
                {
                    vecini.Add(new Nod(new Vector2Int(verifI, verifJ)));
                }
            }
        }

        return vecini;
    }

    private void FixedUpdate()
    {
        //miscare pe carare catre jucator
    }

    public void CampDeschis()
    {
        float x, y, xj, yj, marja = 0.03f, viteza = 55f;
        xj = tzinta.GetComponent<Rigidbody2D>().position.x;
        yj = tzinta.GetComponent<Rigidbody2D>().position.y;
        //realizare miscare catre jucator
        if (rb.position.x <= xj + marja && rb.position.x >= xj - marja)
        {
            x = 0f;
        }
        else if (rb.position.x < xj)
        {
            x = 0.01f;
        }
        else
        {
            x = -0.01f;
        }

        if (rb.position.y <= yj + marja && rb.position.y >= yj - marja)
        {
            y = 0f;
        }
        else if (rb.position.y < yj)
        {
            y = 0.01f;
        }
        else
        {
            y = -0.01f;
        }

        rb.MovePosition(rb.position + new Vector2(x, y) * viteza * Time.fixedDeltaTime);
    }

    #region Traducere coordonate in indici matrice
    public int conv_x_IN_j(int x)
    {
        return x + (N - 1) / 2;
    }

    public int conv_j_IN_x(int j)
    {
        return j - (N - 1) / 2;
    }

    public int conv_y_IN_i(int y)
    {
        return N - y - (N - 1) / 2 - 1;
    }

    public int conv_i_IN_y(int i)
    {
        return N - i - (N - 1) / 2 - 1;
    }
    #endregion
    */
}
