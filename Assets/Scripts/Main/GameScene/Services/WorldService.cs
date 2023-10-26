using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    [SerializeField] GameObject module;
    [SerializeField] GameObject player;
    [SerializeField] bool builded = false;
    [SerializeField] int squareArea;
    // Start is called before the first frame update

    Coroutine buildWorld;
    void Start()
    {
        buildWorld = StartCoroutine(startLevelBuilding());
    }



    IEnumerator startLevelBuilding()
    {
        Vector3 buidingPos = player.transform.position + player.transform.forward * 10f;
        while (!builded)
        {
            areaBuilder(ref buidingPos);
            yield return null;
        }
        
    }

    void pathBuilder(ref Vector3 buidingPos)
    {
        GameObject buildingModule = Instantiate(module, buidingPos, Quaternion.identity);
        int randomX = UnityEngine.Random.Range(0, 2);
        int randomZ = randomX == 0 ? 1 : 0;
        Vector3 offset = new Vector3(randomX * module.GetComponent<MeshRenderer>().localBounds.size.z, 0f, randomZ * module.GetComponent<MeshRenderer>().localBounds.size.z);
        buidingPos = buildingModule.transform.position + offset;
    }

    void areaBuilder(ref Vector3 buidingPos)
    {
        Vector3 center = new Vector3(buidingPos.x, -0.2f, buidingPos.z);
        float moduleLength = module.GetComponent<MeshRenderer>().localBounds.size.z;
        for (int i=0; i< squareArea; i++)
        {
            Instantiate(module, center, Quaternion.identity);
            int remainingLength = UnityEngine.Random.Range( squareArea-2, squareArea);
            Vector3 leftExterme = center;
            Vector3 rightExterme = center;
            while (remainingLength > 0)
            {
                int randomDirection = UnityEngine.Random.Range(0, 2);
                GameObject buildingModule = randomDirection == 0 ? buildInDirection(direction.left, ref leftExterme, ref rightExterme) : buildInDirection(direction.right, ref leftExterme, ref rightExterme);

                --remainingLength;
            }
            center = new Vector3(center.x , center.y, center.z + moduleLength);
        }
        builded = true;
    }

    private GameObject buildInDirection(direction _direction, ref Vector3 leftXtreme, ref Vector3 rightXtreme)
    {
        float moduleLength = module.GetComponent<MeshRenderer>().localBounds.size.z;
        if (_direction == direction.left) 
        { 
            leftXtreme = new Vector3(leftXtreme.x - moduleLength, leftXtreme.y, leftXtreme.z);
            return Instantiate(module, leftXtreme, Quaternion.identity);
        }
        else 
        {
            rightXtreme = new Vector3(rightXtreme.x + moduleLength, leftXtreme.y, rightXtreme.z);
            return Instantiate(module, rightXtreme, Quaternion.identity);
        }
    }
    // Update is called once per frame
    
}

public enum direction { left, right};
