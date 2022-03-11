using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cap : MonoBehaviour
{
    public Rigidbody2D capRB;
    public Animator capAnimator;
    public Sprite[] listaProiectile;
    Vector2 vectorMiscare,vectorDirectie; //vectorDirectie pentru a stii unde ataca
    private float vitezaProiectil = 2.5f;
    private double daunaProiectil = 1.5f;
    private int distantaProiectil = 1;
    private int indexProiectil = 0;
    public float rataProiectile = 1;

    //getter si setter pentru apelarea acestor variabile din alta clasa
    public float VProiectil { get => vitezaProiectil; set => vitezaProiectil = value; }
    public double DaunaProiectil { get => daunaProiectil; set => daunaProiectil = value; }
    public int DistantaProiectil { get => distantaProiectil; set => distantaProiectil = value; }
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
        if (vectorDirectie.x != 0 || vectorDirectie.y !=0)
        {
            
            CreareProiectil(listaProiectile[indexProiectil], capRB, vectorDirectie, vitezaProiectil, daunaProiectil, distantaProiectil);
            
        }
    }

    void CreareProiectil(Sprite aspect, Rigidbody2D capJucator, Vector2 directie, float viteza, double dauna, int distanta)
    {
        //creare obiect nou gol
        GameObject ochi = new GameObject();

        //aici se vede cel mai bine folosirea getter, setter din clasa Ochi
        Ochi ochiNou = ochi.AddComponent<Ochi>();
        ochiNou.VitProiectil = viteza;
        ochiNou.VectorMiscareOchi = directie;
        ochiNou.DistProiectil = distanta;

        //adaugare componenta si setare pozitie in functie de jucator
        Rigidbody2D pozitieOchi = ochi.AddComponent<Rigidbody2D>();
        pozitieOchi.position = capJucator.position;

        //adaugare componenta si setare sprite
        SpriteRenderer aspectOchi = ochi.AddComponent<SpriteRenderer>();
        aspectOchi.sprite = aspect;

        
    }

}
