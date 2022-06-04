using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detaliiIncapere : MonoBehaviour
{
    public List<Vector2Int> obstacoleCurente, puncteMaterializare;
    public List<GameObject> usi, inamici;
    public char tipIncapere;
    public int N; //dimensiunea incaperii
    public bool completata = false;

    //obstacole = 1, teren liber = 0
    //initializare cu 0
    public int[,] grilajCamera = new int[,] { { 0, 0, 0, 0, 0, 0, 0},
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 } };


    private void Start()
    {
        if (puncteMaterializare != null && tipIncapere == 'N')
        {
            this.gameObject.AddComponent<MaterializareInamiciCN>();
            this.GetComponent<MaterializareInamiciCN>().puncteMaterializare = puncteMaterializare;
        }
    }

    public void ActualizareGrilaCamera()
    {
        foreach (Vector2Int ob in obstacoleCurente)
        {
            //think about it
            grilajCamera[ob.x + (N - 1) / 2, ob.y + (N - 1) / 2] = 1;
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
