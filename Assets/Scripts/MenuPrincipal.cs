using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    //transmitere samanta daca aceasta a fost introdusa
    public TMP_InputField inputCamp;
    public void StartJoc()
    {
        if (string.IsNullOrEmpty(inputCamp.text))
        {
            Samanta.introdusa = false;
        }
        else
        {
            Samanta.introdusa = true;
            Samanta.samanta = int.Parse(inputCamp.text);
        }

        // in caz de ESC-> Meniu Principal -> Start, sa nu ramana pauza
        if (Time.timeScale == 0) Time.timeScale = 1;

        //schimbare scena cu cea de joc
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitJoc()
    {
        Application.Quit();
        //test pentru a vedea in editor daca merge
        //Debug.Log("EXIT");
    }

    public void BackMeniu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
