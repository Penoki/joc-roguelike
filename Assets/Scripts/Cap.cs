using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cap : MonoBehaviour
{
    public Rigidbody2D capRB;
    public Animator capAnimator;
    public Sprite[] listaProiectile;
    Vector2 vectorMiscare,vectorDirectie; //vectorDirectie pentru a stii unde ataca

    //parametrii/proprietati ale proiectilului
    private float vitezaProiectil = 1.2f;
    private double daunaProiectil = 1.5f;
    private float distantaProiectil = 2;
    private int indexProiectil = 0;
    public float timpRacire = 0.4f;
    private float timpUrmatorClipit = 0;
    public GameObject proiectilPrototip;
    public Transform[] pozitieCreareProiectil;

    //getter si setter pentru apelarea acestor variabile din alta clasa
    public float VProiectil { get => vitezaProiectil; set => vitezaProiectil = value; }
    public double DaunaProiectil { get => daunaProiectil; set => daunaProiectil = value; }
    public float DistantaProiectil { get => distantaProiectil; set => distantaProiectil = value; }
    public int IndexProiectil { get => indexProiectil; set => indexProiectil = value; }

    //Apelat o data per cadru
    void Update()
    {
        //update viteza clipit
        capAnimator.speed = vitezaProiectil;

        //ascultare taste W A S D
        vectorMiscare.x = Input.GetAxisRaw("Horizontal"); //A -1, D 1
        vectorMiscare.y = Input.GetAxisRaw("Vertical"); //W 1, S -1
        //ascultare taste sageti
        vectorDirectie.x = Input.GetAxisRaw("FireHorizontal"); //sageata stanga -1, dreapta 1
        vectorDirectie.y = Input.GetAxisRaw("FireVertical"); //sus 1, jos -1

        //setare parametrii care dicteaza animatia curenta
        capAnimator.SetFloat("Orizontal", vectorMiscare.x);
        capAnimator.SetFloat("Vertical", vectorMiscare.y);
        capAnimator.SetFloat("Viteza", vectorMiscare.sqrMagnitude);

        capAnimator.SetFloat("FOrizontal", vectorDirectie.x);
        capAnimator.SetFloat("FVertical", vectorDirectie.y);
        capAnimator.SetFloat("FViteza", vectorDirectie.sqrMagnitude);

        //cand input tastatura, atunci cream proiectil
        if ((vectorDirectie.x != 0 || vectorDirectie.y !=0) && Time.time>timpUrmatorClipit)
        {
            #region stabilire pozitie de instantiere si directie
            int i = 0;
            if (vectorDirectie.y == 0 && vectorDirectie.x == 1)        //dreapta
            {
                i = 2;
            }  
            else if (vectorDirectie.y == 0 && vectorDirectie.x == -1)  //stanga
            {
                i = 3;
            }
            else if (vectorDirectie.y == 1)                            //spate
            {
                i = 1;
                vectorDirectie.x = 0;
            }
            else                                                       //fata
            {
                i = 0;
                vectorDirectie.x = 0;
            }
            #endregion
            CreareProiectil(listaProiectile[indexProiectil], capRB, vectorDirectie, vitezaProiectil, daunaProiectil, distantaProiectil, pozitieCreareProiectil[i]);
            timpUrmatorClipit = Time.time + timpRacire;
        }
    }
    void CreareProiectil(Sprite aspect, Rigidbody2D capJucator, Vector2 directie, float viteza, double dauna, float distanta, Transform poz)
    {
        //creare obiect proiectil nou la pozitia specificata in unity
        GameObject proiectilNou = Instantiate(proiectilPrototip, poz.position, poz.rotation) as GameObject;

        //transmitere valori ale proprietatilor catre obiectul nou instantiat
        proiectilNou.GetComponent<Ochi>().VitProiectil = viteza;
        proiectilNou.GetComponent<Ochi>().VectorMiscareOchi = directie;
        proiectilNou.GetComponent<Ochi>().DistProiectil = distanta;
        proiectilNou.GetComponent<SpriteRenderer>().sprite = aspect;

    }
}
