using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public GameObject Medkit;
    public GameObject particleHealth;
    private GameObject player;
    PlayerControllerEidham playerController;

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerControllerEidham>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");

        if (other.gameObject == player)
        {
            Debug.Log("Player collided with medkit");

            // Increase player's health
            playerController.IncreaseHealth(1); // Adjust the amount as needed

            // Play the particle effect
            if (particleHealth != null)
            {
                Instantiate(particleHealth, transform.position, transform.rotation);
            }

            // Destroy the medkit
            Destroy(gameObject);
        }
    }
}
