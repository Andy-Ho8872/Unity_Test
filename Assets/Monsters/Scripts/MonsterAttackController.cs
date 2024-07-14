using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackController : MonoBehaviour
{
    public GameObject Player;
    public Monster MonsterPrefab;
    public GameObject MonsterAttackPrefab;
    public Vector3 MonsterPosition;

    public float offsetY = 1.25f;
    public float attackCoolDown = 0.75f;
    private void Start()
    {
        Player = GameObject.Find("Player");
    }
    void Update()
    {
        setMonsterPosition();
    }
    public void setMonsterPosition()
    {
        if (MonsterPrefab) MonsterPosition = MonsterPrefab.transform.position;
    }
    // Create bullets
    public void createBullet()
    {
        attackCoolDown -= Time.deltaTime;
        Vector3 newPosition = new Vector3(MonsterPosition.x, MonsterPosition.y - offsetY, MonsterPosition.z);
        if (attackCoolDown <= 0)
        {
            // Create bullets
            GameObject bulletPrefab = Instantiate(MonsterAttackPrefab, newPosition, Quaternion.identity);
            // Sets the owner of the current bullet
            Bullet bulletScript = bulletPrefab.GetComponent<Bullet>();
            if (bulletScript != null) bulletScript.setOwner(MonsterPrefab);
            // Add sound effect
            GameManager.Instance.audioManager.playAudioClip(3, "monster_attack", false);
            // Reset timer
            attackCoolDown = 0.75f;
            // Delete bullet
            Destroy(bulletPrefab, 2f);
        }
    }
}
