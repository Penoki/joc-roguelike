using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marioneta : MonoBehaviour
{
    public SpriteRenderer sr;
    public double sanatate = 10;
    public double ranireRacireSec = 1.4;
    public float timpPauza = 1.5f;
    private int msecundaRanit, secundaRanit;
    public GameObject jucator, cameraCurenta;
    public bool ranit = false, atingere = false, obturat = false;

    private void Start()
    {
        jucator = GameObject.FindWithTag("Player");
        cameraCurenta = this.transform.parent.gameObject;
        pontareaInamicului();
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
            Distruge();
        }
    }

    public void Distruge()
    {
        GameObject Astea = this.GetComponent<Astelutza>().Astea;
        Destroy(Astea);
        cameraCurenta.GetComponent<detaliiIncapere>().inamici.Remove(this.gameObject);

        //daca nu mai sunt deloc inamici in viata
        if (cameraCurenta.GetComponent<detaliiIncapere>().inamici.Count <= 0)
        {
            //deschidem usile
            cameraCurenta.GetComponent<detaliiIncapere>().OpenUp();

            //marcam camera completata
            cameraCurenta.GetComponent<detaliiIncapere>().completata = true;
        }

        Destroy(this.gameObject);
    }

    //adaugare inamic la detaliiIncapere
    public void pontareaInamicului()
    {
        cameraCurenta.GetComponent<detaliiIncapere>().inamici.Add(this.gameObject);
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
            //pauza dupa fiecare atac
            this.GetComponent<Astelutza>().enabled = false;
            Invoke("GataPauza", timpPauza);

            jucator.GetComponent<Jucator>().primitDauna();
        }

        verificaJucatorObstacol();
        if (obturat && this.GetComponent<Astelutza>().enabled == true)
        {
            this.GetComponent<Astelutza>().enabled = false;
        }
        else if(!obturat && this.GetComponent<Astelutza>().enabled == false)
        {
            this.GetComponent<Astelutza>().enabled = true;
        }
    }

    public void GataPauza()
    {
        this.GetComponent<Astelutza>().enabled = true;
    }

    public void Pauza()
    {
        this.GetComponent<Astelutza>().enabled = false;
    }

    //verificare jucator daca se afla/zboara deasupra unui obstacol
    public void verificaJucatorObstacol()
    {
        Vector2 ju = jucator.transform.position - cameraCurenta.transform.position;
        float N = cameraCurenta.GetComponent<detaliiIncapere>().N;
        float[,] g = cameraCurenta.GetComponent<detaliiIncapere>().grilajCamera;
        if (g[(int)(ju.x + (N - 1) / 2), (int)(ju.y + (N - 1) / 2)] == 1f)
        {
            obturat = true;
            Debug.Log("DA");
        }
        else
        {
            obturat = false;
            Debug.Log("NU");
        }

    }
}
