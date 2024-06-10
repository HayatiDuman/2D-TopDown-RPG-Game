using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject endingScreen;

    void OnTriggerEnter2D(Collider2D other)
    {
         Debug.Log("OnTriggerEnter2D called");
        if(other.CompareTag("Player"))
        {
            ShowEndingScreen();
        }
    }

    void ShowEndingScreen()
    {
        Debug.Log("Showing victory screen");
        endingScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
