using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerEnemyState : StateGeneric<EnumTowerEnemyStates,TowerEnemyController> 
{
    public TowerEnemyState(TowerEnemyController _controller, EnumTowerEnemyStates _state) : base(_controller, _state)
    {
    }
}
