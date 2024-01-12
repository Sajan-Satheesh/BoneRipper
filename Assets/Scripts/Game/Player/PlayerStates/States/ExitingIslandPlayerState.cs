using System;
using UnityEngine;

public class ExitingIslandPlayerState : PlayerState
{
	public ExitingIslandPlayerState(PlayerController _controller) : base(_controller, EnumPlayerStates.LEVEL_END) { }

    public override void OnCollision()
    {
        throw new NotImplementedException();
    }

    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        //throw new NotImplementedException();
    }

    protected override void TickableFunction()
    {
        Vector3 toMoveDir = (PlayerService.instance.requestPlayerExitPos() - controller.playerModel.player.transform.position).normalized;
        if (Vector3.Distance(controller.playerModel.player.transform.position, WorldService.instance.playerExit) < 0.3f)
        {
            controller.SetAnimationState(EnumPlayerAnimationStates.idle, true, 1f);
            controller.Orient(Vector3.forward, true);
            controller.Move(toMoveDir);
            if (controller.playerModel.player.transform.forward == Vector3.forward)
            {
                PlayerService.instance.ReachedExit();
            }
            return;
        }
        controller.Move(toMoveDir);
        controller.Orient(PlayerService.instance.requestPlayerExitPos());
    }
}
