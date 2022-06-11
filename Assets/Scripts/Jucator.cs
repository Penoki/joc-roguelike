using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jucator : MonoBehaviour
{
    public float vitezaMiscare = 1.7f;
    public int pctSanatate = 8, pctSanatateMax=8;
    public int timpInvincibil = 1;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer srJ, srC;
    public static Color srNcolor;
    private int msecundaRanit, secundaRanit, minutRanire, secundaRanire;
    public bool tpPauza = false;
    private bool ranit = false, ranire = false;

    Vector2 vectorMiscare;

    private void Start()
    {
        srNcolor = new Color(1, 1, 1, 1);

    }

    //Apelat o data per cadru
    void Update()
    {
        //ascultare taste W A S D
        vectorMiscare.x = Input.GetAxisRaw("Horizontal"); //A -1, D 1
        vectorMiscare.y = Input.GetAxisRaw("Vertical"); //W 1, S -1

        //setare parametrii care dicteaza animatia curenta
        animator.SetFloat("Orizontal", vectorMiscare.x);
        animator.SetFloat("Vertical", vectorMiscare.y);
        animator.SetFloat("Miscare", vectorMiscare.sqrMagnitude);
        animator.SetFloat("Viteza", vitezaMiscare);

        //rezolvare aceeasi viteza pe miscare pe diagonala
        if (vectorMiscare.x != 0 && vectorMiscare.y != 0)
        {
            vectorMiscare.Normalize();
        }

        if (ranit)
        {
            if ((msecundaRanit + secundaRanit * 1000) <= (System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000))
            {
                srJ.color = srNcolor;
                srC.color = srNcolor;
                ranit = false;
            }
        }

        if (ranire)
        {
            if ((secundaRanire + minutRanire * 60) <= (System.DateTime.Now.Second + System.DateTime.Now.Minute * 60))
            {
                ranire = false;
            }
        }
    }

    //FixedUpdate este mai de incredere pentru fizici
    //este apelata 50 de ori pe secunda
    void FixedUpdate()
    {
        if(!tpPauza)
        rb.MovePosition(rb.position + vectorMiscare * vitezaMiscare * Time.fixedDeltaTime);
    }

    public void primitDauna(int dauna)
    {
        if (!ranire)
        {
            //punctaj --
            Punctaj.Punctare -= 50;

            ranire = true;
            pctSanatate-=dauna;
            ranit = true;
            srJ.color = new Color(1, 0.6f, 0.6f, 1);
            srC.color = new Color(1, 0.6f, 0.6f, 1);
            msecundaRanit = System.DateTime.Now.Millisecond + 400;
            secundaRanit = System.DateTime.Now.Second;
            minutRanire = System.DateTime.Now.Minute;
            secundaRanire = System.DateTime.Now.Second + timpInvincibil;
            if (secundaRanire >= 60)
            {
                secundaRanire = secundaRanire % 60;
                minutRanire++;
                if (minutRanire >= 60)
                {
                    minutRanire = minutRanire % 60;
                }
            }
            if (msecundaRanit >= 1000)
            {
                msecundaRanit = msecundaRanit % 1000;
                secundaRanit++;
                if (secundaRanit >= 60)
                {
                    secundaRanit = secundaRanit % 60;
                }
            }

            //dispari cand nu mai ai viata
            if (pctSanatate <= 0)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}
