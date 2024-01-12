
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HideOutGenerator 
{
    //[SerializeField] private int hideOutCounts;
    //[SerializeField] private GameObject hideOut;
    List<HideOutView> hideOuts = new List<HideOutView>();
    

    public void removeAllHideOuts()
    {
        foreach (HideOutView item in hideOuts)
        {
            Object.Destroy(item.gameObject);
        }
        hideOuts.Clear();
    }
    public IEnumerator createEnemyHideouts(GameObject land, int hideOutCounts, List<HideOutView> hideOutModels)
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
                    HideOutView hideO = Object.Instantiate(hideOutModels[randomHideOutIndex], hideOutHit.point, Quaternion.identity, land.transform);
                    hideO.transform.right = getPositionInXZplane(rayOrigin,hideOutHit.point.y) - hideOutHit.point;
                    hideOuts.Add(hideO);
                }
            }
            else Debug.DrawLine(rayOrigin, rayOrigin + randomDirection * 100, Color.red, 1f);

            yield return null;
        }
        WorldService.instance.getAllRoofPos();
    }

    public IEnumerator createInCircularPat(GameObject land, int hideOutCounts, List<HideOutView> hideOutModels, float minRadius, float maxRadius)
    {
        Debug.Log("creating enemy HideOuts");

        while (hideOuts.Count < hideOutCounts)
        {
            Vector3 randomDirection = generateRandomDirection(out randomDirection);
            Vector3 randomPosition = Random.Range(minRadius, maxRadius) * randomDirection + land.transform.position;
            int randomHideOutIndex = Random.Range(0, hideOutModels.Count);
            if (possibleSpwan(hideOuts, randomPosition, hideOutModels[randomHideOutIndex].GetComponent<MeshRenderer>().bounds.size.y))
            {
                HideOutView hideO = Object.Instantiate(hideOutModels[randomHideOutIndex], randomPosition, Quaternion.identity, land.transform);
                hideO.transform.right = land.transform.position - randomPosition;
                hideOuts.Add(hideO);
            }
            yield return null;
        }
        WorldService.instance.getAllRoofPos();
    }

    public List<Vector3> getHideOutRoofs()
    {
        if (hideOuts.Count == 0) return null;
        List<Vector3> hideOutRoofs = new List<Vector3>();
        hideOutRoofs.Clear();
        foreach (var hideOut in hideOuts)
        {
            hideOutRoofs.Add(hideOut.TopEnemySpwanLoc);
        }
        return hideOutRoofs;
    }

    public List<Transform> getHideOutTransform()
    {
        if (hideOuts.Count == 0) return null;
        List<Transform> hideOutPos = new List<Transform>();
        hideOutPos.Clear();
        foreach (var hideOut in hideOuts)
        {
            hideOutPos.Add(hideOut.transform);
        }
        return hideOutPos;
    }

    private Vector3 getPositionInXZplane(Vector3 position, float yPos)
    {
        return new Vector3(position.x, yPos, position.z);
    }

    private bool possibleSpwan(List<HideOutView> hideOuts, Vector3 spawnPosition , float minSize)
    {
        for (int i = 0; i < hideOuts.Count; i++)
        {
            float distanceBetweenHideOuts = Vector3.Distance(hideOuts[i].transform.position, spawnPosition);
            float distancefromEntry = Vector3.Distance(WorldService.instance.playerEntry, spawnPosition);
            float distancefromExit = Vector3.Distance(WorldService.instance.playerExit, spawnPosition);

            if (distanceBetweenHideOuts < minSize || distancefromEntry < minSize || distancefromExit <= minSize)
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 generateRandomDirection(out Vector3 randomDirection)
    {
        float randomY = 0f;
        float randomAngle = UnityEngine.Random.Range(0f,360f);
        
        float randomX = Mathf.Cos(randomAngle);
        float randomZ = Mathf.Sin(randomAngle);

        randomDirection = new Vector3(randomX,randomY,randomZ);
        return randomDirection;
    }

}