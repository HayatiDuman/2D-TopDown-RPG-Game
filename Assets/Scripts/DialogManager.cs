using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class Dialog
{
    [SerializeField] string name = "NPC";
    [SerializeField] string[] satirlar;

    public string[] Satirlar
    {
        get { return satirlar; }
    }
    public string Name
    { get { return name; } }
}

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] Text dialogName;
    [SerializeField] int harfBasiGecikme = 40;

    private Dialog diyalog;
    private int uzunluk;
    public int mevcutSatir = 0;
    Coroutine yazCoroutine = null;

    public static DialogManager Ornek { get; private set; }

    private void Awake()
    {
        Ornek = this;
    }

    public IEnumerator DiyalogBaslat(Dialog yeniDiyalog)
    {
        diyalog = yeniDiyalog;
        uzunluk = diyalog.Satirlar.Length;
        dialogBox.SetActive(true);
        MevcutCoroutineDurdur();
        yazCoroutine = StartCoroutine(DiyalogYaz(diyalog.Satirlar[0]));
        yield return null;
    }

    private void Update()
    {
        if (diyalog != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                KarakterTanimlama();
                mevcutSatir++;
                if (mevcutSatir <= uzunluk)
                {
                    MevcutCoroutineDurdur();
                    yazCoroutine = StartCoroutine(DiyalogYaz(diyalog.Satirlar[mevcutSatir - 1]));
                }
                else
                {
                    MevcutCoroutineDurdur();
                    dialogBox.SetActive(false);
                    mevcutSatir = 0;  // Her þey bittiðinde baþa dönmek için sýfýrlýyoruz.
                }
            }
        }
        
    }

    private void MevcutCoroutineDurdur()
    {
        if (yazCoroutine != null)
        {
            StopCoroutine(yazCoroutine);
            yazCoroutine = null;
        }
    }

    public bool DiyalogKontrol()
    {
        if (mevcutSatir == 0)
            return false;
        else
            return true;
    }

    private void KarakterTanimlama()
    {
        dialogName.text = "NPC";
        dialogName.text = diyalog.Name;
    }

    public IEnumerator DiyalogYaz(string satir)
    {
        dialogText.text = "";
        foreach (var harf in satir)
        {
            dialogText.text += harf;
            yield return new WaitForSeconds(1f / harfBasiGecikme);
        }
    }
}