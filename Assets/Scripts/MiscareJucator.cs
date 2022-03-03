using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscareJucator : MonoBehaviour
{
    public float vitezaMiscare = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 vectorMiscare;

    //Apelat o data per cadru
    void Update()
    {
        vectorMiscare.x = Input.GetAxisRaw("Horizontal"); //sageata stanga -1, dreapta 1
        vectorMiscare.y = Input.GetAxisRaw("Vertical"); //sus 1, jos -1

        animator.SetFloat("Orizontal", vectorMiscare.x);
        animator.SetFloat("Vertical", vectorMiscare.y);
        //viteza pe diagonala sa fie aceeasi ca si pe orizontal sau vertical
        if(vectorMiscare.x!=0 && vectorMiscare.y!=0)
        {
            animator.SetFloat("Viteza", vectorMiscare.sqrMagnitude-1);
        }
        else
        {
            animator.SetFloat("Viteza", vectorMiscare.sqrMagnitude);
        }

    }

    //FixedUpdate este mai de incredere pentru fizici
    //este apelata 50 de ori pe secunda
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + vectorMiscare * vitezaMiscare * Time.fixedDeltaTime);
    }
}
