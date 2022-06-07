using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFinala : MonoBehaviour
{
    public GameObject butonFinNivel;

    void Start()
    {
        //luarea referintei pentru buton
        butonFinNivel = Resources.Load("Prefabs/ButonFin") as GameObject;

        //creare buton
        Instantiate(butonFinNivel, this.transform);
    }

}
