using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireballController : MonoBehaviour
{
    public GameObject FireballPrefab;
    public SpriteRenderer fireballSprite;
    public AttackController attackController;
    public float shootRange = 0.5f;
    public bool isShootingRight = true;
    public float timer = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        // Get access to 
        attackController = GameObject.FindGameObjectWithTag("AttackController").GetComponent<AttackController>();
        // To determine the firing direction
        setShootingStatus();
    }
    // Update is called once per frame
    void Update()
    {
        shoot();
        deleteFireball();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the fireball hits the monster, delete it
        if (other.gameObject.CompareTag("Monster"))
        {
            Destroy(FireballPrefab, 0.15f);
        }
    }
    void setShootingStatus()
    {
        if (attackController.Player.transform.localScale.x == 1)
        {
            isShootingRight = true;
            fireballSprite.flipX = false;
        }
        else
        {
            isShootingRight = false;
            fireballSprite.flipX = true;
        }
    }
    void shoot()
    {
        if (isShootingRight)
        {
            FireballPrefab.transform.position += new Vector3(shootRange * Time.deltaTime * 60, 0, 0);
        }
        else
        {
            FireballPrefab.transform.position -= new Vector3(shootRange * Time.deltaTime * 60, 0, 0);
        }
    }
    // Delete the fireball after a couple of seconds 
    void deleteFireball()
    {
        timer = timer - Time.deltaTime;
        if (FireballPrefab && timer <= 0)
        {
            Destroy(FireballPrefab, timer);
        }
    }
}
