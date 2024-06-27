using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackController : MonoBehaviour
{
    public GameObject MonsterPrefab;
    public Vector3 monsterPosition;
    public GameObject MonsterAttackPrefab;
    public float shootRange = 0.5f;
    public float offsetX = 1f;
    public float offsetY = 1.25f;
    public float attackCoolDown = 0.75f;
    private void Start() 
    {
        MonsterPrefab = GameObject.Find("Goblin"); 
        //todo test 
        Debug.Log(MonsterPrefab);
        if(MonsterPrefab != null) Debug.Log($"Monster position is {monsterPosition.x}");
        else Debug.Log("Monster not found");    
    }
    void Update()
    {
        setMonsterPosition();  
        // Fire
        MonsterAttackPrefab.transform.position -= new Vector3(shootRange * Time.deltaTime * 40, 0, 0); 
    }
    public void setMonsterPosition()
    {
        monsterPosition = MonsterPrefab.transform.position;
    }
    public void monsterAttack()
    {
        attackCoolDown -= Time.deltaTime;
        Vector3 newPosition = new Vector3(monsterPosition.x, monsterPosition.y - offsetY, monsterPosition.z);
        if (attackCoolDown <= 0)
        {
            GameObject bullet = Instantiate(MonsterAttackPrefab, newPosition, Quaternion.identity);
            // Reset timer
            attackCoolDown = 0.75f;
            // Delete bullet
            Destroy(bullet, 2f);
        }
    }
}
