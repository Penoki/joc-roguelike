using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetareNivel : MonoBehaviour
{
    public Sprite deschis, inchis;
    private GameObject jucator;
    private bool nuDeasupra;

    private void Start()
    {
        nuDeasupra = false;
        jucator = GameObject.FindGameObjectWithTag("Player");
        float distantaMin = jucator.GetComponent<CapsuleCollider2D>().size.x / 2 + this.GetComponent<CapsuleCollider2D>().size.x / 2;
        if (Vector2.Distance(jucator.transform.position, this.transform.position) > distantaMin)
        {
            nuDeasupra = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && nuDeasupra)
        {
            this.GetComponent<SpriteRenderer>().sprite = inchis;
            Invoke("RestartNivel", 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !nuDeasupra)
        {
            nuDeasupra = true;
        }
    }

    //resetare nivel
    public void RestartNivel()
    {
        Samanta.samanta++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
