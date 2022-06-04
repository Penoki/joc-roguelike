using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializareInamiciCN : MonoBehaviour
{
    public List<Vector2Int> puncteMaterializare;
    public GameObject marioneta;
    // Start is called before the first frame update
    void Start()
    {
        marioneta = Resources.Load("Prefabs/Marioneta") as GameObject;
    }

    public void inamiciMaterializare()
    {
        //instantiere 1 inamic pentru testare
        //Instantiate(marioneta, this.transform);
        //marioneta.transform.position = new Vector3(puncteMaterializare[0].x + 0.5f, puncteMaterializare[0].y + 0.5f, 0);

        //instantiere toti inamicii 
        
        foreach (Vector2Int a in puncteMaterializare)
        {
            Instantiate(marioneta, this.transform);
            marioneta.transform.position = new Vector3(a.x+0.5f, a.y+0.5f, 0);
        }
    }
}
