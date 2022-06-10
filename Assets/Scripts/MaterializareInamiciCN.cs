using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializareInamiciCN : MonoBehaviour
{
    public List<Vector2> puncteMaterializare, puncteSegmentate;
    public List<GameObject> inamic = new List<GameObject>();
    private int nrInamici;
    private GameObject materializat;
    [Range(1, 3)] public int dificultate = 1; // la dificultate 1 intre 3 si 5 inamici per camera
    // Start is called before the first frame update
    void Start()
    {
        inamic.Add(Resources.Load("Prefabs/Marioneta") as GameObject);
        inamic.Add(Resources.Load("Prefabs/SpargatorDeNuci") as GameObject);

        //cati inamici se vor materializa
        nrInamici = Random.Range(2 + dificultate, 4 + dificultate + 1);

        //segmentare locatiilor de spawn in 4
        puncteSegmentate = new List<Vector2>();
        foreach (Vector2 a in puncteMaterializare)
        {
            Vector2 b = new Vector2();
            b.x = a.x + 0.3f;
            b.y = a.y + 0.3f;
            puncteSegmentate.Add(b);

            b.x = a.x + 0.7f;
            b.y = a.y + 0.3f;
            puncteSegmentate.Add(b);

            b.x = a.x + 0.3f;
            b.y = a.y + 0.7f;
            puncteSegmentate.Add(b);

            b.x = a.x + 0.7f;
            b.y = a.y + 0.7f;
            puncteSegmentate.Add(b);
        }
    }

    public void inamiciMaterializare()
    {
        //instantiere 1 inamic pentru testare
        //Instantiate(marioneta, this.transform);
        //marioneta.transform.position = new Vector3(puncteMaterializare[0].x + 0.5f, puncteMaterializare[0].y + 0.5f, 0);


        while (nrInamici > 0)
        {
            //instantiere toti inamicii 
            foreach (Vector2 a in puncteSegmentate)
            {
                if (Random.Range(0, 2) == 1)
                {
                    //50-50 chance de materializare marioneta sau spargator
                    materializat = Instantiate(inamic[Random.Range(0, 2)], this.transform);
                    materializat.transform.position = new Vector3(a.x, a.y, 0) + this.transform.position;
                    nrInamici--;
                    if (nrInamici == 0)
                    { return; }
                }
            }
        }

    }
}
