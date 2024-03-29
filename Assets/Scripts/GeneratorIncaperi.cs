using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GeneratorIncaperi : MonoBehaviour
{
    /* Legenda camere:
                        X = tezaur, camera cu comori
                        S = camera de pornire
                        N = camera de bataie
                        B = camera zmeului, sef
                        0 = vid
                        1 = cost camera valida
                     2->6 = cost invalid
    */
    public int camereN = 7;
    //spatiul maxim de plasare al camerelor 13x13=169
    public const int marimeTabla = 13;

    public Text arataTablaJoc;
    public int sambure = 8;
    public bool introduManualSamanta = false;
    private float distantaMax = 0;
    private int nrIteratiiTotale = 0;
    private int pozitieIndepartata;

    public Tile[] podeaM, podeaU, podeaLU, coltLU, pereteU, obiecte;
    public GameObject cameraGrid, usaSus, usaJos, usaStanga, usaDreapta;
    public GameObject[] obstacol;
    //pozitiile obstacolelor
    public List<int[]> sabloaneSObs = new List<int[]>() { new int[] { -3, 2, -3, 1, -3, -1, -3, -2, -1, 3, -1, 2, -1, -2, -1, -3, 1, 3, 1, 2, 1, -2, 1, -3, 3, 2, 3, 1, 3, -1, 3, -2 },
                                                          new int[] { -3, 3, -3, -3, -2, 2, -2, -2, 2, 2, 2, -2, 3, 3, 3, -3} };
    public List<int[]> sabloaneNObs = new List<int[]>() { new int[] { -2, 2, -2, -2, -1, 1, -1, -1, 1, 1, 1, -1, 2, 2, 2, -2},
                                                          new int[] { -3, 3, -3, -3, 0, 0, 2, 3, 2, -3, 3, 3, 3, 2, 3, -2, 3, -3 } };
    public List<int[]> sabloaneXObs = new List<int[]>() { new int[] { -2, 2, -2, 1, -2, -1, -2, -2, -1, 2, -1, 1, -1, -1, -1, -2, 1, 2, 1, 1, 1, -1, 1, -2, 2, 2, 2, 1, 2, -1, 2, -2 } };
    public List<int[]> sabloaneBObs = new List<int[]>() { new int[] { -3, 3, -3, -3, 3, 3, 3, -3},
                                                          new int[] { -3, 3, -3, 2, -3, 1, -3, -1, -3, -2, -3, -3, 3, 3, 3, 2, 3, 1, 3, -1, 3, -2, 3, -3} };
    //pozitiile de nastere ale inamicilor
    public List<int[]> sabloaneNIna = new List<int[]>() { new int[] { -1, 0, 0, 1, 0, 0, 0, -1, 1, 0},
                                                          new int[] { -2, 3, -2, -3, 1, 3, 1, -3} };

    void Start()
    {
        if (!Samanta.introdusa)
        {
            //fortuita generare de samanta
            sambure = Random.Range(int.MinValue, int.MaxValue-2);
            Samanta.samanta = sambure; 
            Samanta.introdusa = true; 
        } 
        else
        { sambure = Samanta.samanta; }
        Random.InitState(sambure);

        char[,] grilaj = CreareTablaJoc(sambure);
        CreareCamerePrinAdiacenta('S', Random.Range(0, sabloaneSObs.Count), 0, 0, 0, 0, grilaj, null);

        #region Afisare Tabla Joc Creata
        arataTablaJoc.text = $"Samanta: {sambure}\n";
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
        //arataTablaJoc.text += $"\n Numar iteratii totale: {nrIteratiiTotale}";
        #endregion
    }

    private char[,] CreareTablaJoc(int samanta)
    {
        bool tezaur = true;                                          //scopul de a avea doar 1 incapere de genul per etaj  
        int index = -1;
        char[,] grilaj = new char[marimeTabla, marimeTabla];
        int camereN_initial, camereN_ramase;
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
            int k = Random.Range(0, adiacentS.Count);
            adiacentS_noua.Add(adiacentS[k]);
            adiacentS.RemoveAt(k);
            nrIteratiiTotale++;
        }

        for (int i = 0; i < adiacentS_noua.Count; i++)
        {
            int g = adiacentS_noua[i] / 100;
            int h = adiacentS_noua[i] % 100;
            grilaj[g, h] = 'N';                 //plasare 'N'-uri pe tabla de joc

            IncrementareCostAdiacent(ref grilaj, parceleValide, g, h, 1);
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

                IncrementareCostAdiacent(ref grilaj, parceleValide, g, h, 1);
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

                IncrementareCostAdiacent(ref grilaj, parceleValide, g, h, 2);
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
        IncrementareCostAdiacent(ref grilaj, parceleValide, pozitieIndepartata / 100, pozitieIndepartata % 100, 2);
        #endregion

        #endregion

        return grilaj;
    }

    private void IncrementareCostAdiacent(ref char[,] grilaj_primit, List<int> parceleValide, int g, int h, int increment)
    {
        //++ cost pozitii adiacente noii camere introduse
        if (g < 12)                                                 //interzis depasire dimensiuni tabla de joc
            if (grilaj_primit[g + 1, h] < '6')                              //'6' este valoarea maxima de cost posibila
            {
                grilaj_primit[g + 1, h] = (char)((int)grilaj_primit[g + 1, h] + increment);     //incrementare cost
                if (grilaj_primit[g + 1, h] == '1')
                {
                    parceleValide.Add((g + 1) * 100 + h);                    //salvare pozitie parcela valida
                }
                else
                {
                    parceleValide.Remove((g + 1) * 100 + h);                 //stergere pozitie daca e mai mare de 1 costul
                }
            }
        if (g > 0)
            if (grilaj_primit[g - 1, h] < '6')
            {
                grilaj_primit[g - 1, h] = (char)((int)grilaj_primit[g - 1, h] + increment);
                if (grilaj_primit[g - 1, h] == '1')
                {
                    parceleValide.Add((g - 1) * 100 + h);
                }
                else
                {
                    parceleValide.Remove((g - 1) * 100 + h);
                }
            }
        if (h < 12)
            if (grilaj_primit[g, h + 1] < '6')
            {
                grilaj_primit[g, h + 1] = (char)((int)grilaj_primit[g, h + 1] + increment);
                if (grilaj_primit[g, h + 1] == '1')
                {
                    parceleValide.Add(g * 100 + h + 1);
                }
                else
                {
                    parceleValide.Remove(g * 100 + h + 1);
                }
            }
        if (h > 0)
            if (grilaj_primit[g, h - 1] < '6')
            {
                grilaj_primit[g, h - 1] = (char)((int)grilaj_primit[g, h - 1] + increment);
                if (grilaj_primit[g, h - 1] == '1')
                {
                    parceleValide.Add(g * 100 + h - 1);
                }
                else
                {
                    parceleValide.Remove(g * 100 + h - 1);
                }
            }
    }

    private GameObject CreareCamera(char tipCamera, int tipSablon, int cordX, int cordY)
    {
        GameObject camera = Instantiate(cameraGrid, new Vector3(cordX, cordY, 0), Quaternion.identity);
        Tilemap tp1 = camera.transform.GetChild(0).gameObject.GetComponent<Tilemap>();              //template perete
        Tilemap tp2 = camera.transform.GetChild(1).gameObject.GetComponent<Tilemap>();              //template podea
        Tilemap to = camera.transform.GetChild(2).gameObject.GetComponent<Tilemap>();               //template obstacole

        //plasare pereti si podea randomizate
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                if ((Mathf.Abs(i) == 4 || Mathf.Abs(j) == 4) && (i != 0 && j != 0)) //pozitie de perete in dreptul ushii/lor
                {
                    if (Mathf.Abs(i) == Mathf.Abs(j))
                    {
                        //randomizare coltzuri peretzi
                        if (Random.Range(0, 3) == 1)
                            tp1.SetTile(new Vector3Int(i, j, 0), coltLU[Random.Range(1, coltLU.Length)]);
                    }
                    else
                    {
                        //randomizare o parte din pereti
                        if (Random.Range(0, 3) == 1)
                            tp1.SetTile(new Vector3Int(i, j, 0), pereteU[Random.Range(1, pereteU.Length)]);
                    }
                }
                else if ((Mathf.Abs(i) == 3 || Mathf.Abs(j) == 3) && (i != 0 && j != 0))
                {
                    if (Mathf.Abs(i) == Mathf.Abs(j))
                    {
                        //randomizare coltzuri podea
                    }
                    else
                    {
                        //randomizare o parte din podea
                        if (Random.Range(0, 3) == 1)
                            tp2.SetTile(new Vector3Int(i, j, 0), podeaU[Random.Range(1, podeaU.Length)]);
                    }
                }
                else if ((i != 0 && j != 0))
                {
                    //randomizare podea mijloc
                    if (Random.Range(0, 3) == 1)
                        tp2.SetTile(new Vector3Int(i, j, 0), podeaM[Random.Range(1, podeaM.Length)]);
                }
            }
        }

        //plasare obstacole
        int sansaObstacol = 8; //80% sa fie plasat
        switch (tipCamera)
        {
            case 'S':
                int[] a = sabloaneSObs[tipSablon];          //alegere template
                for (int i = 0; i < a.Length - 1; i += 2)
                {
                    if (Random.Range(0, 10) < sansaObstacol)
                    {
                        int ob = Random.Range(1, 4);
                        if (ob == 1)                //verificam daca pica groapa
                        {
                            if (to.GetTile(new Vector3Int(a[i], a[i + 1] + 1, 0)) == obiecte[0] || to.GetTile(new Vector3Int(a[i], a[i + 1] + 1, 0)) == obiecte[1])       //verificam daca exista deja o groapa deasupra
                            {
                                to.SetTile(new Vector3Int(a[i], a[i + 1], 0), obiecte[0]);
                            }
                            else
                            {
                                to.SetTile(new Vector3Int(a[i], a[i + 1], 0), obiecte[1]);
                            }
                        }
                        if (ob == 2)
                        {
                            //plasam cutie
                            GameObject cutie = Instantiate(obstacol[0], camera.transform);
                            cutie.transform.position = new Vector3(cutie.transform.position.x + a[i], cutie.transform.position.y + a[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la cutie
                                cutie.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }

                        if (ob == 3)
                        {
                            //plasam vaza
                            GameObject vaza = Instantiate(obstacol[1], camera.transform);
                            vaza.transform.position = new Vector3(vaza.transform.position.x + a[i], vaza.transform.position.y + a[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la vaza
                                vaza.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }
                        //trimitere detalii despre unde se afla obstacole in camera aceasta
                        camera.GetComponent<detaliiIncapere>().obstacoleCurente.Add(new Vector2Int(a[i], a[i + 1]));
                    }
                }

                //adaugare tag camera curenta
                camera.GetComponent<detaliiIncapere>().tag = "cameraCurenta";
                break;

            case 'N':
                int[] b = sabloaneNObs[tipSablon];          //alegere template
                for (int i = 0; i < b.Length - 1; i += 2)
                {
                    if (Random.Range(0, 10) < sansaObstacol)
                    {
                        int ob = Random.Range(1, 4);
                        if (ob == 1)                //verificam daca pica groapa
                        {
                            if (to.GetTile(new Vector3Int(b[i], b[i + 1] + 1, 0)) == obiecte[0] || to.GetTile(new Vector3Int(b[i], b[i + 1] + 1, 0)) == obiecte[1])       //verificam daca exista deja o groapa deasupra
                            {
                                to.SetTile(new Vector3Int(b[i], b[i + 1], 0), obiecte[0]);
                            }
                            else
                            {
                                to.SetTile(new Vector3Int(b[i], b[i + 1], 0), obiecte[1]);
                            }
                        }
                        if (ob == 2)
                        {
                            //plasam cutie
                            GameObject cutie = Instantiate(obstacol[0], camera.transform);
                            cutie.transform.position = new Vector3(cutie.transform.position.x + b[i], cutie.transform.position.y + b[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la cutie
                                cutie.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }

                        if (ob == 3)
                        {
                            //plasam vaza
                            GameObject vaza = Instantiate(obstacol[1], camera.transform);
                            vaza.transform.position = new Vector3(vaza.transform.position.x + b[i], vaza.transform.position.y + b[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la vaza
                                vaza.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }
                        //trimitere detalii despre unde se afla obstacole in camera aceasta
                        camera.GetComponent<detaliiIncapere>().obstacoleCurente.Add(new Vector2Int(b[i], b[i + 1]));
                    }
                }
                //cod pentru plasarea punctelor de materializare ale inamicilor
                int[] b1 = sabloaneNIna[tipSablon];
                for (int i = 0; i < b1.Length - 1; i += 2)
                {
                    camera.GetComponent<detaliiIncapere>().puncteMaterializare.Add(new Vector2Int(b1[i], b1[i + 1]));
                }

                break;

            case 'X':
                int[] c = sabloaneXObs[tipSablon];          //alegere template
                for (int i = 0; i < c.Length - 1; i += 2)
                {
                    if (Random.Range(0, 10) < sansaObstacol)
                    {
                        int ob = Random.Range(1, 4);
                        if (ob == 1)                //verificam daca pica groapa
                        {
                            if (to.GetTile(new Vector3Int(c[i], c[i + 1] + 1, 0)) == obiecte[0] || to.GetTile(new Vector3Int(c[i], c[i + 1] + 1, 0)) == obiecte[1])       //verificam daca exista deja o groapa deasupra
                            {
                                to.SetTile(new Vector3Int(c[i], c[i + 1], 0), obiecte[0]);
                            }
                            else
                            {
                                to.SetTile(new Vector3Int(c[i], c[i + 1], 0), obiecte[1]);
                            }
                        }
                        if (ob == 2)
                        {
                            //plasam cutie
                            GameObject cutie = Instantiate(obstacol[0], camera.transform);
                            cutie.transform.position = new Vector3(cutie.transform.position.x + c[i], cutie.transform.position.y + c[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la cutie
                                cutie.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }

                        if (ob == 3)
                        {
                            //plasam vaza
                            GameObject vaza = Instantiate(obstacol[1], camera.transform);
                            vaza.transform.position = new Vector3(vaza.transform.position.x + c[i], vaza.transform.position.y + c[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la vaza
                                vaza.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }
                        //trimitere detalii despre unde se afla obstacole in camera aceasta
                        camera.GetComponent<detaliiIncapere>().obstacoleCurente.Add(new Vector2Int(c[i], c[i + 1]));
                    }
                }

                //adaugare script pentru camera finala
                camera.AddComponent<CameraComori>();

                break;
            case 'B':
                int[] d = sabloaneBObs[tipSablon];          //alegere template
                for (int i = 0; i < d.Length - 1; i += 2)
                {
                    if (Random.Range(0, 10) < sansaObstacol)
                    {
                        int ob = Random.Range(1, 4);
                        if (ob == 1)                //verificam daca pica groapa
                        {
                            if (to.GetTile(new Vector3Int(d[i], d[i + 1] + 1, 0)) == obiecte[0] || to.GetTile(new Vector3Int(d[i], d[i + 1] + 1, 0)) == obiecte[1])       //verificam daca exista deja o groapa deasupra
                            {
                                to.SetTile(new Vector3Int(d[i], d[i + 1], 0), obiecte[0]);
                            }
                            else
                            {
                                to.SetTile(new Vector3Int(d[i], d[i + 1], 0), obiecte[1]);
                            }
                        }
                        if (ob == 2)
                        {
                            //plasam cutie
                            GameObject cutie = Instantiate(obstacol[0], camera.transform);
                            cutie.transform.position = new Vector3(cutie.transform.position.x + d[i], cutie.transform.position.y + d[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la cutie
                                cutie.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }

                        if (ob == 3)
                        {
                            //plasam vaza
                            GameObject vaza = Instantiate(obstacol[1], camera.transform);
                            vaza.transform.position = new Vector3(vaza.transform.position.x + d[i], vaza.transform.position.y + d[i + 1], 0);
                            if (Random.Range(0, 2) == 1)
                            {
                                //flip orizontal la vaza
                                vaza.transform.localScale = new Vector3(-1, 1, 1);
                            }
                        }
                        //trimitere detalii despre unde se afla obstacole in camera aceasta
                        camera.GetComponent<detaliiIncapere>().obstacoleCurente.Add(new Vector2Int(d[i], d[i + 1]));
                    }
                }

                //adaugare script pentru camera finala
                camera.AddComponent<CameraFinala>();

                break;
        }

        camera.GetComponent<detaliiIncapere>().ActualizareGrilaCamera();
        camera.GetComponent<detaliiIncapere>().tipIncapere = tipCamera;

        return camera;
    }

    //functie recursiva
    private void CreareCamerePrinAdiacenta(char tipCamera, int tipSablon, int preCordX, int preCordY, int cordX, int cordY, char[,] grilaj, GameObject usa)
    {
        int preK = -preCordY / 9 + 6;
        int preL = preCordX / 9 + 6;
        int k = -cordY / 9 + 6;
        int l = cordX / 9 + 6;

        //plasare camera curenta
        GameObject cameraCurenta = CreareCamera(tipCamera, tipSablon, cordX, cordY);

        //plasare usa complementara primei
        if (usa != null)
        {
            if (usa.name.Contains(usaSus.name))
            {
                //plasarea unei usi in camera curenta(parinte) catre camera de jos
                GameObject usa2 = Instantiate(usaJos, cameraCurenta.transform);

                //legarea usilor
                usa.GetComponent<Teleportinator>().destinatie = usa2.transform.GetChild(0).transform;
                usa2.GetComponent<Teleportinator>().destinatie = usa.transform.GetChild(0).transform;
            }
            if (usa.name.Contains(usaJos.name))
            {
                //plasarea unei usi in camera curenta(parinte) catre camera de sus
                GameObject usa2 = Instantiate(usaSus, cameraCurenta.transform);

                //legarea usilor
                usa.GetComponent<Teleportinator>().destinatie = usa2.transform.GetChild(0).transform;
                usa2.GetComponent<Teleportinator>().destinatie = usa.transform.GetChild(0).transform;
            }
            if (usa.name.Contains(usaStanga.name))
            {

                //plasarea unei usi in camera curenta(parinte) catre camera din dreapta
                GameObject usa2 = Instantiate(usaDreapta, cameraCurenta.transform);

                //legarea usilor
                usa.GetComponent<Teleportinator>().destinatie = usa2.transform.GetChild(0).transform;
                usa2.GetComponent<Teleportinator>().destinatie = usa.transform.GetChild(0).transform;

            }
            if (usa.name.Contains(usaDreapta.name))
            {
                //plasarea unei usi in camera curenta(parinte) catre camera din stanga
                GameObject usa2 = Instantiate(usaStanga, cameraCurenta.transform);

                //legarea usilor
                usa.GetComponent<Teleportinator>().destinatie = usa2.transform.GetChild(0).transform;
                usa2.GetComponent<Teleportinator>().destinatie = usa.transform.GetChild(0).transform;
            }
        }

        if (k != 0)
        {
            if ((int)grilaj[k - 1, l] > 64 && (k - 1) != preK)
            {
                //plasarea primei usi in camera curenta(parinte) catre camera de sus
                GameObject usa1a = Instantiate(usaSus, cameraCurenta.transform);

                //mers la camera de sus
                CreareCamerePrinAdiacenta(grilaj[k - 1, l], CareSablon(grilaj[k - 1, l]), cordX, cordY, cordX, (6 - (k - 1)) * 9, grilaj, usa1a);
            }
        }
        if (k != 12)
        {
            if ((int)grilaj[k + 1, l] > 64 && (k + 1) != preK)
            {
                //plasarea primei usi in camera curenta(parinte) catre camera de jos
                GameObject usa1b = Instantiate(usaJos, cameraCurenta.transform);

                //mers la camera de jos
                CreareCamerePrinAdiacenta(grilaj[k + 1, l], CareSablon(grilaj[k + 1, l]), cordX, cordY, cordX, (6 - (k + 1)) * 9, grilaj, usa1b);
            }
        }
        if (l != 0)
        {
            if ((int)grilaj[k, l - 1] > 64 && (l - 1) != preL)
            {
                //plasarea primei usi in camera curenta(parinte) catre camera din stanga
                GameObject usa1c = Instantiate(usaStanga, cameraCurenta.transform);

                //mers la camera din stanga
                CreareCamerePrinAdiacenta(grilaj[k, l - 1], CareSablon(grilaj[k, l - 1]), cordX, cordY, ((l - 1) - 6) * 9, cordY, grilaj, usa1c);
            }
        }
        if (l != 12)
        {
            if ((int)grilaj[k, l + 1] > 64 && (l + 1) != preL)
            {
                //plasarea primei usi in camera curenta(parinte) catre camera din dreapta
                GameObject usa1d = Instantiate(usaDreapta, cameraCurenta.transform);

                //mers la camera din dreapta
                CreareCamerePrinAdiacenta(grilaj[k, l + 1], CareSablon(grilaj[k, l + 1]), cordX, cordY, ((l + 1) - 6) * 9, cordY, grilaj, usa1d);
            }
        }
    }

    private int CareSablon(char tipCamera)
    {
        int a = 0;
        switch (tipCamera)
        {
            case 'S':
                a = Random.Range(0, sabloaneSObs.Count);
                break;
            case 'N':
                a = Random.Range(0, sabloaneNObs.Count);
                break;
            case 'X':
                a = Random.Range(0, sabloaneXObs.Count);
                break;
            case 'B':
                a = Random.Range(0, sabloaneBObs.Count);
                break;
        }
        return a;
    }
}
