using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraFollowSpeed= 3f;
    public Transform Player;
    private float cameraOffsetX = 0.1f;
    private float cameraOffsetY = 8f;
    // Update is called once per frame
    void Update()
    {  
        // Get the current position of the Player (x, y, z)
        Vector3 newPosition = new Vector3(Player.position.x - cameraOffsetX, Player.position.y + cameraOffsetY, -10f); // 10f default position Z of camera
        // Change the current camera position to Player position --- Vector3.Slerp(pointA, pointB, time)
        gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, newPosition, cameraFollowSpeed * Time.deltaTime);
    }
}
