using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetareNivel : MonoBehaviour
{
    public Sprite deschis, inchis;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.GetComponent<SpriteRenderer>().sprite = inchis;
            Invoke("RestartNivel", 1f);
        }
    }

    //resetare nivel
    public void RestartNivel()
    {
        Samanta.samanta ++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
