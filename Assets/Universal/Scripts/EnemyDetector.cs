using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    List<GameObject> enemiesInRange = new();
    public GameObject GetTheClosestEnemy()
    {
        GameObject theBestTarget = null;
        Vector3 currentPosition = transform.position;
        // Mathf.Infinity is used to set an initial infinite value, 
        // ensuring that any distance calculated will be less than this value in subsequent comparisons. 
        // This guarantees that, no matter how far the enemy is, the first enemy checked will be considered the closest.
        float squaredClosestDistance = Mathf.Infinity; //* stores the squared value of the closest distance
        foreach (GameObject closestEnemy in enemiesInRange)
        {
            Vector3 directionToTarget = closestEnemy.transform.position - currentPosition;
            // the actual distance(square value) between the enemy and the object
            // to compare 2 distances, we don't need a real value, a squared value will be effective
            float squaredDistanceToTarget = directionToTarget.sqrMagnitude;
            // if the squaredDistanceToTarget is less than squaredClosestDistance, update the best target
            if (squaredDistanceToTarget < squaredClosestDistance)
            {
                squaredClosestDistance = squaredDistanceToTarget;
                theBestTarget = closestEnemy;
            }
        }
        Debug.Log("the best target is:" + theBestTarget);
        return theBestTarget;
    }
    public List<GameObject> GetEnemiesInRange()
    {
        return enemiesInRange;
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        // if the detector collider reaches the enemy, add the enemy into the list
        if (collider.CompareTag("Monster"))
        {
            if (!enemiesInRange.Contains(collider.gameObject))
            {
                enemiesInRange.Add(collider.gameObject);
                Debug.Log("From EnemyDetector Script, the enemy is added");
                Debug.Log("From EnemyDetector Script, the enemies count is:" + enemiesInRange.Count);
                // TODO: How to continuous detect the next target?
                foreach (var item in enemiesInRange)
                {
                    Debug.Log("From EnemyDetector Script, the enemy is:  " + item.name);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        // if the attack is over remove all the enemy in the list
        if (collider.CompareTag("Monster"))
        {
            // if there is a enemy in the list
            if (enemiesInRange.Count > 0) enemiesInRange.Remove(collider.gameObject);
        }
    }
}
