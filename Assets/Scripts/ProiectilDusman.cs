using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProiectilDusman : MonoBehaviour
{
    private Rigidbody2D rbProiectil;
    public Vector3 tzinta;
    public Vector2 directie;
    public float vitProiectil;
    public float timpProiectil;
    bool asteptat;

    // Start is called before the first frame update
    void Start()
    {
        rbProiectil = this.GetComponent<Rigidbody2D>();
        //distrugere proiectil dupa distProiectil secunde
        Destroy(this.gameObject, timpProiectil);

        Invoke("Asteapta", 0.1f);
        //calcul directie
        directie = (Vector2)tzinta - rbProiectil.position;
        directie.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.GetComponent<Jucator>().primitDauna(1);
            Destroy(this.gameObject);
            return;
        }
        if (collision.transform.tag == "pereti" || collision.transform.tag == "obstacol")
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (asteptat)
        {
            rbProiectil.MovePosition(rbProiectil.position + directie * vitProiectil * Time.fixedDeltaTime);

        }
        //esecuri de deletat :))
        //rbProiectil.AddForce(directie * vitProiectil);
        //rbProiectil.MovePosition(rbProiectil.position + directie * vitProiectil * Time.fixedDeltaTime);
        //rbProiectil.position = Vector2.MoveTowards(rbProiectil.position, tzinta, vitProiectil * Time.deltaTime);
        //rbProiectil.MovePosition(rbProiectil.position + new Vector2(tzinta.x - decalaj, tzinta.y + decalaj) * vitProiectil * Time.fixedDeltaTime);
    }

    private void Asteapta()
    {
        asteptat = true;
    }
}
