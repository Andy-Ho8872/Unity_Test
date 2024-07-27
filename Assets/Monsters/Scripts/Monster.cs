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
        if (other.gameObject.CompareTag("Attack") && monsterController.current_HP >= 0)
        {
            // Update monster status
            monsterController.updateMonsterHP_Bar();
            monsterController.setMonsterStatusOnCollision();
            // Play animation
            monsterController.animator.SetTrigger("isGettingHit");
            // Play sound
            GameManager.Instance.audioManager.playAudioClip(4, "monster_hurt", false);
        }
    }
}