using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contor : MonoBehaviour
{
    private static int contor = 0;

    public static int Cont { get => contor; set => contor = value; }
}
