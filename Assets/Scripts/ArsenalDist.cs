using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalDist : MonoBehaviour
{
    public LayerMask blocadaMasca;
    bool alert = false;
    private GameObject jucator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            alert = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            alert = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        jucator = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (alert)
        {
            RaycastHit2D hit = Physics2D.Linecast(this.transform.parent.position, jucator.transform.position, blocadaMasca);
            if (!hit)
            {
                
            }

        }
    }
}
