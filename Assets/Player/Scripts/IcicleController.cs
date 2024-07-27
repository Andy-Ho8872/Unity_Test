using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour
{
    public GameObject iciclePrefab;
    public int skill_damage = 2;
    public float skill_coolDown = 2f;
    public float skill_coolDown_left = 0f; // can be used in default

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the skill hits the monster, delete it
        if (collision.gameObject.CompareTag("Monster"))
        {
            GameManager.Instance.attackController.applyDamageToMonster(collision, skill_damage, iciclePrefab, 2f);
        }
    }
    private void Update()
    {
        setSkillCoolDownLeft();
    }
    void setSkillCoolDownLeft()
    {
        // if it's in coolDown
        if (skill_coolDown_left > 0)
        {
            skill_coolDown_left -= Time.deltaTime;
            GameManager.Instance.UI.skillIconTransition(1, skill_coolDown_left, skill_coolDown);
        }
        // if the coolDown is over
        else if (skill_coolDown_left < 0)
        {
            skill_coolDown_left = 0;
        }
    }
}
