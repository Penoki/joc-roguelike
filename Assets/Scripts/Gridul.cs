using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridul : MonoBehaviour
{
    //public Transform jucator;
    public LayerMask blocadaMasca;
    public GameObject incapere;
    public int[,] grilajCamera;
    //definirea suprafetei care gridul o va acoperi
    public Vector2 marimeGridLume;
    //definirea spatiului ocupat de fiecare nod
    public float razaNod;
    //cameraParinte este folosit pentru pozitia incaperii
    private Vector3 decalajCameraParinte;
    Nod[,] grid;

    float diametruNod;
    public float decalaj = 0.5f;
    int marimeGridX, marimeGridY;

    public List<Nod> carare;

    private void Start()
    {
        incapere = this.transform.parent.gameObject;
        decalajCameraParinte = incapere.transform.position;

        diametruNod = razaNod * 2;
        //cate noduri vor intra pe grid in functie de diametrul unui nod
        marimeGridX = Mathf.RoundToInt(marimeGridLume.x / diametruNod);
        marimeGridY = Mathf.RoundToInt(marimeGridLume.y / diametruNod);

        grilajCamera = incapere.GetComponent<detaliiIncapere>().grilajCamera;

        CreazaGrid();
    }

    //metoda pentru convertirea pozitiei din lume in pozitie de pe grid
    public Nod coordLume_to_Nod(Vector3 pozLume)
    {
        //substragere decalaj camera
        pozLume = pozLume - decalajCameraParinte;
        //calcul procentaj unde s-ar afla dintre celule de pe grid, cazul nostru 34
        float procentX = (pozLume.x - decalaj + marimeGridLume.x / 2) / marimeGridLume.x;
        float procentY = (pozLume.y - decalaj + marimeGridLume.y / 2) / marimeGridLume.y;
        //daca jucatorul se afla in afara gridului, nu ne va da valori eronate
        procentX = Mathf.Clamp01(procentX);
        procentY = Mathf.Clamp01(procentY);

        int x = Mathf.RoundToInt((marimeGridX - 1) * procentX);
        int y = Mathf.RoundToInt((marimeGridY - 1) * procentY);

        return grid[x, y];
    }

    //metoda pentru convertirea pozitiei de pe grid in pozitie din lume
    public Vector3 Nod_to_coordLume(Nod pozNod)
    {
        Vector3 coordLume;
        float xprocent, yprocent;
        xprocent = Mathf.Abs(pozNod.poz.x / (marimeGridX - 1));
        yprocent = Mathf.Abs(pozNod.poz.y / (marimeGridY - 1));

        coordLume.z = 0;
        coordLume.x = xprocent * marimeGridLume.x + decalaj - marimeGridLume.x / 2;
        coordLume.y = yprocent * marimeGridLume.y + decalaj - marimeGridLume.y / 2;

        return coordLume;
    }

    void CreazaGrid()
    {
        //initializare grid
        grid = new Nod[marimeGridX, marimeGridY];
        //transform.position = centrul lumii, 
        Vector3 stangaJosLumii = transform.position - Vector3.right * marimeGridLume.x / 2 - Vector3.up * marimeGridLume.y / 2;

        for (int x = 0; x < marimeGridX; x++)
        {
            int x_obs = x / 5;
            for (int y = 0; y < marimeGridY; y++)
            {
                int y_obs = y / 5;
                //calcularea fiecare pozitie a fiecarui nod din grid
                Vector3 punctLume = stangaJosLumii + Vector3.right * (x * diametruNod + razaNod) + Vector3.up * (y * diametruNod + razaNod);

                //verificarea coliziunii cu obstacolele
                //bool traversibil = !(Physics2D.OverlapCircle(punctLume, razaNod, blocadaMasca));
                bool traversibil = false;
                if (grilajCamera[x_obs, y_obs] == 0)
                {
                    traversibil = true;
                }

                grid[x, y] = new Nod(traversibil, punctLume, x, y);
            }
        }
    }

    //vizualizare in joc
    private void OnDrawGizmos()
    {
        //desenare cu gizmos a gridului
        Gizmos.DrawWireCube(transform.position, new Vector3(marimeGridLume.x, marimeGridLume.y, 1));

        //verificarea gridului cu Gizmos
        if (grid != null)
        {
            //Nod jucatorNod = coordLume_to_Nod(jucator.position);
            foreach (Nod a in grid)
            {

                //pentru fiecare nod din grid, vom desena un cub
                // - 0.1f pentru un spatiu mic intre cuburi
                Gizmos.color = (a.traversabil) ? Color.white : Color.red;
                //if (jucatorNod == a) { Gizmos.color = Color.green; }
                if (carare != null && carare.Contains(a)) { Gizmos.color = Color.cyan; }
                Gizmos.DrawCube(a.poz, Vector3.one * (diametruNod - 0.1f));
            }
        }
    }

    //functie care returneaza vecinii unui NOD
    public List<Nod> iaVecinii(Nod nodul)
    {
        List<Nod> vecini = new List<Nod>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int verifI = nodul.gridX + i;
                int verifJ = nodul.gridY + j;

                //verificare ca vecinul sa nu fie afar din grid
                if (verifI >= 0 && verifI < marimeGridX && verifJ >= 0 && verifJ < marimeGridY)
                {
                    //fix pentru trecerea inamicului printre 2 obstacole pozitionate diagonal
                    // daca se afla pe o pozitie diagonala nodului
                    if (Mathf.Abs(i) + Mathf.Abs(j) == 2)
                    {
                        //verifica vecinii adiacenti directi acestui vecin de pe diagonala daca sunt traversabili
                        if (grid[nodul.gridX, verifJ].traversabil == true || grid[verifI, nodul.gridY].traversabil == true)
                        {
                            vecini.Add(grid[verifI, verifJ]);
                        }
                        continue;
                    }

                    vecini.Add(grid[verifI, verifJ]);
                }
            }
        }

        return vecini;
    }
}
