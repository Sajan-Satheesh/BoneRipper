using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyModel
{
    public Vector3 mousePosInWorld;
    public LayerMask destructibleLayers;
    public LayerMask directionHit;
    public bool playable = false; 
    public Coroutine jumping;

    public float speed;
    public float health;
    public Weapons currWeapon;
    public EnemyState currState;
    public GameObject enemy;

    public EnemyModel(SO_Enemy enemyDetails, EnemyView enemyView)
    {
        speed = enemyDetails.enemySpeed;
        health = enemyDetails.enemyHealth;
        currWeapon = enemyDetails.defaultWeapon;
        currState = enemyDetails.defaultState;
        enemy = enemyView.gameObject;
    }

}
