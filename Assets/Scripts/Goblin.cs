using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    // Get access to
    public Animator animator;
    public GameObject MonsterPrefab;
    public GameObject Player;
    public SpriteRenderer monsterSprite;
    // local variables
    public enum Behavior { Idle, IsRunning, IsEscaping, IsDead };
    public enum FacingDirection { Left, Right };
    public Behavior behavior;
    public FacingDirection facingDirection;
    public float HP = 5;
    public float lowHP = 2;
    public float moveSpeed = 2;
    private float playerPositionX;
    private float monsterPositionX;
    private float distanceBetweenPlayerAndMonster;
    private float escapeZone;

    private void Start()
    {
        // Set original status
        behavior = Behavior.Idle;
    }
    private void Update()
    {
        updateCurrentPosition();
        setMonsterSprite();
        setMonsterBehavior();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            HP = HP - 1;
            animator.SetTrigger("isGettingHit");
            setMonsterStatus();
        }
    }
    private void updateCurrentPosition() {
        // local variables
        playerPositionX = Player.transform.position.x;
        monsterPositionX = MonsterPrefab.transform.position.x;
        distanceBetweenPlayerAndMonster = Mathf.Abs(playerPositionX - monsterPositionX);
        escapeZone = 8;
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
        // When the monster is in low HP
        if (HP > 0 && HP <= lowHP)
        {
            behavior = Behavior.IsEscaping;
        }
        // When the monster is dead
        if (HP <= 0)
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
    // Controls the movement of monster
    public void monsterMove(FacingDirection facingDirection) {
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
                // Cancel the running animation
                animator.SetBool("isRunning", false);
                // If the monster entered the escape zone with low HP
                if (distanceBetweenPlayerAndMonster <= escapeZone && HP <= lowHP)
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
                animator.SetBool("isRunning", true);
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
                // Cancel the running animation
                animator.SetBool("isRunning", false);
                // Add death animation
                animator.SetTrigger("isDead");
                // Delete the monster object
                Destroy(MonsterPrefab, 2f);
                break;
        }
    }
}