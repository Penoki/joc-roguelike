using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cap : MiscareJucator
{
    public Animator capAnimator;
    Vector2 vectorMiscare;
    //Apelat o data per cadru
    void Update()
    {
        vectorMiscare.x = Input.GetAxisRaw("Horizontal"); //sageata stanga -1, dreapta 1
        vectorMiscare.y = Input.GetAxisRaw("Vertical"); //sus 1, jos -1

        capAnimator.SetFloat("Orizontal", vectorMiscare.x);
        capAnimator.SetFloat("Vertical", vectorMiscare.y);
        //viteza pe diagonala sa fie aceeasi ca si pe orizontal sau vertical
        if (vectorMiscare.x != 0 && vectorMiscare.y != 0)
        {
            capAnimator.SetFloat("Viteza", vectorMiscare.sqrMagnitude - 1);
        }
        else
        {
            capAnimator.SetFloat("Viteza", vectorMiscare.sqrMagnitude);
        }

    }
}
