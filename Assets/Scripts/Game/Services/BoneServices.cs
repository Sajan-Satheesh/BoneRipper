using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoneServices : GenericSingleton<BoneServices>
{
    [SerializeField] private BoneView boneView;
    [SerializeField] private float boneImpulseSpeed;
    private List<BoneView> allSpawnedBones = new List<BoneView>();
    BonePool bonePool;

    private int boneCheckIndex = 0;
    override protected void Awake()
    {
        base.Awake();
        bonePool = new BonePool(boneView);
    }

    private void Update()
    {
        if(allSpawnedBones.Count > 0)
        {
            boneIndexIncrementer();
            if (allSpawnedBones[boneCheckIndex].transform.position.y < requestLandPostion().y)
            {
                bonePool.returnItem(allSpawnedBones[boneCheckIndex]);
                allSpawnedBones.RemoveAt(boneCheckIndex);
            }

        }
    }

    public void spawnBones(List<Transform> spawnLocations)
    {
        spawnLocations.ForEach(spawnBone);
    }

    private void spawnBone(Transform spawnLoc)
    {
        allSpawnedBones.Add(bonePool.getItem(spawnLoc));
        allSpawnedBones[allSpawnedBones.Count - 1].gameObject.GetComponent<Rigidbody>().AddForce(requestPlayerDirection() * boneImpulseSpeed, ForceMode.Impulse);
    }

    private Vector3 requestPlayerDirection()
    {
        return (PlayerService.instance.getPlayerDirection() + Vector3.up).normalized;
    }
    private void boneIndexIncrementer()
    {
        ++boneCheckIndex;
        if(boneCheckIndex >= allSpawnedBones.Count) boneCheckIndex= 0;
    }

    private Vector3 requestLandPostion()
    {
        return WorldService.instance.getLandPosition();
    }
}
