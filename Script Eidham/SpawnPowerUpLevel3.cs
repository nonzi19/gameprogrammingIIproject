using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUpLevel3 : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; 

    // Coroutine to spawn power-ups at intervals
    public void SpawnPowerUpAtPosition(Vector3 position)
    {
        // Choose a random power-up prefab
        int prefabIndex = Random.Range(0, powerUpPrefabs.Length);
        GameObject selectedPowerUpPrefab = powerUpPrefabs[prefabIndex];

        // Spawn the selected power-up at the given position
        Instantiate(selectedPowerUpPrefab, position, Quaternion.identity);
    }
}
