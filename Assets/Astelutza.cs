using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astelutza : MonoBehaviour
{
    struct NOD
    {
        Vector2Int poz;
        int Gcost, Hcost, Fcost;

        public Vector2Int Poz { get => poz; set => poz = value; }
        public int Gcost1 { get => Gcost; set => Gcost = value; }
        public int Hcost1 { get => Hcost; set => Hcost = value; }
        public int Fcost1 { get => Fcost; set => Fcost = value; }
    }

    private List<NOD> OPEN = new List<NOD>();       //nodurile care trebuie sa fie evaluate
    private List<NOD> CLOSED = new List<NOD>();     //nodurile care au fost deja evaluate
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
        //1 adaugare NOD de start in OPEN
        NOD start = new NOD { };
        //adaugare pozitie in NOD
        start.Poz = new Vector2Int(conv_y_IN_i(Mathf.FloorToInt(rb.position.y)), conv_x_IN_j(Mathf.FloorToInt(rb.position.x)));
        //calcul costuri
        Calcul(start);
        //adauga in open pentru evaluare
        OPEN.Add(start);

        //cautarea path-ului cel mai scurt
        while (OPEN.Count > 0)
        {
            //cautare NOD in OPEN cu cel mai mic cost F
            NOD curent = OPEN[0];
            foreach (NOD a in OPEN)
            {
                if (a.Fcost1 < curent.Fcost1 || (a.Fcost1==curent.Fcost1 && a.Hcost1<curent.Hcost1))
                {
                    curent = a;
                }
            }
            //dupa evaluare mutam NOD in CLOSED
            OPEN.Remove(curent);
            CLOSED.Add(curent);

            //daca NOD coincide cu tzinta, atunci ne oprim
            if (curent.Poz == new Vector2(Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.y), Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.x)))
            {
                break;
            }


            int x = curent.Poz.x;
            int y = curent.Poz.y;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //Verificare Traversibil
                    bool traversibil = false;
                    //verificare sa nu luam pozitia curenta ca vecin
                    if (i != 0 || j != 0)
                    {
                        //verificare sa nu fie pe margine
                        if (!(x + i < 0 || y + j < 0 || x + i > N - 1 || y + j > N - 1))
                        {
                            if (grilajCurent[curent.Poz.x + i, curent.Poz.y + j] == 0)
                            {
                                if (!(x + i == y + j || i == -j))
                                {
                                    traversibil = true;
                                }
                                else
                                {

                                }
                            }
                        }
                        //daca se afla pe margine
                        else
                        {

                        }
                    }
                }
            }
        }
    }

    //functie care returneaza vecinii unui NOD
    public List<NOD> iaVecinii(Vector2Int nodul)
    {
        List<NOD> vecini = new List<NOD>();
        return vecini;
    }

    //functie pentru calcularea costurilor pentru un NOD
    private void Calcul(NOD a)
    {
        //distanta/cost de la nod la start
        float g = 10 * Mathf.Sqrt(Mathf.Pow(conv_y_IN_i(Mathf.FloorToInt(rb.position.y)) - a.Poz.x, 2) + Mathf.Pow(conv_x_IN_j(Mathf.FloorToInt(rb.position.x)) - a.Poz.y, 2));
        a.Gcost1 = (int)g;
        //distanta/cost de la nod la tzinta
        float h = 10 * Mathf.Sqrt(Mathf.Pow(conv_y_IN_i(Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.y)) - a.Poz.x, 2) + Mathf.Pow(conv_x_IN_j(Mathf.FloorToInt(tzinta.GetComponent<Rigidbody2D>().position.x)) - a.Poz.y, 2));
        a.Hcost1 = (int)h;
        //costul total
        a.Fcost1 = a.Gcost1 + a.Hcost1;
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
}
