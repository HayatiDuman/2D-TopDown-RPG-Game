using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, Interactable
{
    [SerializeField] public GameObject key;
    public void InterfaceEtkilesim()
    {
        key.SetActive(true);
    }
}
