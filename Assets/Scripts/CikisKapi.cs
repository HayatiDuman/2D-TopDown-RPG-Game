using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CikisKapi : MonoBehaviour, Interactable
{
    public GameObject kapaliKapi; // Kapalý GameObject'i
    public GameObject acikKapi; // Açýk kapý GameObject'i
    [SerializeField] public GameObject key;
    private bool anahtar = false;
    [SerializeField] Dialog diyalog;
    public static CikisKapi Ornek { get; private set; }

    public void InterfaceEtkilesim()
    {
        KapiAc();
        if (anahtar)
        {
            StopCoroutine(DialogManager.Ornek.DiyalogBaslat(diyalog));
        }else if (anahtar && kapaliKapi.activeSelf)
        {
            StartCoroutine(DialogManager.Ornek.DiyalogBaslat(diyalog));
        }

    }

    private void KapiAc()
    {
        var varmý = key.activeSelf;
        if (varmý)
            anahtar = true;
        if (anahtar)
        {
            Debug.Log("çýkýþ");
            KapiDurumu();
        }
        else
            Debug.Log("çýkýþ yapýlamýyor");
    }

    private void KapiDurumu()
    {
        if (anahtar)
        {
            kapaliKapi.SetActive(false);
            acikKapi.SetActive(true);
            key.SetActive(false);
            key.transform.SetParent(GameObject.Find("Chest1").transform);
        }
        else
        {
            kapaliKapi.SetActive(true);
            acikKapi.SetActive(false);
        }
    }
}
