
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HideOutGenerator 
{
    //[SerializeField] private int hideOutCounts;
    //[SerializeField] private GameObject hideOut;
    List<GameObject> hideOuts = new List<GameObject>();
    List<Vector3> hideOutRoofs = new List<Vector3>();


    public IEnumerator createEnemyHideouts(GameObject land, int hideOutCounts, List<GameObject> hideOutModels)
    {
        Debug.Log("creating enemy HideOuts");
        
        while (hideOuts.Count < hideOutCounts)
        {
            RaycastHit hideOutHit;
            Vector3 randomDirection = generateRandomDirection(out randomDirection);
            Vector3 rayOrigin = land.transform.position + Vector3.up*0.3f;
            
            if (Physics.Raycast(rayOrigin, randomDirection, out hideOutHit, Mathf.Infinity, LayerMask.GetMask("Land")))
            {
                int randomHideOutIndex = Random.Range(0, hideOutModels.Count);
                Debug.DrawLine(rayOrigin, rayOrigin + randomDirection * 20, Color.green, 10f);
                if (possibleSpwan(hideOuts, hideOutHit.point, hideOutModels[randomHideOutIndex].GetComponent<MeshRenderer>().bounds.size.y))
                {
                    GameObject hideO = Object.Instantiate(hideOutModels[randomHideOutIndex], hideOutHit.point, Quaternion.identity, land.transform);
                    hideO.transform.right = getPositionInXZplane(rayOrigin,hideOutHit.point.y) - hideOutHit.point;
                    hideOuts.Add(hideO);
                }
            }
            else Debug.DrawLine(rayOrigin, rayOrigin + randomDirection * 100, Color.red, 1f);

            yield return null;
        }
        WorldService.instance.getAllRoofPos();
    }

    public List<Vector3> getHideOutRoofs()
    {
        if (hideOuts.Count == 0) return null;

        hideOutRoofs.Clear();
        foreach (var hideOut in hideOuts)
        {
            hideOutRoofs.Add(hideOut.transform.position + Vector3.up * 3f);
        }
        return hideOutRoofs;

    }

    private Vector3 getPositionInXZplane(Vector3 position, float yPos)
    {
        return new Vector3(position.x, yPos, position.z);
    }

    private bool possibleSpwan(List<GameObject> hideOuts, Vector3 spawnPosition , float minSize)
    {
        for (int i = 0; i < hideOuts.Count; i++)
        {
            float distanceBetweenHideOuts = Vector3.Distance(hideOuts[i].transform.position, spawnPosition);
            float distancefromEntry = Vector3.Distance(WorldService.instance.playerEntry, spawnPosition);
            float distancefromExit = Vector3.Distance(WorldService.instance.playerExit, spawnPosition);

            if (distanceBetweenHideOuts < minSize || distancefromEntry < minSize || distancefromExit < minSize)
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 generateRandomDirection(out Vector3 randomDirection)
    {
        float randomY = UnityEngine.Random.Range(0f, -0.1f);
        float randomAngle = UnityEngine.Random.Range(0f,360f);
        
        float randomX = Mathf.Cos(randomAngle);
        float randomZ = Mathf.Sin(randomAngle);

        randomDirection = new Vector3(randomX,randomY,randomZ);
        return randomDirection;
    }

}