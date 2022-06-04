using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ochi : MonoBehaviour
{
    private Rigidbody2D rbProiectil;
    private float vitProiectil;
    private float distProiectil;
    private double catDauna;
    private bool patrunzator = false;
    Vector2 vectorMiscareOchi;

    //getter si setter pentru apelarea acestor variabile din alta clasa
    public Rigidbody2D RbProiectil { get => rbProiectil; set => rbProiectil = value; }
    public float VitProiectil { get => vitProiectil; set => vitProiectil = value; }
    public Vector2 VectorMiscareOchi { get => vectorMiscareOchi; set => vectorMiscareOchi = value; }
    public float DistProiectil { get => distProiectil; set => distProiectil = value; }
    public double CatDauna { get => catDauna; set => catDauna = value; }

    // Start is called before the first frame update
    void Start()
    {
        rbProiectil = this.GetComponent<Rigidbody2D>();

        //distrugere proiectil dupa distProiectil secunde
        Destroy(this.gameObject, distProiectil);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!patrunzator && (collision.transform.tag == "pereti" || collision.transform.tag == "inamic" || collision.transform.tag == "obstacol"))
        {
            Destroy(this.gameObject);
        }
    }
    
    //FixedUpdate este mai de incredere pentru fizici
    //este apelata 50 de ori pe secunda
    void FixedUpdate()
    {
        RbProiectil.MovePosition(RbProiectil.position + VectorMiscareOchi * VitProiectil * Time.fixedDeltaTime);
    }

    
}
