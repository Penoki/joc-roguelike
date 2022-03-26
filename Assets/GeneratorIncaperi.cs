using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorIncaperi : MonoBehaviour
{
    /* Legenda camere:
                        X = tezaur, camera cu comori
                        S = camera de pornire
                        N = camera de bataie
                        B = camera zmeului, sef
                        _ = vid
    */
    public int camereN = 7;
    public const int marimeTabla = 13;
    private bool zmeu = true, tezaur = true;                            //scopul de a avea doar 1 incapere de genul per etaj
    private char[,] grilaj= new char[marimeTabla, marimeTabla];         //spatiul maxim de plasare al camerelor
    public Text arataTablaJoc;
    public int samanta=8;
    public bool introduManualSamanta = false;

    void Start()
    {
        if (!introduManualSamanta)
        { samanta = System.DateTime.Now.Millisecond; } //fortuita generare de samanta
        Random.InitState(samanta);
        int camereN_initial, camereN_ramase, indexAdiacentaN;
        List<int> adiacentS = new List<int> { 56, 65, 67, 76 };
        List<int> adiacentS_noua = new List<int>();

        //initializare grilaj cu camere vid
        for (int i = 0; i < marimeTabla; i++)
        {
            for (int j = 0; j < marimeTabla; j++)
            {
                grilaj[i, j] = '_';
            }
        }

        #region Generare Tabla de Joc
        //
        //=>PASUL 1: plasare camera de start S
        grilaj[6,6] = 'S';

        //
        //=>PASUL 2: plasare camere N pe langa S
        camereN_initial = Random.Range(1, 5);                                   //cate camere N sa fie langa S
        camereN_ramase = camereN - camereN_initial;                             //cate camere sa mai adauge
        //Debug.Log(camereN_ramase);
        while (adiacentS_noua.Count < camereN_initial && adiacentS.Count > 0)   //extragere pozitii random instantiere camere N pe langa S
        {
            int index = Random.Range(0, adiacentS.Count);
            adiacentS_noua.Add(adiacentS[index]);
            adiacentS.RemoveAt(index);
        }

        for (int i = 0; i < adiacentS_noua.Count; i++)
        {
            grilaj[adiacentS_noua[i] / 10, adiacentS_noua[i] % 10] = 'N';       //plasare N-uri pe tabla de joc
        }

        //
        //=>PASUL 3: plasare restul camerelor N in functie de cele puse initial la pasul 2
        while(camereN_ramase>0)
        { 
            for (int i = 1; i < marimeTabla-1; i++)
            {
                for (int j = 1; j < marimeTabla-1; j++)
                {
                    if (camereN_ramase < 1) break;                                                                                  //cand s-au plasat toate camerele, stop
                    if (grilaj[i,j]=='_' && Random.Range(0,2)==1)
                    {
                        int Nord = grilaj[i, j - 1], Sud = grilaj[i, j + 1], Est = grilaj[i + 1, j], Vest = grilaj[i - 1, j];       //pozitiile adiacente celei curente din for
                        if (Nord != 'S' && Sud != 'S' && Est != 'S' && Vest != 'S')                                                 //nu plasam pe pozitiile de langa S, acestea fiind tratate la pasul 2
                        {
                            indexAdiacentaN = 0;
                            if (Nord == 'N') indexAdiacentaN++;
                            if (Sud == 'N') indexAdiacentaN++;
                            if (Est == 'N') indexAdiacentaN++;
                            if (Vest == 'N') indexAdiacentaN++;
                            if (indexAdiacentaN == 1)                                                                               //daca avem doar un N adiacent, plasam N
                            {
                                grilaj[i, j] = 'N';
                                camereN_ramase--;
                                //Debug.Log(camereN_ramase);
                            }
                        }
                    }
                }
            }
        }

        //
        //=>PASUL 4: plasare camera de comori, tezaur, X adiacent doar cu 1 N, restul vid
        while(tezaur)
        {
            for (int i = 1; i < marimeTabla - 1; i++)
            {
                for (int j = 1; j < marimeTabla - 1; j++)
                {
                    if (!tezaur) break;                                                                                             //daca a fost plasata camera, stop
                    if (grilaj[i, j] == '_' && Random.Range(0, 2) == 1)
                    {
                        int Nord = grilaj[i, j - 1], Sud = grilaj[i, j + 1], Est = grilaj[i + 1, j], Vest = grilaj[i - 1, j];       //pozitiile adiacente celei curente din for
                        if (Nord != 'S' && Sud != 'S' && Est != 'S' && Vest != 'S')                                                 //nu plasam pe pozitiile de langa S, acestea fiind tratate la pasul 2
                        {
                            indexAdiacentaN = 0;
                            if (Nord == 'N') indexAdiacentaN++;
                            if (Sud == 'N') indexAdiacentaN++;
                            if (Est == 'N') indexAdiacentaN++;
                            if (Vest == 'N') indexAdiacentaN++;
                            if (indexAdiacentaN == 1)                                                                               //daca avem doar un N adiacent, plasam N
                            {
                                grilaj[i, j] = 'X';
                                tezaur = false;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Afisare Grilaj Creat
        arataTablaJoc.text = $"Samanta: {samanta}\n";
        arataTablaJoc.text += "Tabla de joc:\n";

        //afisare grilaj creat
        for(int i=0;i<13;i++)
        {
            for(int j=0;j<13;j++)
            {
                arataTablaJoc.text += " ";
                arataTablaJoc.text += grilaj[i, j];
                arataTablaJoc.text += " ";
            }
            arataTablaJoc.text += "\n";
        }
        #endregion
    }

    private void Update()
    {
    }

}
