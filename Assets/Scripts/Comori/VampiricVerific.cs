using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampiricVerific : MonoBehaviour
{
    public detaliiIncapere infoCameraCurenta;

    void Update()
    {
        infoCameraCurenta = GameObject.FindGameObjectWithTag("cameraCurenta").GetComponent<detaliiIncapere>();
        if (infoCameraCurenta.tipIncapere == 'N' && infoCameraCurenta.completata == false)
        {

        }
    }
}
