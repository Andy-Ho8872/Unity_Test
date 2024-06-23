using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackController : MonoBehaviour
{
    public GameObject MonsterPrefab;
    public GameObject MonsterAttackPrefab;
    public float attackCoolDown = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }
    // todo bug detected
    public IEnumerator monsterAttack()
    {
        Vector3 monsterPosition = MonsterPrefab.transform.position;
        Vector3 newPosition = new Vector3(monsterPosition.x, monsterPosition.y, monsterPosition.z);
        Instantiate(MonsterAttackPrefab, newPosition, Quaternion.identity);
        yield return new WaitForSeconds(attackCoolDown);
        // Delete bullet
        Destroy(MonsterAttackPrefab, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
