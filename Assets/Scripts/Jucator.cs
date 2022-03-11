using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jucator : MonoBehaviour
{
    public float vitezaMiscare = 2.4f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 vectorMiscare; 
    //Apelat o data per cadru
    void Update()
    {
        //ascultare taste W A S D
        vectorMiscare.x = Input.GetAxisRaw("Horizontal"); //A -1, D 1
        vectorMiscare.y = Input.GetAxisRaw("Vertical"); //W 1, S -1

        //setare parametrii care dicteaza animatia curenta
        animator.SetFloat("Orizontal", vectorMiscare.x);
        animator.SetFloat("Vertical", vectorMiscare.y);
        animator.SetFloat("Viteza", vectorMiscare.sqrMagnitude);

        //rezolvare aceeasi viteza pe miscare pe diagonala
        if (vectorMiscare.x != 0 && vectorMiscare.y != 0)
        {
            vectorMiscare.Normalize();
        }
    }

    //FixedUpdate este mai de incredere pentru fizici
    //este apelata 50 de ori pe secunda
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + vectorMiscare * vitezaMiscare * Time.fixedDeltaTime);
    }
}
