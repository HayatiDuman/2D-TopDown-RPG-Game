using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamdanKapi : MonoBehaviour
{
    public GameObject kapaliKapi; // Kapal� GameObject'i
    public GameObject acikKapi; // A��k kap� GameObject'i

    public bool isOpen = false; // �amdan�n yan�p yanmad���n� belirten boolean

    public static SamdanKapi Ornek { get; private set; }

    void Start()
    {
        KapiDurumu();
    }

    public void KapiAc()
    {
        if (!isOpen)
        {
            isOpen = true; // Kap�n�n a��k oldu�unu belirt
            KapiDurumu();
            Debug.Log("Kap� a��k--");
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
