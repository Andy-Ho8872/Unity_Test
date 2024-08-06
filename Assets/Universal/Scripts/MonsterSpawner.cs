using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Player player;
    [Header("Monsters")]
    public GameObject[] MonsterPrefabs;
    [Header("Attributes")]
    public float spawnArea = 10f; // ex -10 ~ 10
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            spawnMonster(MonsterPrefabs);
        }
    }
    public void spawnMonster(GameObject[] monsters)
    {
        // Get random index of from all monsters
        int randomIndex = Random.Range(0, monsters.Length);
        // Get the player's position
        Vector3 playerPosition = player.transform.position;
        // Set the spawn area //todo: The monster may have spawned outside the map...
        float offset = 3f;
        float spawnRangeX = Random.Range(playerPosition.x - spawnArea - offset, playerPosition.x + spawnArea + offset);
        Vector3 spawnPosition = new Vector3(spawnRangeX, playerPosition.y + offset, playerPosition.z);
        // Randomly spawn monster
        Instantiate(monsters[randomIndex], spawnPosition, Quaternion.identity);
    }
}
