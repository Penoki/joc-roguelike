using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeJucatorZbor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //modificat layer-ul pe jucator ca fiind cel de zbor
            //adica ignora coliziunea cu obstacolele
            collision.gameObject.layer = 6;

            //facut jucator sa arate ca o fantoma :))
            Jucator.srNcolor = new Color(1,1,1,0.8f);
            collision.GetComponent<Jucator>().srC.color = Jucator.srNcolor;
            collision.GetComponent<Jucator>().srJ.color = Jucator.srNcolor;
        }
    }
}
