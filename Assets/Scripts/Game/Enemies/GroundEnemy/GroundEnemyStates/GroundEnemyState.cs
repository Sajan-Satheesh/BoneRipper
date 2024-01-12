using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundEnemyState : StateGeneric<EnumGroundEnemyStates, GroundEnemyController>
{
    public GroundEnemyState(GroundEnemyController _controller, EnumGroundEnemyStates _state) : base(_controller, _state) { }

}
