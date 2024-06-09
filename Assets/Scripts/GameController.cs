using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OyunDurumu { Serbest, Dovus, Diyalog}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    OyunDurumu durum;

    private void Update()
    {
        if(durum == OyunDurumu.Serbest)
        {
            playerController.Update();
        }else if(durum == OyunDurumu.Diyalog)
        {


        }else if(durum == OyunDurumu.Dovus)
        {

        }
    }

}
