using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamdanKapi : MonoBehaviour
{
    public GameObject kapaliKapi; // Kapalý GameObject'i
    public GameObject acikKapi; // Açýk kapý GameObject'i

    public bool isOpen = false; // Þamdanýn yanýp yanmadýðýný belirten boolean

    public static SamdanKapi Ornek { get; private set; }

    void Start()
    {
        KapiDurumu();
    }

    public void KapiAc()
    {
        if (!isOpen)
        {
            isOpen = true; // Kapýnýn açýk olduðunu belirt
            KapiDurumu();
            Debug.Log("Kapý açýk--");
        }
    }

    private void KapiDurumu()
    {
        if (isOpen)
        {
            kapaliKapi.SetActive(false);
            acikKapi.SetActive(true);
        }
        else
        {
            kapaliKapi.SetActive(true);
            acikKapi.SetActive(false);
        }
    }
}
