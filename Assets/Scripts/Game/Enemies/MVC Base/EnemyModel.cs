using UnityEngine;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;

[Serializable]
public class EnemyModel
{
    public LayerMask destructibleLayers;
    public LayerMask directionHit;
    public bool isAttacking = false; 
    public List<Transform> bodyParts= new List<Transform>();
    public float health;
    public EnumWeapons currWeapon;
    public GameObject enemy;
    public Transform releaseHand;
    public EnumEnemyAnimationStates currAnimationState;

    public EnemyModel(EnemyData enemyDetails, EnemyView enemyView)
    {
        health = enemyDetails.enemyHealth;
        currWeapon = enemyDetails.defaultWeapon;
        enemy = enemyView.gameObject;
        releaseHand = enemy.transform;
        bodyParts = enemy.gameObject.GetComponent<EnemyView>().bodyPartPositions;
    }

    public virtual void DestroyModel()
    {
        if(enemy != null) Object.Destroy(enemy);
        if(releaseHand!= null) Object.Destroy(releaseHand.gameObject);
    }
    public List<Transform> GetBodyParts()
    {
        bodyParts = enemy.gameObject.GetComponent<EnemyView>().bodyPartPositions;
        return bodyParts;
    }

    ~EnemyModel()
    {
        Debug.Log("Destroyed Enemy Model");
    }

}
