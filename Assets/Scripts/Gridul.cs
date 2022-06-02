using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridul : MonoBehaviour
{
    public Transform jucator;
    public LayerMask blocadaMasca;
    //definirea suprafetei care gridul o va acoperi
    public Vector2 marimeGridLume;
    //definirea spatiului ocupat de fiecare nod
    public float razaNod;
    Nod[,] grid;

    float diametruNod;
    int gridX, gridY;

    private void Start()
    {
        diametruNod = razaNod * 2;
        //cate noduri vor intra pe grid in functie de diametrul unui nod
        gridX = Mathf.RoundToInt(marimeGridLume.x / diametruNod);
        gridY = Mathf.RoundToInt(marimeGridLume.y / diametruNod);

        CreazaGrid();

    }

    //metoda pentru convertirea pozitiei din lume in pozitie de pe grid
    public Nod coordLume_to_Nod(Vector3 pozLume)
    {
        //calcul procentaj unde s-ar afla dintre celule de pe grid, cazul nostru 34
        float procentX = (pozLume.x + marimeGridLume.x / 2) /  marimeGridLume.x;
        float procentY = (pozLume.y + marimeGridLume.y/2) /  marimeGridLume.y;
        //daca jucatorul se afla in afara gridului, nu ne va da valori eronate
        procentX = Mathf.Clamp01(procentX);
        procentY = Mathf.Clamp01(procentY);

        int x = Mathf.RoundToInt((gridX - 1) *  procentX);
        int y = Mathf.RoundToInt((gridY - 1) *  procentY);

        return grid[x-2,y-2];
    }

    void CreazaGrid()
    {
        //initializare grid
        grid = new Nod[gridX, gridY];
        //transform.position = centrul lumii, 
        Vector3 stangaJosLumii = transform.position - Vector3.right * marimeGridLume.x / 2 - Vector3.up * marimeGridLume.y / 2;

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //calcularea fiecare pozitie a fiecarui nod din grid
                Vector3 punctLume = stangaJosLumii + Vector3.right * (x * diametruNod + razaNod) + Vector3.up * (y * diametruNod + razaNod);

                //verificarea coliziunii cu obstacolele
                bool traversibil = !(Physics2D.OverlapCircle(punctLume, razaNod, blocadaMasca));
                grid[x, y] = new Nod(punctLume, traversibil);
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
            Nod jucatorNod = coordLume_to_Nod(jucator.position);
            foreach (Nod a in grid)
            {

                //pentru fiecare nod din grid, vom desena un cub
                // - 0.1f pentru un spatiu mic intre cuburi
                Gizmos.color = (a.traversabil) ? Color.white : Color.red;
                if (jucatorNod == a) { Gizmos.color = Color.green; }
                Gizmos.DrawCube(a.poz, Vector3.one * (diametruNod - 0.1f));
            }
        }
    }
}
