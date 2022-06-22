using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class VampiricVerific : MonoBehaviour
{
    public detaliiIncapere infoCameraCurenta;
    public int nrInamiciCurent, nrInamiciVechi;
    private bool cameraNoua = true;

    void Update()
    {
        infoCameraCurenta = GameObject.FindGameObjectWithTag("cameraCurenta").GetComponent<detaliiIncapere>();
        if (infoCameraCurenta.tipIncapere == 'N' && !infoCameraCurenta.completata)
        {
            Invoke("nrActualizare", 1);
            if (nrInamiciCurent < nrInamiciVechi)
            {
                //daca jucatorul are mai putina viata decat maximul
                if (this.GetComponent<Jucator>().pctSanatateMax > this.GetComponent<Jucator>().pctSanatate)
                {
                    System.Random reee= new System.Random();
                    //sansa 28% ca jucatorul sa fie binecuvantat cu santate
                    if (reee.Next(0, 100) < 28)
                    {
                        this.GetComponent<Jucator>().pctSanatate++;
                        //actualizare viata ecran
                        this.GetComponent<Jucator>().ui.ActualizareVital();
                    } 
                }
            }
            nrInamiciVechi = nrInamiciCurent;
        }

    }

    public void nrActualizare()
    {
        nrInamiciCurent = infoCameraCurenta.inamici.Count;
    }
}
