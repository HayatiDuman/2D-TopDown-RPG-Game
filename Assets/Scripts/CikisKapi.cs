using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CikisKapi : MonoBehaviour, Interactable
{
    public GameObject kapaliKapi; // Kapal� GameObject'i
    public GameObject acikKapi; // A��k kap� GameObject'i
    [SerializeField] public GameObject key;
    private bool anahtar = false;
    [SerializeField] Dialog diyalog;
    public static CikisKapi Ornek { get; private set; }

    public GameObject endingScreen;

    private const string parent= "Character";

    public void InterfaceEtkilesim()
    {
        KapiAc();
        if (!anahtar)
            StartCoroutine(DialogManager.Ornek.DiyalogBaslat(diyalog));

    }

    private void KapiAc()
    {
        var varmı = key.activeSelf;
        PlayerController playerController = new PlayerController();
        if (varmı && key.transform.parent == GameObject.Find("Character").transform)
            anahtar = true;
        if (anahtar)
        {
            Debug.Log("��k��");
            KapiDurumu();
        }
        else
            Debug.Log("��k�� yap�lam�yor");
    }

    private void KapiDurumu()
    {
        if (anahtar)
        {
            kapaliKapi.SetActive(false);
            acikKapi.SetActive(true);
            key.SetActive(false);
            key.transform.SetParent(GameObject.Find("Chest1").transform);
            ShowEndingScreen();
        }
        else
        {
            kapaliKapi.SetActive(true);
            acikKapi.SetActive(false);
        }
    }

    void ShowEndingScreen()
    {
        Debug.Log("Showing victory screen");
        endingScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
