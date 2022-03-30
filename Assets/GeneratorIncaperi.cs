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
                        0 = vid
                        1 = cost camera valida
                     2->6 = cost 
    */
    public int camereN = 7;
    public const int marimeTabla = 13;
    private bool tezaur = true;                                          //scopul de a avea doar 1 incapere de genul per etaj
    private char[,] grilaj = new char[marimeTabla, marimeTabla];         //spatiul maxim de plasare al camerelor
    public Text arataTablaJoc;
    public int samanta = 8;
    public bool introduManualSamanta = false;
    private int index = -1;
    private float distantaMax = 0;
    private int pozitieIndepartata;
    private int nrIteratiiTotale = 0;

    void Start()
    {
        if (!introduManualSamanta)
        { samanta = System.DateTime.Now.Millisecond; } //fortuita generare de samanta
        Random.InitState(samanta);
        int camereN_initial, camereN_ramase, indexAdiacentaN;
        List<int> adiacentS = new List<int> { 506, 605, 607, 706 };
        List<int> adiacentS_noua = new List<int>();
        List<int> parceleValide = new List<int>();     //lista in care se vor salva locurile cu cost '1' unde se pot plasa camere

        //initializare grilaj cu camere vid
        for (int i = 0; i < marimeTabla; i++)
        {
            for (int j = 0; j < marimeTabla; j++)
            {
                grilaj[i, j] = '0';
                nrIteratiiTotale++;
            }
        }

        #region Generare Tabla de Joc
        //
        #region=>PASUL 1: plasare camera de start 'S'
        grilaj[6, 6] = 'S';
        grilaj[5, 6] = grilaj[6, 5] = grilaj[6, 7] = grilaj[7, 6] = '2';        //camere adiacente 'S' atribuite cost 2
        #endregion

        //
        #region=>PASUL 2: plasare camere 'N' pe langa 'S'
        camereN_initial = Random.Range(1, 5);                                   //cate camere 'N' sa fie langa 'S'
        camereN_ramase = camereN - camereN_initial;                             //cate camere sa mai adauge la pas 3
        //Debug.Log(camereN_ramase);
        while (adiacentS_noua.Count < camereN_initial && adiacentS.Count > 0)   //extragere pozitii random instantiere camere 'N' pe langa 'S'
        {
            int index = Random.Range(0, adiacentS.Count);
            adiacentS_noua.Add(adiacentS[index]);
            adiacentS.RemoveAt(index);
            nrIteratiiTotale++;
        }

        for (int i = 0; i < adiacentS_noua.Count; i++)
        {
            int g = adiacentS_noua[i] / 100;
            int h = adiacentS_noua[i] % 100;
            grilaj[g, h] = 'N';                 //plasare 'N'-uri pe tabla de joc

            IncrementareCostAdiacent(parceleValide, g, h, 1);
            nrIteratiiTotale++;
        }
        #endregion

        //
        #region =>PASUL 3: plasare restul de camere 'N'  

        while (camereN_ramase > 0)
        {
            index++;
            if (Random.Range(0, 2) == 1)
            {
                camereN_ramase--;
                int g = parceleValide[index] / 100;
                int h = parceleValide[index] % 100;
                grilaj[g, h] = 'N';
                parceleValide.RemoveAt(index);      //stergere pozitie in care s-a plasat 'N'

                IncrementareCostAdiacent(parceleValide, g, h, 1);
                index = -1;
            }
            if (index == (parceleValide.Count - 1)) index = -1;
            nrIteratiiTotale++;
        }
        #endregion

        //
        #region=>PASUL 4: plasare camera tezaur 'X'
        while (tezaur)
        {
            index++;
            if (Random.Range(0, 2) == 1)
            {
                tezaur = false;
                int g = parceleValide[index] / 100;
                int h = parceleValide[index] % 100;
                grilaj[g, h] = 'X';
                parceleValide.RemoveAt(index);      //stergere pozitia in care s-a plasat 'X'

                IncrementareCostAdiacent(parceleValide, g, h, 2);
                index = -1;
            }
            if (index == (parceleValide.Count - 1)) index = -1;
            nrIteratiiTotale++;
        }
        #endregion

        //
        #region=>PASUL 5: plasare camera zmeu 'B'
        //Cautarea celei mai indepartate parcele valide
        for (int i = 0; i < parceleValide.Count; i++)
        {
            int g = parceleValide[i] / 100;
            int h = parceleValide[i] % 100;
            float distanta = Mathf.Sqrt((g - 6) * (g - 6) + (h - 6) * (h - 6));        //formula distantei Euclidiene dintre 2 puncte 
            if (distantaMax < distanta)
            {
                distantaMax = distanta;
                pozitieIndepartata = g * 100 + h;
            }
            nrIteratiiTotale++;
        }
        //Plasare camera zmeu 'B'
        grilaj[pozitieIndepartata / 100, pozitieIndepartata % 100] = 'B';
        IncrementareCostAdiacent(parceleValide, pozitieIndepartata / 100, pozitieIndepartata % 100, 2);
        #endregion

        #endregion

        #region Afisare Grilaj Creat
        arataTablaJoc.text = $"Samanta: {samanta}\n";
        arataTablaJoc.text += "Tabla de joc:\n";

        //afisare grilaj creat
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                arataTablaJoc.text += " ";
                arataTablaJoc.text += grilaj[i, j];
                arataTablaJoc.text += " ";
            }
            arataTablaJoc.text += "\n";
            nrIteratiiTotale++;
        }
        arataTablaJoc.text += $"\n Numar iteratii totale: {nrIteratiiTotale}";
        #endregion
    }

    private void IncrementareCostAdiacent(List<int> parceleValide, int g, int h, int increment)
    {
        //++ cost pozitii adiacente noii camere introduse
        if (g < 13)                                                               //nu depasim dimensiunile tablei de joc
            if (grilaj[g + 1, h] < '6')                                           //'6' este valoarea maxima de cost posibila
            {
                grilaj[g + 1, h] = (char)((int)grilaj[g + 1, h] + increment);     //incrementare cost
                if (grilaj[g + 1, h] == '1')
                {
                    parceleValide.Add((g + 1) * 100 + h);                          //salvare pozitie parcela valida
                }
                else
                {
                    parceleValide.Remove((g + 1) * 100 + h);                       //stergere pozitie daca e mai mare de 1 costul
                }
            }

        if (g > 0)
            if (grilaj[g - 1, h] < '6')
            {
                grilaj[g - 1, h] = (char)((int)grilaj[g - 1, h] + increment);
                if (grilaj[g - 1, h] == '1')
                {
                    parceleValide.Add((g - 1) * 100 + h);
                }
                else
                {
                    parceleValide.Remove((g - 1) * 100 + h);
                }
            }

        if (h < 13)
            if (grilaj[g, h + 1] < '6')
            {
                grilaj[g, h + 1] = (char)((int)grilaj[g, h + 1] + increment);
                if (grilaj[g, h + 1] == '1')
                {
                    parceleValide.Add(g * 100 + h + 1);
                }
                else
                {
                    parceleValide.Remove(g * 100 + h + 1);
                }
            }

        if (h > 0)
            if (grilaj[g, h - 1] < '6')
            {
                grilaj[g, h - 1] = (char)((int)grilaj[g, h - 1] + increment);
                if (grilaj[g, h - 1] == '1')
                {
                    parceleValide.Add(g * 100 + h - 1);
                }
                else
                {
                    parceleValide.Remove(g * 100 + h - 1);
                }
            }
    }
}
