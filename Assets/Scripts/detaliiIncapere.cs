using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detaliiIncapere : MonoBehaviour
{
    public List<Vector2> obstacoleCurente, puncteMaterializare;
    public List<GameObject> usi, inamici;
    public char tipIncapere;
    public float N; //dimensiunea incaperii
    public bool completata = false;

    //obstacole = 1, teren liber = 0
    //initializare cu 0
    public float[,] grilajCamera = new float[,] { { 0f, 0f, 0f, 0f, 0f, 0f, 0f},
                                              { 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                                              { 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                                              { 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                                              { 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                                              { 0f, 0f, 0f, 0f, 0f, 0f, 0f },
                                              { 0f, 0f, 0f, 0f, 0f, 0f, 0f } };


    private void Start()
    {
        //dimensiune incapere consacrata
        N = 7;
        if (puncteMaterializare != null && tipIncapere == 'N')
        {
            this.gameObject.AddComponent<MaterializareInamiciCN>();
            this.GetComponent<MaterializareInamiciCN>().puncteMaterializare = puncteMaterializare;
        }
    }

    public void ActualizareGrilaCamera()
    {
        foreach (Vector2 ob in obstacoleCurente)
        {
            //think about it
            grilajCamera[(int)(ob.x + (N - 1) / 2), (int)(ob.y + (N - 1) / 2)] = 1f;
        }
    }

    //functie pentru inchiderea tuturor ushilor din incapere
    public void LockDown()
    {
        foreach (GameObject usa in usi)
        {
            usa.GetComponent<Collider2D>().enabled = false;
            usa.GetComponent<SpriteRenderer>().sprite = usa.GetComponent<Teleportinator>().inchisa;
        }
    }

    //functie pentru deschiderea tuturor ushilor din incapere
    public void OpenUp()
    {
        foreach (GameObject usa in usi)
        {
            usa.GetComponent<Collider2D>().enabled = true;
            usa.GetComponent<SpriteRenderer>().sprite = usa.GetComponent<Teleportinator>().deschisa;
        }
    }

}
