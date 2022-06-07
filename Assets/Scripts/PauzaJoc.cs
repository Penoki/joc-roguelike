using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzaJoc : MonoBehaviour
{
    bool apasat = false;
    public GameObject fundal, butoane, cameraVideo;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!apasat)
            {
                Pauza();
                apasat = true;
            }
            else
            {
                Continua();
                apasat = false;
            }
        }
    }

    void Pauza()
    {
        //oprire timp
        Time.timeScale = 0;
        fundal.SetActive(true);
        butoane.SetActive(true);

        cameraVideo.GetComponent<UrmarireaCameraJucator>().enabled = false;
    }

    void Continua()
    {
        //repornire timp
        Time.timeScale = 1;
        fundal.SetActive(false);
        butoane.SetActive(false);

        cameraVideo.GetComponent<UrmarireaCameraJucator>().enabled = true;
    }
}
