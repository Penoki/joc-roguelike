using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nod
{
    public bool traversabil;
    public Vector3 poz;
    public int Gcost, Hcost;
    public Nod parinte;

    public Nod(Vector3 _poz, bool _traversibil)
    {
        poz = _poz;
        traversabil = _traversibil;
    }

    public int Fcost
    {
        get
        {
            return Gcost + Hcost;
        }
    }
}
