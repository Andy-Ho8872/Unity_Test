using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FireballController : MonoBehaviour
{
    public Player player;
    public GameObject FireballPrefab;
    public SpriteRenderer fireballSprite;
    public float shootRange = 0.5f;
    public bool isShootingRight = true;
    public int skill_damage = 1;
    public float skill_coolDown = 1f;
    public float skill_coolDown_left = 0f; // can be used in default
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        // To determine the firing direction
        setShootingStatus();
    }
    // Update is called once per frame
    void Update()
    {
        setSkillCoolDownLeft();
        shoot();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the fireball hits the monster, delete it
        if (collision.gameObject.CompareTag("Monster"))
        {
            GameManager.Instance.attackController.applyDamageToMonster(collision, skill_damage, FireballPrefab, 0.15f);
        }
    }
    void setShootingStatus()
    {
        if (player.transform.localScale.x == 1)
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
    void setSkillCoolDownLeft()
    {
        // if it's in coolDown
        if (skill_coolDown_left > 0)
        {
            skill_coolDown_left -= Time.deltaTime;
            GameManager.Instance.UI.skillIconTransition(0, skill_coolDown_left, skill_coolDown);
        }
        // if the coolDown is over
        else if (skill_coolDown_left < 0)
        {
            skill_coolDown_left = 0;
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
}