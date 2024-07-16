using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private float OffsetY = 3f;
    // Get access to 
    public Player player;
    public GameObject FireballPrefab;
    public Animator animator;
    public FireballController fireballController;
    public void fireball()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y + OffsetY, playerPosition.z);
        // Create fire ball 
        if (Input.GetKeyDown(KeyCode.Z) && fireballController.skill_coolDown_left <= 0)
        {
            Instantiate(FireballPrefab, newPosition, Quaternion.identity);
            animator.SetTrigger("isAttacking");
            GameManager.Instance.audioManager.playAudioClip(0, "player_attack", false);
            // reset the coolDown timer
            fireballController.skill_coolDown_left = fireballController.skill_coolDown;
        }
    }
}