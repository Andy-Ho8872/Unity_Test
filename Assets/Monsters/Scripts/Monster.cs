using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // Get access to
    public MonsterController monsterController;
    
    private void Update()
    {
        monsterController.setMonsterBehavior();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack") && monsterController.current_HP >= 1)
        {
            // Take damage
            monsterController.current_HP -= 1;
            // Play animation
            monsterController.animator.SetTrigger("isGettingHit");
            // Update monster status
            monsterController.updateMonsterHP_Bar();
            monsterController.setMonsterStatusOnCollision();
            // Play sound
            GameManager.Instance.audioManager.playAudioClip(4, "monster_hurt", false);
            // Generate damage number above the monster...   the Vector3.up and Vector3.right is the offset value
            GameManager.Instance.UI.generateHitNumber(1, transform.position);
        }
    }
}