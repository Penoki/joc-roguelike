using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaugaComponentaVampiric : MonoBehaviour
{
    public GameObject jucator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            jucator = collision.gameObject;
            jucator.AddComponent<VampiricVerific>();

            //+1 sanatate, +1 max sanatate
            jucator.GetComponent<Jucator>().pctSanatate++;
            jucator.GetComponent<Jucator>().pctSanatateMax++;
        }
    }
}
