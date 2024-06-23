using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterController : MonoBehaviour

{
    // Get access to
    public Animator animator;
    public GameObject MonsterPrefab;
    public GameObject MonsterAttackPrefab;
    public MonsterAttackController monsterAttackController;
    public Player Player;
    public SpriteRenderer monsterSprite;
    public GameObject HP_Bar;
    // local variables
    public enum Behavior { Idle, IsRunning, IsEscaping, IsAttacking, IsDead };
    public enum FacingDirection { Left, Right };
    public Behavior behavior;
    public FacingDirection facingDirection;
    public float max_HP = 5;
    public float current_HP = 5;
    public float lowHP = 2;
    public float moveSpeed = 2;
    [SerializeField] private float playerPositionX;
    [SerializeField] private float monsterPositionX;
    [SerializeField] private float distanceBetweenPlayerAndMonster;
    [SerializeField] private float chaseDetectionArea = 12;
    [SerializeField] private float attackDetectionArea = 8;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // Set original status
        behavior = Behavior.Idle;
    }
    void Update()
    {
        updateChasingArea();
        setMonsterStatus();
    }
    // Control the flip of the sprite
    public void setMonsterSprite(FacingDirection direction)
    {
        if (direction == FacingDirection.Left)
        {
            monsterSprite.flipX = true;
        }
        else if (direction == FacingDirection.Right)
        {
            monsterSprite.flipX = false;
        }
    }
    // Control the status of monster
    public void setMonsterStatusOnCollision()
    {
        // When the monster is in low HP
        if (current_HP > 0 && current_HP <= lowHP)
        {
            behavior = Behavior.IsEscaping;
        }
        // When the monster is dead
        if (current_HP <= 0)
        {
            isDead = true;
        }
    }
    public void updateMonsterHP_Bar()
    {
        // When the monster is getting hit, update the HP bar
        Vector3 originalScale = HP_Bar.transform.localScale;
        HP_Bar.transform.localScale = new Vector3(current_HP / max_HP, originalScale.y, originalScale.z);
    }
    public void setMonsterStatus()
    {
        if (isDead)
        {
            behavior = Behavior.IsDead;
        }
        // When the is near player and in low HP status
        else if (distanceBetweenPlayerAndMonster <= chaseDetectionArea && current_HP <= lowHP && !isDead)
        {
            behavior = Behavior.IsEscaping;
        }
        // When player entered the attack area
        else if (behavior != Behavior.IsEscaping)
        {
            if (distanceBetweenPlayerAndMonster <= attackDetectionArea)
            {
                canAttack = true;
                behavior = Behavior.IsAttacking;
            }
            // When player left the chase detection area
            else if (distanceBetweenPlayerAndMonster >= chaseDetectionArea)
            {
                behavior = Behavior.Idle;
            }
            // When the monster is near the player and has enough HP
            else if (distanceBetweenPlayerAndMonster <= chaseDetectionArea && !canAttack)
            {
                behavior = Behavior.IsRunning;
            }
            // When player left the chase detection area
            else if (distanceBetweenPlayerAndMonster >= attackDetectionArea)
            {
                canAttack = false;
            }
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
                break;
            case Behavior.IsRunning:
                // Cancel the animation
                animator.SetBool("isInDetectionArea", false);
                // Add animation
                animator.SetBool("isRunning", true);
                // Chase the player
                chasePlayer();
                // Select moving direction
                switch (facingDirection)
                {
                    // Move left
                    case FacingDirection.Left:
                        setMonsterSprite(FacingDirection.Left);
                        monsterMove(FacingDirection.Left);
                        break;
                    // Move right
                    case FacingDirection.Right:
                        setMonsterSprite(FacingDirection.Right);
                        monsterMove(FacingDirection.Right);
                        break;
                }
                break;
            case Behavior.IsEscaping:
                // Cancel the animation
                animator.ResetTrigger("isAttacking");
                animator.SetBool("isInDetectionArea", false);
                // Add animation
                animator.SetBool("isEscaping", true);
                // Make the monster escape to left side
                if (distanceBetweenPlayerAndMonster <= chaseDetectionArea && playerPositionX > monsterPositionX)
                {
                    setMonsterSprite(FacingDirection.Left);
                    monsterMove(FacingDirection.Left);
                }
                // Make the monster escape to right side
                else if (distanceBetweenPlayerAndMonster <= chaseDetectionArea && playerPositionX < monsterPositionX)
                {
                    setMonsterSprite(FacingDirection.Right);
                    monsterMove(FacingDirection.Right);
                }
                // If the monster has left the chasing area
                else if (distanceBetweenPlayerAndMonster > chaseDetectionArea)
                {
                    behavior = Behavior.Idle;
                }
                break;
            case Behavior.IsAttacking:
                animator.SetTrigger("isAttacking");
                animator.SetBool("isRunning", false);
                animator.SetBool("isInDetectionArea", true);
                // todo wrap the code as function
                // StartCoroutine(monsterAttackController.monsterAttack());
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
