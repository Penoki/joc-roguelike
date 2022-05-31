using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detaliiIncapere : MonoBehaviour
{
    public List<Vector2Int> obstacoleCurente, puncteMaterializare;
    //obstacole = 1, teren liber = 0
    //initializare cu 0
    public int[,] grilajCamera = new int[,] { { 0, 0, 0, 0, 0, 0, 0},
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 },
                                              { 0, 0, 0, 0, 0, 0, 0 } };
    public const int N = 7;


    public void ActualizareGrilaCamera()
    {
        foreach (Vector2Int ob in obstacoleCurente)
        {
            //convertire spatiu 2D x y in matrice i j    si marcare obstacole
            grilajCamera[N - ob.y - (N - 1) / 2 - 1, ob.x + (N - 1) / 2] = 1;
        }
    }
}
