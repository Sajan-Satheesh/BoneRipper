using UnityEngine;
using System;
using Object = UnityEngine.Object;

[Serializable]
public class EnemyModel
{
    public LayerMask destructibleLayers;
    public LayerMask directionHit;
    public bool isAttacking = false; 
    
    public float health;
    public Weapons currWeapon;
    public GameObject enemy;
    public Transform releaseHand;

    public EnemyModel(SO_Enemy enemyDetails, EnemyView enemyView)
    {
        
        health = enemyDetails.enemyHealth;
        currWeapon = enemyDetails.defaultWeapon;
        enemy = enemyView.gameObject;
        releaseHand = enemy.transform;
    }

    virtual public void destroyModel()
    {
        if(enemy != null) Object.Destroy(enemy);
        if(releaseHand!= null) Object.Destroy(releaseHand.gameObject);
    }

    ~EnemyModel()
    {
        Debug.Log("Destroyed Enemy Model");
    }

}
