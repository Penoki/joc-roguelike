using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manechin : MonoBehaviour
{
    public SpriteRenderer sr;
    public double sanatate = 10;
    public double ranireRacireSec = 1.4;
    private int msecundaRanit, secundaRanit;
    public GameObject jucator;
    public bool ranit = false, atingere = false;

    private void Start()
    {
        jucator = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "ostil")
        {
            ranit = true;
            dauna(collision.GetComponent<Ochi>().CatDauna);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            atingere = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            atingere = false;
        }
    }

    private void dauna(double catdemult)
    {
        sanatate -= catdemult;
        sr.color = new Color(1, 0.6f, 0.6f, 1);
        msecundaRanit = System.DateTime.Now.Millisecond + 400;
        secundaRanit = System.DateTime.Now.Second;
        if (msecundaRanit >= 1000)
        {
            msecundaRanit = msecundaRanit % 1000;
            secundaRanit++;
            if (secundaRanit >= 60)
            {
                secundaRanit = secundaRanit % 60;
            }
        }
        if (sanatate <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (ranit)
        {
            if ((msecundaRanit + secundaRanit * 1000) <= (System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000))
            {
                sr.color = new Color(1, 1, 1, 1);
                ranit = false;
            }
        }

        if (atingere)
        {
            jucator.GetComponent<Jucator>().primitDauna();
        }
    }
}
