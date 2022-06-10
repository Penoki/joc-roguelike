using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComori : MonoBehaviour
{
    public List<GameObject> comori = new List<GameObject>();
    void Start()
    {
        //luarea referintelor pentru comori
        Object[] aux = Resources.LoadAll("Prefabs/Comori");

        //adaugarea elementelor dintr-o matrice intr-o lista pentru convenienta
        for (int i = 0; i < aux.Length; i++)
        {
            comori.Add(aux[i] as GameObject);
        }

        //creare buton
        Instantiate(comori[Random.Range(0,comori.Count)], this.transform);
    }


}
