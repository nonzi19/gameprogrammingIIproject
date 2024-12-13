using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBombLv3 : MonoBehaviour
{
    private GameObject player;
    public GameObject particleStunBomb;
    PlayerControllerEidham playerController;
    public float stunDuration = 3.0f; // Duration of the stun effect

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerControllerEidham>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Activate stun effect on enemies
            StartCoroutine(StunEnemies());

            // Particle effect for visual feedback
            Instantiate(particleStunBomb, transform.position, Quaternion.identity);

            // Destroy the stun bomb object
            Destroy(gameObject);
        }
    }

    IEnumerator StunEnemies()
    {
        // Get all enemies in the scene 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemyAiEidham enemyAi = enemy.GetComponent<EnemyAiEidham>();
            if (enemyAi != null)
            {
                // Stun the enemy by stopping their movement for the specified duration
                enemyAi.StunEnemy(stunDuration);

                // Play a stun animation or effect on the enemy
                enemyAi.PlayStunAnimation();
            }
        }

        // Wait for the stun duration before restoring enemy movement
        yield return new WaitForSeconds(stunDuration);

        // Restore enemy movement after the stun period
        foreach (GameObject enemy in enemies)
        {
            EnemyAiEidham enemyAi = enemy.GetComponent<EnemyAiEidham>();
            if (enemyAi != null)
            {
                enemyAi.EndStun();
            }
        }
    }
}
