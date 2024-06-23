using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Variables for jumping 
    public bool isGrounded = true;
    public float jumpHeight = 60f;
    // Variables for moving
    public float defaultLinearDrag = 5;
    public bool shouldLimitSpeed = true;
    public float moveSpeed = 2.5f;
    public float moveSpeedLimit = 2.5f;
    // Variables for Dashing
    public bool isDashing = false;
    public bool canDash = true;
    private float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCoolDown = 2f;
    // Get access to 
    public Rigidbody2D PlayerRB;
    public Player Player;
    public Animator animator;
    // This function controls the movement of the player with coordinates(x ,y) 
    public void run()
    {
        float faceLeft = -1;
        float faceRight = 1;
        float moveLeft = -30000;
        float moveRight = 30000;
        Vector2 playerVelocity = PlayerRB.velocity;
        // Get the scale of the object
        Vector3 playerScale = Player.transform.localScale;
        // Reset the velocity to 0, which ensures the player will not be affected by friction = 0
        playerVelocity.x = 0;
        // Speed limitation
        limitSpeed();
        // Use LeftArrow" and "RightArrow" to move the player (Horizontal control)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Make the player face and move left
            moveWithDirection(playerScale, faceLeft, moveLeft);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Make the player face and move right
            moveWithDirection(playerScale, faceRight, moveRight);
        }
        else
        {
            // Add animation
            animator.SetBool("isRunning", false);
        }
    }
    // This function controls the player's facing direction、animation、and moving velocity
    // objectScale = Player's scale
    // direction = 1(face right) or -1(face left)
    // velocity = move power
    private void moveWithDirection(Vector3 objectScale, float direction, float velocity)
    {
        // To detect the facing direction
        Player.transform.localScale = new Vector3(direction, objectScale.y, objectScale.z);
        // Add animation
        animator.SetBool("isRunning", true);
        // Move the player based on velocity
        PlayerRB.AddForce(new Vector2(velocity * moveSpeed * Time.deltaTime, 0), ForceMode2D.Force);
        // Disable running animation when jumping
        if (!isGrounded)
        {
            animator.SetBool("isRunning", false);
        }
    }
    public void jump()
    {
        checkIsGrounded();
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            // Sets the linear drag to 0
            PlayerRB.drag = 0;
            // Use "UpArrow" to make the player jump (Vertical control)
            PlayerRB.AddForce(new Vector2(PlayerRB.velocity.x, jumpHeight), ForceMode2D.Impulse);
            // Add animation
            animator.SetBool("isJumping", true);
            // The Player is jumping
            isGrounded = false;
        }
        // reset the linear drag to default value
        else if (isGrounded)
        {
            PlayerRB.drag = defaultLinearDrag;
            animator.SetBool("isJumping", false);
        }
    }
    private void checkIsGrounded()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Player.transform.position, Vector2.down, 1f);
        // When the player is falling
        if (PlayerRB.velocity.y <= 0)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("GroundTileMap"))
                {
                    isGrounded = true;
                }
            }
        }
    }
    public void limitSpeed()
    {
        if (!shouldLimitSpeed) return;
        // limitations for moving left
        if (PlayerRB.velocity.x <= -moveSpeedLimit)
        {
            PlayerRB.velocity = new Vector2(-moveSpeedLimit, PlayerRB.velocity.y);
        }
        // limitations for moving right
        if (PlayerRB.velocity.x >= moveSpeedLimit)
        {
            PlayerRB.velocity = new Vector2(moveSpeedLimit, PlayerRB.velocity.y);
        }
    }
    // The IEnumerator(Coroutine function) is kind of like async function in Javascript...? 
    public IEnumerator dash()
    {
        // When the player is about to dash
        canDash = false;
        isDashing = true;
        PlayerRB.drag = 0;
        // Get the original gravity of player, the default value is 1(1G)
        float originalGravity = PlayerRB.gravityScale;
        // Ensure that the player will not be affected by gravity while dashing
        PlayerRB.gravityScale = 0f;
        // Give dashing power to the player and lock the vertical velocity
        PlayerRB.velocity = new Vector2(Player.transform.localScale.x * dashingPower, 0f);
        // After casting the dash, reset to original value
        yield return new WaitForSeconds(dashingTime);
        PlayerRB.gravityScale = originalGravity;
        isDashing = false;
        // When the CoolDown of dash is over
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }
}
