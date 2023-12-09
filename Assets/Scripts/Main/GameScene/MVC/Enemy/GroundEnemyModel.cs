using System;
using UnityEngine;

[Serializable]
public class GroundEnemyModel:EnemyModel
{
    public float speed;
    public Vector3 spawnPostion;
    public GroundEnemyState currState;

    public GroundEnemyModel(SO_GroundEnemy enemyDetails, EnemyView enemyView, Vector3 spawnPostion) : base(enemyDetails, enemyView)
    {
        speed = enemyDetails.enemySpeed;
        currState = enemyDetails.defaultState;
        this.spawnPostion = spawnPostion;
    }

}
