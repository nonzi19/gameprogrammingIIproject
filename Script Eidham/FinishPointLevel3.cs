using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPointLevel3 : MonoBehaviour
{
    public GameObject FinishLevel3Menu;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // Stop time in the game
            Time.timeScale = 0f;

            FinishLevel3Menu.SetActive(true);
        }
    }
}
