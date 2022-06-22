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
        //resetare punctaj
        Punctaj.Punctare = 0;
        if (string.IsNullOrEmpty(inputCamp.text))
        {
            Samanta.introdusa = false;
        }
        else
        {
            if (double.Parse(inputCamp.text) <= int.MaxValue && double.Parse(inputCamp.text) >= int.MinValue)
            {
                Samanta.introdusa = true;
                Samanta.samanta = int.Parse(inputCamp.text);
            }
        }

        // in caz de ESC-> Meniu Principal -> Start
        if (Time.timeScale == 0)
        {
            //sa nu ramana pauza
            Time.timeScale = 1;
            PauzaJoc.apasat = false;
        }

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
