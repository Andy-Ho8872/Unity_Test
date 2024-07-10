using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackController : MonoBehaviour
{
    public GameObject MonsterPrefab;
    public Vector3 MonsterPosition;
    public SpriteRenderer MonsterSprite;
    public GameObject MonsterAttackPrefab;
    public bool isShootingRight;
    public float shootRange = 0.5f;
    public float offsetX = 1f;
    public float offsetY = 1.25f;
    public float attackCoolDown = 0.75f;
    private void Start()
    {
        MonsterPrefab = GameObject.Find("Goblin"); // Monster game object
        MonsterSprite = MonsterPrefab.GetComponent<SpriteRenderer>();
        setShootingStatus();
    }
    void Update()
    {
        setMonsterPosition();
        shoot();
    }
    public void setShootingStatus()
    {
        // If monster is facing left
        if (MonsterSprite.flipX) isShootingRight = false;
        else isShootingRight = true;
    }
    public void shoot()
    {
        if (isShootingRight) MonsterAttackPrefab.transform.position += new Vector3(shootRange * Time.deltaTime * 40, 0, 0);
        else MonsterAttackPrefab.transform.position -= new Vector3(shootRange * Time.deltaTime * 40, 0, 0);
    }
    public void setMonsterPosition()
    {
        MonsterPosition = MonsterPrefab.transform.position;
    }
    // Create bullets
    public void createBullet()
    {
        attackCoolDown -= Time.deltaTime;
        Vector3 newPosition = new Vector3(MonsterPosition.x, MonsterPosition.y - offsetY, MonsterPosition.z);
        if (attackCoolDown <= 0)
        {
            GameObject bullet = Instantiate(MonsterAttackPrefab, newPosition, Quaternion.identity);
            // Add sound effect
            GameManager.Instance.audioManager.playAudioClip(3, "monster_attack", false);
            // Reset timer
            attackCoolDown = 0.75f;
            // Delete bullet
            Destroy(bullet, 2f);
        }
    }
}
