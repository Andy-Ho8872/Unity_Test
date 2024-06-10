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
    public MovementController movementController;
    public AttackController attackController;
    public Animator animator;
    // Variables for player
    [SerializeField] private float HP = 10;
    [SerializeField] private bool isDead = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Make sure that the player won't jump multiple times in the air
        if (other.gameObject.CompareTag("Ground"))
        {
            movementController.isGrounded = true;
            movementController.animator.SetBool("isJumping", false);
        }
        // The player will get hit when colliding with monsters
        if (other.gameObject.CompareTag("Monster"))
        {
            HP = HP - 1;
            detectIfPlayerIsDead();
        }
    }
    void Update()
    {
        //todo If the player is dead just return
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
    // FixedUpdate will be called every fixed frame(built-in function in Unity)
    // Commonly used for physic-related functions
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
    // When the player is dead
    private void detectIfPlayerIsDead()
    {
        if (HP <= 0)
        {
            HP = 0;
            isDead = true;
            animator.SetTrigger("isDead");
        }
    }
}
