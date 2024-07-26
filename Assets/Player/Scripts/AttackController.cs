using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // Get access to 
    public Player player;
    public Animator animator;
    public GameObject[] monsters;
    [Header("Skill Prefabs")]
    public GameObject FireballPrefab;
    public GameObject IciclePrefab;
    [Header("Scripts")]
    public FireballController fireballController;
    public IcicleController icicleController;
    private void Update()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
    }
    public bool canCastSkill()
    {
        if (monsters.Length > 0) return true;
        else return false;
    }
    public void fireball()
    {
        float OffsetY = 3f;
        Vector3 playerPosition = player.transform.position;
        Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y + OffsetY, playerPosition.z);
        // Create fire ball 
        if (Input.GetKeyDown(KeyCode.Z) && fireballController.skill_coolDown_left <= 0)
        {
            GameObject fireball = Instantiate(FireballPrefab, newPosition, Quaternion.identity);
            // Animation
            animator.SetTrigger("isAttacking");
            // Play sound effect
            GameManager.Instance.audioManager.playAudioClip(0, "player_attack", false);
            // Reset the coolDown timer
            fireballController.skill_coolDown_left = fireballController.skill_coolDown;
            // Delete the fireball after 1.5 seconds
            Destroy(fireball, 1.5f);
        }
    }
    public void icicle()
    {
        // Create icicle 
        if (Input.GetKeyDown(KeyCode.X) && icicleController.skill_coolDown_left <= 0 && canCastSkill())
        {
            // Find all the monster in the scene and generate icicle from their position
            foreach (var monster in monsters)
            {
                Vector3 monsterPosition = monster.transform.position;
                GameObject icicle = Instantiate(IciclePrefab, monsterPosition, Quaternion.identity);
            }
            // Animation
            animator.SetTrigger("isAttacking");
            // Play sound effect
            GameManager.Instance.audioManager.playAudioClip(0, "player_attack", false);
            // Reset the coolDown timer
            icicleController.skill_coolDown_left = icicleController.skill_coolDown;
        }
    }
}