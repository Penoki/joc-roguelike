using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportinator : MonoBehaviour
{

    public Transform destinatie;
    public Sprite deschisa, inchisa;
    public GameObject cameraDestinatie, cameraCurenta;
    private Collider2D coliziuneJ;

    private void Start()
    {
        cameraDestinatie = destinatie.transform.parent.transform.parent.gameObject;
        cameraCurenta = this.transform.parent.gameObject;
        pontareaUshii();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            coliziuneJ = collision;
            //teleportare jucator
            coliziuneJ.transform.position = destinatie.position;

            if (cameraDestinatie.GetComponent<detaliiIncapere>().tipIncapere == 'N' && !cameraDestinatie.GetComponent<detaliiIncapere>().completata)
            {
                //inchiderea ushilor
                cameraDestinatie.GetComponent<detaliiIncapere>().LockDown();

                //apelare functie dupa 10 milisecunde
                Invoke("MaterializareInamici", 0.01f);
            }
        }
    }

    //adaugare usa la detaliiIncapere
    public void pontareaUshii()
    {
        cameraCurenta.GetComponent<detaliiIncapere>().usi.Add(this.gameObject);
    }


    public void MaterializareInamici()
    {
        cameraDestinatie.GetComponent<MaterializareInamiciCN>().inamiciMaterializare();
    }
}
