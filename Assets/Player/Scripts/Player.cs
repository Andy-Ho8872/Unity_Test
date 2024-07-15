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
    public UI UI;
    public MovementController movementController;
    public AttackController attackController;
    public Animator animator;
    public float max_HP = 10;
    public float current_HP = 10;
    [SerializeField] private Rigidbody2D PlayerRB;
    // Variables for player
    [SerializeField] private bool isDead = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerRB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (isDead) return;
        // Prevent the player from moving, jumping, flipping while dashing
        if (movementController.isDashing) return;
        // Control the player movement
        movementController.run();
        movementController.jump();
        // Control the attack
        attackController.fireball();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MonsterAttack"))
        {
            if (isDead) return;
            detectIfPlayerIsDead();
            takeDamage(1);
            UI.updatePlayerHP_Bar_UI();
            // play animation and sound
            animator.SetTrigger("isGettingHit");
            GameManager.Instance.audioManager.playAudioClip(2, "player_hurt", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the collision contact point
        ContactPoint2D contactPoint = collision.GetContact(0);
        // When The player is colliding with monsters
        if (collision.gameObject.CompareTag("Monster"))
        {
            movementController.shouldLimitSpeed = false; // Disable the limitation for moving speed
            knockBack(contactPoint);
            takeDamage(1);
            UI.updatePlayerHP_Bar_UI();
            detectIfPlayerIsDead();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // When the collision between player and monster is over, reset the value
        if (collision.gameObject.CompareTag("Monster"))
        {
            movementController.shouldLimitSpeed = true;
        }
    }
    // When the player is taking damage
    private void takeDamage(float damage)
    {
        if (current_HP >= 1) current_HP = current_HP - damage;
    }
    private void knockBack(ContactPoint2D contactPoint)
    {
        float pushingDirection = contactPoint.normal.x; // 1 or -1
        float pushingPower = 500f;
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
