using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFinala : MonoBehaviour
{
    public GameObject butonFinNivel, zmeu;

    void Start()
    {
        //luarea referintei pentru buton si zmeu
        butonFinNivel = Resources.Load("Prefabs/ButonFin") as GameObject;
        zmeu = Resources.Load("Prefabs/Zmeu") as GameObject;

    }

    public void CreareZmeu()
    {
        //creare zmeu
        Instantiate(zmeu, this.transform);
    }

    public void CreareButon()
    {
        //creare buton
        Instantiate(butonFinNivel, this.transform);
    }

}
