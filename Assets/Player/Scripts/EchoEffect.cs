using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EchoEffect : MonoBehaviour
{
    private float timeBetweenSpawns;
    public float startTimeBetweenSpawns = 0.05f;
    public GameObject Player;
    public GameObject PlayerEcho;
    public MovementController movementController;
    // Start is called before the first frame update
    void Start()
    {
        // Make the echo invisible
        PlayerEcho.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize the echo position to player position
        Vector3 playerPosition = Player.transform.position;
        PlayerEcho.transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
        // Check if player is moving or dashing
        if (movementController.isDashing)
        {
            // Make echo visible
            PlayerEcho.SetActive(true);
            if (timeBetweenSpawns <= 0)
            {
                // flip echos
                PlayerEcho.transform.localScale = new Vector3(Player.transform.localScale.x, Player.transform.localScale.y, Player.transform.localScale.z);
                // Create echos
                GameObject instance = Instantiate(PlayerEcho, transform.position, Quaternion.identity);
                // Delete echos
                Destroy(instance, 1f);
                // Set the start time between spawn
                timeBetweenSpawns = startTimeBetweenSpawns;
            }
            else
            {
                timeBetweenSpawns = timeBetweenSpawns - Time.deltaTime;
            }
        }
        // Make PlayerEcho prefab invisible
        else
        {
            PlayerEcho.SetActive(false);
        }
    }
}
