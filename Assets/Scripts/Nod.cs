using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nod
{
    public bool traversabil;
    public Vector3 poz;
    public int Gcost, Hcost;
    public Nod parinte;
    public int gridX, gridY;

    public Nod(bool _traversibil, Vector3 _poz, int _gridX, int _gridY)
    {
        poz = _poz;
        traversabil = _traversibil;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int Fcost
    {
        get
        {
            return Gcost + Hcost;
        }
    }
}
