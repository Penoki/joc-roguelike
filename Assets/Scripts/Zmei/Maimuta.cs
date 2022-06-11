using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maimuta : MonoBehaviour
{
    public LayerMask blocadaMasca;
    private System.Random zar = new System.Random();
    public Rigidbody2D rb;
    public GameObject jucator, cameraCurenta;
    public List<Sprite> monke;
    public bool verifica, ranit, urmarire;
    public double sanatate, santateMax;
    private float timpPauzaVerif, timpUrmarire, viteza;
    public float pragDistanta, distanta;

    //proprietati proiectil
    private float vitProiectil = 2.4f;
    private float timpProiectil = 3;
    public ProiectilDusman nuca;

    void Start()
    {
        timpUrmarire = 3f;
        timpPauzaVerif = 1.4f;
        viteza = 2f;
        verifica = true;
        ranit = false;

        //luare referinte
        jucator = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();

        //luare referinta camera
        cameraCurenta = this.transform.parent.gameObject;

        //calcul pragDistanta
        pragDistanta = jucator.GetComponent<CapsuleCollider2D>().size.x / 2 + this.GetComponent<CapsuleCollider2D>().size.x / 2 + 0.1f;

    }

    void FixedUpdate()
    {
        if (urmarire)
        {
            rb.position = Vector2.MoveTowards(rb.position, jucator.GetComponent<Rigidbody2D>().position, viteza * Time.deltaTime);
            if (pragDistanta >= distanta)
            {
                urmarire = false;
                Debug.Log("AtacDupaUrmarire");
                //atac de aproape 1
                jucator.GetComponent<Jucator>().primitDauna(1);

                //modificat aparenta monke
                SchimbAspect1();
                Invoke("SchimbAspect2", 0.3f);
            }
        }
    }

    void Update()
    {
        distanta = CalcDistanta(jucator.transform.position, this.transform.position);
        //verificarea distantei dintre maimuta si jucator
        if (verifica && !urmarire)
        {
            verifica = false;

            if (pragDistanta >= distanta)
            {
                //aproape
                if (sanatate > santateMax * 30 / 100)
                {
                    if (!IsInvoking("SchimbAspect2"))
                    {
                        //atac de aproape 1
                        Debug.Log("AtacAproape1");
                        jucator.GetComponent<Jucator>().primitDauna(1);

                        //modificat aparenta monke
                        SchimbAspect1();
                        Invoke("SchimbAspect2", 0.3f);
                    }
                }
                else
                {
                    if (!IsInvoking("SchimbAspect2"))
                    {
                        //atac de aproape 2
                        Debug.Log("AtacAproape2");
                        jucator.GetComponent<Jucator>().primitDauna(2);

                        //modificat aparenta monke
                        SchimbAspect1();
                        Invoke("SchimbAspect2", 0.3f);
                    }
                }
            }
            else
            {
                //departe
                if (sanatate > santateMax * 30 / 100)
                {
                    //dat cu zarul
                    if (zar.Next(1, 100) <= 60)
                    {
                        //atac la distanta 1
                        Debug.Log("AtacDistanta1");
                        AtacLaDistanta(false);
                    }
                    else
                    {
                        //deplasare catre jucator
                        urmarire = true;
                        if (IsInvoking("PlictisitUrmarit")) { CancelInvoke("PlictisitUrmarit"); }
                        Invoke("PlictisitUrmarit", timpUrmarire);
                    }
                }
                else
                {
                    //dat cu zarul
                    if (zar.Next(1, 100) <= 60)
                    {
                        //atac la distanta 2
                        Debug.Log("AtacDistanta2");
                        AtacLaDistanta(true);
                    }
                    else
                    {
                        //deplasare catre jucator
                        urmarire = true;
                        if (IsInvoking("PlictisitUrmarit")) { CancelInvoke("PlictisitUrmarit"); }
                        Invoke("PlictisitUrmarit", timpUrmarire - 1f);
                    }
                }
            }
            if (IsInvoking("PauzaVerificare")) { CancelInvoke("PauzaVerificare"); }
            Invoke("PauzaVerificare", timpPauzaVerif);
        }

        if (ranit)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            ranit = false;
        }
    }

    //functie de calculare distanta jucator inamic
    public float CalcDistanta(Vector2 A, Vector2 B)
    {
        //Debug.Log(Vector2.Distance(A, B));
        return Vector2.Distance(A, B);
    }

    public void PauzaVerificare()
    {
        verifica = true;
    }

    public void PlictisitUrmarit()
    {
        urmarire = false;
    }

    public void SchimbAspect1()
    {
        this.GetComponent<SpriteRenderer>().sprite = monke[1];
    }

    public void SchimbAspect2()
    {
        this.GetComponent<SpriteRenderer>().sprite = monke[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "ostil")
        {
            if (IsInvoking("vulnerabil")) { CancelInvoke("vulnerabil"); }
            Invoke("vulnerabil", 0.88f);

            dauna(collision.GetComponent<Ochi>().CatDauna);
        }
    }

    //functie de a il face vulnerabil vizual doar
    public void vulnerabil()
    {
        ranit = true;
    }

    private void dauna(double catdemult)
    {
        sanatate -= catdemult;
        if (sanatate <= santateMax * 30 / 100 && viteza == 2f) { viteza = 3.2f; }

        this.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0.6f, 1);

        if (sanatate <= 0)
        {
            Distruge();
        }
    }

    //atac ranged
    public void AtacLaDistanta(bool dificil)
    {
        RaycastHit2D hit = Physics2D.Linecast(this.transform.position, jucator.transform.position, blocadaMasca);
        if (!hit)
        {
            Transform pozStart = this.transform;

            if (!dificil)
                MaterializareProiectil1(pozStart);
            else
                MaterializareProiectil2(pozStart);

            //modificat aparenta monke
            SchimbAspect1();
            Invoke("SchimbAspect2", 0.3f);
        }
    }

    //functie pentru materializarea proiectilului1
    public void MaterializareProiectil1(Transform start)
    {

        ProiectilDusman nouProiectil = Instantiate(nuca, start.position, start.rotation);

        nouProiectil.timpProiectil = timpProiectil;
        nouProiectil.vitProiectil = vitProiectil;
        nouProiectil.tzinta = jucator.transform.position;
        nouProiectil.GetComponent<Rigidbody2D>().position = start.position;
    }

    //functie pentru materializarea proiectilului2
    public void MaterializareProiectil2(Transform start)
    {

        ProiectilDusman nouProiectil = Instantiate(nuca, start.position, start.rotation);

        nouProiectil.timpProiectil = timpProiectil + 2f;
        nouProiectil.vitProiectil = vitProiectil + 2f;
        nouProiectil.tzinta = jucator.transform.position;
        nouProiectil.GetComponent<Rigidbody2D>().position = start.position;
    }

    public void Distruge()
    {
        cameraCurenta.GetComponent<detaliiIncapere>().inamici.Remove(this.gameObject);

        //daca nu mai sunt deloc inamici in viata
        if (cameraCurenta.GetComponent<detaliiIncapere>().inamici.Count <= 0)
        {
            //deschidem usile
            cameraCurenta.GetComponent<detaliiIncapere>().OpenUp();

            //marcam camera completata
            cameraCurenta.GetComponent<detaliiIncapere>().Invoke("CompletareCamera", 1);

            //cream buton
            cameraCurenta.GetComponent<CameraFinala>().CreareButon();
        }

        //adaugare punctaj
        Punctaj.Punctare = Punctaj.Punctare + 800;
        Debug.Log(Punctaj.Punctare);

        Destroy(this.gameObject);
    }

    //adaugare inamic la detaliiIncapere
    public void pontareaInamicului()
    {
        cameraCurenta.GetComponent<detaliiIncapere>().inamici.Add(this.gameObject);
    }
}
