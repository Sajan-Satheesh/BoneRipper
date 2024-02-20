using System;
using UnityEngine;

[Serializable]
public class GroundEnemyModel:EnemyModel
{
    public float speed;
    public Vector3 spawnPostion;
    //public EnumGroundEnemyStates currState;
    public GroundEnemyStateMachine groundEnemyStateMachine;
    

    public GroundEnemyModel(GroundEnemyData enemyDetails, EnemyView enemyView, Vector3 spawnPostion, GroundEnemyController groundEnemyController) : base(enemyDetails, enemyView)
    {
        speed = enemyDetails.enemySpeed;
        groundEnemyStateMachine = new GroundEnemyStateMachine(groundEnemyController,enemyDetails.defaultState);
        this.spawnPostion = spawnPostion;
    }

}
