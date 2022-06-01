using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nod
{
    public Vector2Int poz;
    int Gcost, Hcost;

    public Nod(Vector2Int _poz)
    {
        poz = _poz;
    }

    public int Fcost
    {
        get
        {
            return Gcost + Hcost;
        }
    }
}
