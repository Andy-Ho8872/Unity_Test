using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public bool isShootingRight;
    public float shootRange = 0.5f;
    private Monster ownerMonster;
    private SpriteRenderer monsterSprite;
    void Start()
    {
        setShootingStatus();
    }
    void Update()
    {
        shoot();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the attack hits the player
        if (collision.CompareTag("Player"))
        {
            Destroy(bulletPrefab);
        }
    }
    public void setOwner(Monster monster)
    {
        ownerMonster = monster;
        monsterSprite = monster.GetComponent<SpriteRenderer>();
    }
    public void setShootingStatus()
    {
        // If monster is facing left
        if (monsterSprite.flipX) isShootingRight = false;
        else isShootingRight = true;
    }
    public void shoot()
    {
        if (isShootingRight) bulletPrefab.transform.position += new Vector3(shootRange * Time.deltaTime * 40, 0, 0);
        else bulletPrefab.transform.position -= new Vector3(shootRange * Time.deltaTime * 40, 0, 0);
    }
}
