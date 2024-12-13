using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEidham : MonoBehaviour
{
    public GameObject bloodEnemies; 

    void Start()
    {
        // Destroy the bullet after 2 seconds to prevent it from sticking around
        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Instantiate the blood effect at the point of collision
            Instantiate(bloodEnemies, collision.contacts[0].point, Quaternion.identity);
        }

        // Destroy the bullet on collision with any object
        Destroy(gameObject);
    }
}
