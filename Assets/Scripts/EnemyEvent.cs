using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    [SerializeField] public int can = 3;

    public void CanEksilt()
    {
        if (can > 0)
            --can;
       // else if (can = 0)

    }
    
}
