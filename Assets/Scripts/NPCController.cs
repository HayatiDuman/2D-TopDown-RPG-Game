using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog diyalog;
    public void InterfaceEtkilesim()
    {
        StartCoroutine(DialogManager.Ornek.DiyalogBaslat(diyalog));
    }
}
