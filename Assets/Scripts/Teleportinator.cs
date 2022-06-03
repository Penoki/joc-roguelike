using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportinator : MonoBehaviour
{
    public bool deschisa = true;
    public Transform destinatie;
    public GameObject cameraDestinatie;

    private void Start()
    {
        cameraDestinatie = destinatie.transform.parent.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && deschisa)
        {
            collision.transform.position = destinatie.position;
            cameraDestinatie.GetComponent<MaterializareInamiciCN>().inamiciMaterializare();
            //destinatie.GetComponentInParent<Teleportinator>().deschisa = true;
        }
    }

}
