using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;


public class Player : MonoBehaviour
{
    // Get access to 
    public Player player;
    public MovementController movementController;
    public AttackController attackController;
    public GameObject Player_HP_Bar_Base;
    public GameObject Player_HP_Bar;
    public Animator animator;
    [SerializeField] private Rigidbody2D PlayerRB;
    // Variables for player
    [SerializeField] private float max_HP = 10;
    [SerializeField] private float current_HP = 10;
    [SerializeField] private bool isDead = false;
        private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerRB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        placePlayerHPBar();
        if (isDead) return;
        // Prevent the player from moving, jumping, flipping while dashing
        if (movementController.isDashing) return;
        // Control the player movement
        movementController.run();
        movementController.jump();
        // Control the attack
        attackController.fireball();
        detectIfPlayerIsDead();
    }
        private void FixedUpdate()
    {
        // Prevent the player from moving, jumping, flipping while dashing
        if (movementController.isDashing) return;
        // Detect if the player can dash
        if (Input.GetKey(KeyCode.LeftShift) && movementController.canDash)
        {
            // like await in Javascript? 
            StartCoroutine(movementController.dash());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Make sure that the player won't jump multiple times in the air
        if (collision.gameObject.CompareTag("Ground"))
        {
            movementController.isGrounded = true;
            movementController.animator.SetBool("isJumping", false);
        }
        // The player will get hit when colliding with monsters
        if (collision.gameObject.CompareTag("Monster"))
        {
            movementController.shouldLimitSpeed = false; // Disable the limitation for moving speed
            ContactPoint2D contactPoint = collision.GetContact(0); // 1(right side) ~ -1(left side)
            knockBack(contactPoint);
            takeDamage(1);
            setPlayerStatus();
            detectIfPlayerIsDead();
        }
    }
    private void OnCollisionExit2D(Collision2D collision) 
    {
        // when the collision between player and monster is over, reset the value
        if(collision.gameObject.CompareTag("Monster")) {
            movementController.shouldLimitSpeed = true;
        }
    }
    // FixedUpdate will be called every fixed frame(built-in function in Unity)
    // Commonly used for physic-related functions
    private void placePlayerHPBar()
    {
        Vector3 playerPosition = player.transform.position;
        float xOffset = 2f;
        float yOffset = 4.5f;
        Player_HP_Bar_Base.transform.position = new Vector3(playerPosition.x + xOffset, playerPosition.y + yOffset, playerPosition.z);
    }
    private void setPlayerStatus()
    {
        // When the player is getting hit, update the HP bar
        Vector3 originalScale = Player_HP_Bar.transform.localScale;
        Player_HP_Bar.transform.localScale = new Vector3(current_HP / max_HP, originalScale.y, originalScale.z);
    }
    // When the player is taking damage
    private void takeDamage(float damage)
    {
        current_HP = current_HP - damage;
    }
    private void knockBack(ContactPoint2D contactPoint) {
        float pushingDirection = contactPoint.normal.x; // 1 or -1
        float pushingPower = 1000f;
        PlayerRB.AddForce(new Vector2(pushingDirection * pushingPower, 0), ForceMode2D.Impulse);
    }
    // When the player is dead
    private void detectIfPlayerIsDead()
    {
        if (current_HP <= 0)
        {
            current_HP = 0;
            isDead = true;
            animator.SetTrigger("isDead");
        }
    }
}
