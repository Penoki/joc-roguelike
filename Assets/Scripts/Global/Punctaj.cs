using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punctaj : MonoBehaviour
{
    private static int punctaj = 0;

    public static int Punctare { get => punctaj; set => punctaj = value; }
}
