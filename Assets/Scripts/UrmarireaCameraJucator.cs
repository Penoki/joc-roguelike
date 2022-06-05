using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrmarireaCameraJucator : MonoBehaviour
{
    public Transform tinta;
    public Vector3 decalaj;
    [Range(1, 10)]
    public float netezire;
    void FixedUpdate()
    {
        if(tinta!=null)
        Urmareste();
    }

    void Urmareste()
    {
        Vector3 pozitieTinta = tinta.position + decalaj;
        Vector3 pozitieNeteda = Vector3.Lerp(transform.position, pozitieTinta, netezire * Time.fixedDeltaTime);
        transform.position = pozitieNeteda; 
    }
}
