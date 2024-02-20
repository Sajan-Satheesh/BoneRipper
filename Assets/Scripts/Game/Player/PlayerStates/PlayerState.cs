using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PlayerState : StateGeneric<EnumPlayerStates, PlayerController>
{
    public PlayerState(PlayerController _controller, EnumPlayerStates _state) : base(_controller, _state) { }
}
