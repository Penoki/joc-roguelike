using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void StartJoc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitJoc()
    {
        Application.Quit();
        //test pentru a vedea in editor daca merge
        //Debug.Log("EXIT");
    }
}
