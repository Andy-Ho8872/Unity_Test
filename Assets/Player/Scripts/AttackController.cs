using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private float OffsetY = 3f;
    // Get access to 
    public GameObject Player;
    public GameObject FireballPrefab;
    public Animator animator;
    public void fireball()
    {
        Vector3 playerPosition = Player.transform.position;
        Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y + OffsetY, playerPosition.z);
        // Create fire ball 
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(FireballPrefab, newPosition, Quaternion.identity);
            animator.SetTrigger("isAttacking");
            GameManager.Instance.audioManager.playAudioClip(0, "player_attack", false);
        }
    }
}