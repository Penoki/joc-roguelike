using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astelutza : MonoBehaviour
{
    public GameObject tzinta; //jucatorul in cazul de fata
    public Rigidbody2D rb;
    public int[,] grilajCurent;  //camera cu obstacole

    private void Start()
    {
        grilajCurent = this.gameObject.GetComponentInParent<detaliiIncapere>().grilajCamera;
    }

    private void FixedUpdate()
    {
        float x, y, xj, yj, marja = 0.03f, viteza = 55f;
        xj = tzinta.GetComponent<Rigidbody2D>().position.x;
        yj = tzinta.GetComponent<Rigidbody2D>().position.y;
        //realizare miscare catre jucator
        if (rb.position.x <= xj + marja && rb.position.x >= xj - marja)
        {
            x = 0f;
        }
        else if (rb.position.x < xj)
        {
            x = 0.01f;
        }
        else
        {
            x = -0.01f;
        }

        if (rb.position.y <= yj + marja && rb.position.y >= yj - marja)
        {
            y = 0f;
        }
        else if (rb.position.y < yj)
        {
            y = 0.01f;
        }
        else
        {
            y = -0.01f;
        }

        rb.MovePosition(rb.position + new Vector2(x, y) * viteza * Time.fixedDeltaTime);
    }
}
