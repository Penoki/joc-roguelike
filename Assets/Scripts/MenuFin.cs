using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuFin : MonoBehaviour
{
    public TMP_Text txtPct;

    void Start()
    {
        txtPct.text += Punctaj.Punctare;
        Punctaj.Punctare = 0;
    }
    public void ExitJoc()
    {
        Application.Quit();
        //test pentru a vedea in editor daca merge
        //Debug.Log("EXIT");
    }

    public void BackMeniu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
