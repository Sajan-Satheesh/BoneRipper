using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoatPlayerState : PlayerState
{
    public InBoatPlayerState(PlayerController _controller) : base(_controller, EnumPlayerStates.IN_BOAT) { }

    public override void OnCollision()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {

        if (controller.playerModel.player.transform.parent != BoatService.instance.getBoatSeatTransform())
        {
            controller.playerModel.player.transform.parent = BoatService.instance.getBoatSeatTransform();
            PlayerService.instance.events.InvokeOnReachingBoat();
        }
        controller.SetAnimationState(EnumPlayerAnimationStates.restInBoat);
    }

    public override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void TickableFunction()
    {
        return;
    }
}
