using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerEidham : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval; // Time interval between spawns
    
    

    private void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Choose a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // Choose a random enemy prefab
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyPrefab = enemyPrefabs[enemyIndex];

            // Spawn the enemy at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
