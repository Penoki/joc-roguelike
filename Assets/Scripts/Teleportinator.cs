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
            //nu lasam jucatorul sa se miste un timp de 10 milisecunde
            coliziuneJ.GetComponent<Jucator>().enabled = false;
            //apelare functie dupa 100 milisecunde
            Invoke("taxaDeDrumAchitata", 0.15f);

            //transmitere tag cameraCurenta
            cameraCurenta.tag = "Untagged";
            cameraDestinatie.tag = "cameraCurenta";

            if (cameraDestinatie.GetComponent<detaliiIncapere>().tipIncapere == 'N' && !cameraDestinatie.GetComponent<detaliiIncapere>().completata)
            {
                //inchiderea ushilor
                cameraDestinatie.GetComponent<detaliiIncapere>().LockDown();

                //apelare functie dupa 200 milisecunde
                Invoke("MaterializareInamici", 0.15f);
            }
        }
    }

    //adaugare usa la detaliiIncapere
    public void pontareaUshii()
    {
        cameraCurenta.GetComponent<detaliiIncapere>().usi.Add(this.gameObject);
    }

    //permitere jucator sa se miste
    public void taxaDeDrumAchitata()
    {
        coliziuneJ.GetComponent<Jucator>().enabled = true;
    }

    public void MaterializareInamici()
    {
        cameraDestinatie.GetComponent<MaterializareInamiciCN>().inamiciMaterializare();
    }
}
