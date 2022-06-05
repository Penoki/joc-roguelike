using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalDist : MonoBehaviour
{
    public LayerMask blocadaMasca;
    bool alert = false, racire = false;
    public float timpRacire = 2f;
    private GameObject inamicParinte, jucator;
    public List<Transform> pozitiiMaterializareProiectil;
    public ProiectilDusman nuca;
    public float vitProiectil = 1;
    public float timpProiectil = 6;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            alert = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            alert = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        jucator = GameObject.Find("Jucator");
        inamicParinte = this.transform.parent.gameObject;
        pozitiiMaterializareProiectil.Add(inamicParinte.transform.Find("PozFata").transform);
        pozitiiMaterializareProiectil.Add(inamicParinte.transform.Find("PozSpate").transform);
        pozitiiMaterializareProiectil.Add(inamicParinte.transform.Find("PozStanga").transform);
        pozitiiMaterializareProiectil.Add(inamicParinte.transform.Find("PozDreapta").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (alert && !racire)
        {
            RaycastHit2D hit = Physics2D.Linecast(this.transform.parent.position, jucator.transform.position, blocadaMasca);
            if (!hit)
            {
                racire = true;
                Transform pozStart = this.transform;
                //determinare pozitia cea mai apropiata de jucator
                foreach (Transform a in pozitiiMaterializareProiectil)
                {
                    //daca distanta e mai mica catre jucator, atunci punctul acela este mai aproape
                    if (Vector3.Distance(pozStart.position, jucator.transform.position) > Vector3.Distance(a.position, jucator.transform.position))
                    {
                        pozStart = a;
                    }
                }

                MaterializareProiectil(pozStart);
                Invoke("ResetRacire", timpRacire);
            }
        }
    }

    //functie pentru materializarea proiectilului
    public void MaterializareProiectil(Transform start)
    {
        
        ProiectilDusman nouProiectil = Instantiate(nuca, start.position, start.rotation);

        nouProiectil.timpProiectil = timpProiectil;
        nouProiectil.vitProiectil = vitProiectil;
        nouProiectil.tzinta = jucator.transform.position;
        nouProiectil.GetComponent<Rigidbody2D>().position = start.position;
    }

    public void ResetRacire()
    {
        racire = false;
    }
}
