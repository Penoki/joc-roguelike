using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfataUtilizator : MonoBehaviour
{
    public GameObject harta,jucator;
    public Text txtVital, txtPunctaj;

    private void Start()
    {
        ActualizareVital();
        ActualizarePunctaj();
    }

    void Update()
    {
        //intrerupator minimap
        if (Input.GetKeyDown(KeyCode.M))
        {
            harta.SetActive(!harta.activeSelf);
        }
    }

    public void ActualizareVital()
    {
        txtVital.text = "Vitalitate: " + jucator.GetComponent<Jucator>().pctSanatate + "/" + jucator.GetComponent<Jucator>().pctSanatateMax;
    }

    public void ActualizarePunctaj()
    {
        txtPunctaj.text = "Punctaj: " + Punctaj.Punctare;
    }
}
