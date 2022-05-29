using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cap : MonoBehaviour
{
    public Rigidbody2D capRB;
    public Animator capAnimator;
    public Sprite[] listaProiectile;
    public Jucator corp;
    Vector2 vectorMiscare,vectorDirectie; //vectorDirectie pentru a stii unde ataca

    //parametrii/proprietati ale proiectilului
    private float vitezaProiectil = 2.4f;
    private double daunaProiectil = 1.5f;
    private float distantaProiectil = 1.2f;
    private int indexProiectil = 0;
    private bool permisClipit = false;
    private bool proiectilSpate = false; //pentru ordinea de afisare sprite-uri
    public GameObject proiectilPrototip;
    public Transform[] pozitieCreareProiectil;
    public float rataDeClipire = 1f; //multiplicator al vitezei animatiei de clipire, respectiv al ratei de creare proiectil
    private float inclinareDirectie;


    //getter si setter pentru apelarea acestor variabile din alta clasa
    public float VProiectil { get => vitezaProiectil; set => vitezaProiectil = value; }
    public double DaunaProiectil { get => daunaProiectil; set => daunaProiectil = value; }
    public float DistantaProiectil { get => distantaProiectil; set => distantaProiectil = value; }
    public int IndexProiectil { get => indexProiectil; set => indexProiectil = value; }
    public void PermiteClipire() //functie apelata de eveniment declansat de animatii
    {
        permisClipit = true;
    }
    public void ProiectilStratOrdine() //modificare ordinea de afisare a proiectilului pentru a fi in spatele capului cand declansat acel AnimationEvent
    {
        proiectilSpate = true;
    }

    private void Start()
    {
        //update viteza toate animatii cap
        capAnimator.speed = 1.8f;
    }

    //Apelat o data per cadru
    void Update()
    {
        //calcul inclinare directie proiectil, in functie de viteza de miscare a jucatorului
        inclinareDirectie = corp.vitezaMiscare / 8;

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

        capAnimator.SetFloat("RataClipit", rataDeClipire);

        //cand input tastatura, atunci cream proiectil
        if ((vectorDirectie.x != 0 || vectorDirectie.y !=0) && permisClipit)
        {
            #region stabilire pozitie de instantiere
            int i = 0;
            if (vectorDirectie.y == 0 && vectorDirectie.x == 1)                         //strict dreapta
            {
                i = 2;                                              //pozitie creare dreapta
            }  
            else if (vectorDirectie.y == 0 && vectorDirectie.x == -1)                   //strict stanga
            {
                i = 3;                                              //pozitie creare stanga
            }
            else if (vectorDirectie.y == 1)                                             //sus
            {
                i = 1;                                              //pozitie creare sus
                vectorDirectie.x = 0;//anulare miscare diagonala
            }
            else                                                                        //jos
            {
                i = 0;                                              //pozitie creare jos
                vectorDirectie.x = 0;//anulare miscare diagonala
            }
            #endregion

            #region stabilire inclinare directie in functie de miscarea jucatorului
            if (vectorMiscare.y == 1 && vectorDirectie.y==0)            //daca se misca sus si nu impusca sus jos
            {
                vectorDirectie.y += inclinareDirectie;          //inclinare in sus
            }
            else if (vectorMiscare.y == -1 && vectorDirectie.y == 0)    //daca se misca jos si nu impusca sus jos
            {
                vectorDirectie.y -= inclinareDirectie;          //inclinare in jos
            }
            else if (vectorMiscare.x == 1 && vectorDirectie.x == 0)     //daca se misca dreapta si nu impusca stanga dreapta
            {
                vectorDirectie.x += inclinareDirectie;          //inclinare in dreapta
            }
            else if(vectorMiscare.x == -1 && vectorDirectie.x == 0)     //daca se misca stanga si nu impusca stanga dreapta
            {
                vectorDirectie.x -= inclinareDirectie;          //inclinare in stanga
            }
            #endregion

            CreareProiectil(listaProiectile[indexProiectil], capRB, vectorDirectie, vitezaProiectil, daunaProiectil, distantaProiectil, pozitieCreareProiectil[i]);
            permisClipit = false;
            proiectilSpate = false;
        }
    }
    void CreareProiectil(Sprite aspect, Rigidbody2D capJucator, Vector2 directie, float viteza, double dauna, float distanta, Transform poz)
    {
        //creare obiect proiectil nou la pozitia specificata in unity
        GameObject proiectilNou = Instantiate(proiectilPrototip, poz.position, poz.rotation) as GameObject;

        //transmitere valori ale proprietatilor catre obiectul nou instantiat
        proiectilNou.GetComponent<Ochi>().CatDauna = dauna;
        proiectilNou.GetComponent<Ochi>().VitProiectil = viteza;
        proiectilNou.GetComponent<Ochi>().VectorMiscareOchi = directie;
        proiectilNou.GetComponent<Ochi>().DistProiectil = distanta;
        proiectilNou.GetComponent<SpriteRenderer>().sprite = aspect;
        if (proiectilSpate) { proiectilNou.GetComponent<SpriteRenderer>().sortingOrder = -1; }
    }
}
