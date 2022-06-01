using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nod
{
    public Vector2Int poz;
    public int Gcost, Hcost;
    public Nod parinte;

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
