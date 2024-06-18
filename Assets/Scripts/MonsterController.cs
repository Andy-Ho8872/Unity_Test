using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour

{
     // Get access to
    public Animator animator;
    public GameObject MonsterPrefab;
    public Player Player;
    public SpriteRenderer monsterSprite;
    public GameObject HP_Bar;
    // local variables
    public enum Behavior { Idle, IsRunning, IsEscaping, IsDead };
    public enum FacingDirection { Left, Right };
    public Behavior behavior;
    public FacingDirection facingDirection;
    public float max_HP = 5;
    public float current_HP = 5;
    public float lowHP = 2;
    public float moveSpeed = 2;
    private float playerPositionX;
    private float monsterPositionX;
    private float distanceBetweenPlayerAndMonster;
    private float escapeZone;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // Set original status
        behavior = Behavior.Idle;
    }
    // Control the flip of the sprite
    public void setMonsterSprite()
    {
        if (facingDirection == FacingDirection.Left)
        {
            monsterSprite.flipX = true;
        }
        else
        {
            monsterSprite.flipX = false;
        }
    }
    // Control the status of monster
    public void setMonsterStatus()
    {
        // When the monster is getting hit, update the HP bar
        Vector3 originalScale = HP_Bar.transform.localScale;
        HP_Bar.transform.localScale = new Vector3(current_HP / max_HP, originalScale.y, originalScale.z);
        // When the monster is in low HP
        if (current_HP > 0 && current_HP <= lowHP)
        {
            behavior = Behavior.IsEscaping;
        }
        // When the monster is dead
        if (current_HP <= 0)
        {
            behavior = Behavior.IsDead;
        }
    }
    // Make the monster chase the player
    public void chasePlayer()
    {
        // The player is on the right side of the monster
        if (monsterPositionX >= playerPositionX)
        {
            facingDirection = FacingDirection.Left;
        }
        else
        {
            facingDirection = FacingDirection.Right;
        }
    }
    public void updateChasingArea()
    {
        // local variables
        playerPositionX = Player.transform.position.x;
        monsterPositionX = MonsterPrefab.transform.position.x;
        distanceBetweenPlayerAndMonster = Mathf.Abs(playerPositionX - monsterPositionX);
        escapeZone = 8;
    }
    // Controls the movement of monster
    public void monsterMove(FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.Left:
                MonsterPrefab.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                break;
            case FacingDirection.Right:
                MonsterPrefab.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                break;
        }
    }
     // Controls the behavior of monster
    public void setMonsterBehavior()
    {
        switch (behavior)
        {
            case Behavior.Idle:
                // Cancel the animation
                animator.SetBool("isRunning", false);
                animator.SetBool("isEscaping", false);
                // If the monster entered the escape zone with low HP
                if (distanceBetweenPlayerAndMonster <= escapeZone && current_HP <= lowHP)
                {
                    behavior = Behavior.IsEscaping;
                }
                break;
            case Behavior.IsRunning:
                // Add animation
                animator.SetBool("isRunning", true);
                // Chase the player
                chasePlayer();
                // Select moving direction
                switch (facingDirection)
                {
                    // Move left
                    case FacingDirection.Left:
                        monsterMove(FacingDirection.Left);
                        break;
                    // Move right
                    case FacingDirection.Right:
                        monsterMove(FacingDirection.Right);
                        break;
                }
                break;
            case Behavior.IsEscaping:
                // Add animation
                animator.SetBool("isEscaping", true);
                // Make the monster escape to left side
                if (distanceBetweenPlayerAndMonster <= escapeZone && playerPositionX > monsterPositionX)
                {
                    monsterSprite.flipX = true;
                    monsterMove(FacingDirection.Left);
                }
                // Make the monster escape to right side
                else if (distanceBetweenPlayerAndMonster <= escapeZone && playerPositionX < monsterPositionX)
                {
                    monsterSprite.flipX = false;
                    monsterMove(FacingDirection.Right);
                }
                // If the monster has left the escape zone
                else if (distanceBetweenPlayerAndMonster > escapeZone)
                {
                    behavior = Behavior.Idle;
                }
                break;
            case Behavior.IsDead:
                // Cancel the animation
                animator.SetBool("isEscaping", false);
                // Add death animation
                animator.SetTrigger("isDead");
                // Delete the monster object
                Destroy(MonsterPrefab, 2f);
                break;
        }
    }
}
