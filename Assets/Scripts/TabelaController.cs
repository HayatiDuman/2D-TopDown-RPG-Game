using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabelaController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog diyalog;
    public void InterfaceEtkilesim()
    {
        StartCoroutine(DialogManager.Ornek.DiyalogBaslat(diyalog));
    }
}
