using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // Get access to
    public MonsterController monsterController;
    
    private void Update()
    {
        monsterController.updateChasingArea();
        monsterController.setMonsterSprite();
        monsterController.setMonsterBehavior();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            monsterController.current_HP -= 1;
            monsterController.animator.SetTrigger("isGettingHit");
            monsterController.setMonsterStatus();
        }
    }
}