using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LightningChainController : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.5f;
    [SerializeField] float delayBetweenEachChain = 0.5f;
    [SerializeField][Range(1, 10)] int maxEnemiesInChain = 8;
    [SerializeField] EnemyDetector enemyDetector;
    [SerializeField] Transform playerFirePoint;
    [SerializeField] GameObject lineRendererPrefab; // LightningChain prefab
    [SerializeField] int counter;

    public GameObject currentClosestEnemy;
    public List<GameObject> existingLineRenderer = new();
    public List<GameObject> enemiesInChain = new();

    //TODO: TEST
    public int skill_damage = 1;
    public float skill_coolDown = 1f;
    public float skill_coolDown_left = 0f;

    public void StartShooting()
    {
        // The first chain is from the player's point
        currentClosestEnemy = enemyDetector.GetTheClosestEnemy();
        // Create new line between enemy
        if (enemyDetector != null && playerFirePoint != null && lineRendererPrefab != null)
        {
            if (currentClosestEnemy != null)
            {
                CreateLineRenderer(playerFirePoint, currentClosestEnemy.transform, true); // Chain comes from player by default
                if (maxEnemiesInChain > 1)
                {
                    StartCoroutine(ChainReaction(currentClosestEnemy));
                }
            }
        }
    }
    private void CreateLineRenderer(Transform startPoint, Transform endPoint, bool fromPlayer = false)
    {
        GameObject lineRenderer = Instantiate(lineRendererPrefab);
        existingLineRenderer.Add(lineRenderer);
        // * Recursive function, The first chain is from
        StartCoroutine(UpdateLineRenderer(lineRenderer, startPoint, endPoint, fromPlayer));
    }
    //* This function will be called recursively
    private IEnumerator UpdateLineRenderer(GameObject lightingChainPrefab, Transform startPoint, Transform endPoint, bool fromPlayer = false)
    {
        //* Prefab structure
        // LightningChain
        //  Start (index=0)
        //  Line (index=1)
        //   HitImpact
        Transform lightningChainTransform = lightingChainPrefab.transform;
        Transform lineTransform = lightningChainTransform.GetChild(1).transform;
        Transform hitImpactPrefabTransform = lineTransform.GetChild(0).transform;

        lightingChainPrefab.GetComponent<LineRendererController>().CreateLine(startPoint, endPoint);

        Debug.Log("The fromPlayer Value is: " + fromPlayer);

        yield return new WaitForSeconds(refreshRate);
        // if the chian is from player
        if (fromPlayer)
        {
            // Get line renderer length and set the HitImpact position to the end of length
            float lineLength = Vector3.Distance(startPoint.position, endPoint.position);
            // Transfer the local position to the world position
            hitImpactPrefabTransform.localPosition = new Vector3(lineLength, 0, 0); // Values in the Inspector value
            // Execute the function again with new params
            StartCoroutine(UpdateLineRenderer(lineRendererPrefab, startPoint, currentClosestEnemy.transform, true));
            //todo: if the game object is not the same one
            if (currentClosestEnemy != enemyDetector.GetTheClosestEnemy())
            {
                StopShooting();
                StartShooting();
            }
        }
        // if the chain is not from player(EX: from monster)
        else
        {
            StartCoroutine(UpdateLineRenderer(lineRendererPrefab, startPoint, endPoint, false));
        }
    }
    private IEnumerator ChainReaction(GameObject closestEnemy)
    {
        yield return new WaitForSeconds(delayBetweenEachChain);
        // if the attacks hit the maximum enemies, cancel the cast
        if (counter == maxEnemiesInChain)
        {
            yield return null;
        }
        else
        {
            counter++;
            // Add enemy in the chain list
            enemiesInChain.Add(closestEnemy);
            // Get the next enemy with detector
            GameObject nextEnemy = closestEnemy.GetComponent<EnemyDetector>().GetTheClosestEnemy();
            // Create a line if the next enemy is not in the list
            if (!enemiesInChain.Contains(nextEnemy))
            {
                CreateLineRenderer(closestEnemy.transform, nextEnemy.transform, false);
                StartCoroutine(ChainReaction(nextEnemy));
            }
        }
    }

    //todo: test
    public void StopShooting()
    {
        // Reset
        counter = 1;
        //** Use timer to trigger this function??
        for (int i = 0; i < existingLineRenderer.Count; i++)
        {
            Destroy(existingLineRenderer[i]);
        }
        existingLineRenderer.Clear();
        enemiesInChain.Clear();
    }
}
