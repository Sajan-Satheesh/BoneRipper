using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayerState : PlayerState
{
    public JumpingPlayerState(PlayerController _controller) : base(_controller, EnumPlayerStates.JUMPING) { }

    public override void OnCollision()
    {
        
    }

    public override void OnStateEnter()
    {
        PlayerService.instance.setPlayerRoot(controller);
        if (controller.playerModel.isJumpable)
        {
            controller.SetAnimationState(EnumPlayerAnimationStates.jumpStart);
            controller.playerModel.isJumpable = false;
        }
    }

    public override void OnStateExit()
    {
        controller.playerModel.player.transform.position = controller.playerModel.jumpPath[0];
        if (controller.playerModel.afterJumpState == EnumPlayerStates.RUNNING)
            PlayerService.instance.reachedLand();
        else
        {
            PlayerService.instance.events.InvokeOnReachingBoat();
        }
        

    }

    protected override void TickableFunction()
    {
        controller.playerModel.player.transform.Translate((controller.playerModel.jumpPath[1] - controller.playerModel.jumpPath[0]) * controller.playerModel.jumpSpeed * Time.deltaTime);
        float prevToPlayerDist = Vector3.Distance(controller.playerModel.player.transform.position, controller.playerModel.jumpPath[0]);
        float preToTargetDist = Vector3.Distance(controller.playerModel.jumpPath[1], controller.playerModel.jumpPath[0]);
        if (controller.playerModel.jumpPath.Count > 3 && controller.playerModel.jumpPath[1].y > controller.playerModel.jumpPath[0].y && controller.playerModel.jumpPath[1].y > controller.playerModel.jumpPath[2].y)
            controller.SetAnimationState(EnumPlayerAnimationStates.aboutToLand, true, 3f);
        if (controller.playerModel.jumpPath.Count == 7)
            controller.SetAnimationState(EnumPlayerAnimationStates.landing);
        if (prevToPlayerDist >= preToTargetDist)
        {
            controller.playerModel.jumpPath.RemoveAt(0);
        }
        if (controller.playerModel.jumpPath.Count == 1)
        {
            controller.SetState(controller.playerModel.afterJumpState);
        }
    }
}
