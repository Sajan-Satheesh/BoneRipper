using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuningPlayerState : PlayerState
{
    public RuningPlayerState(PlayerController _controller) : base(_controller, EnumPlayerStates.RUNNING) { }

    public override void OnCollision()
    {
        return;
    }

    public override void OnStateEnter()
    {
        PlayerService.instance.setPlayerRoot(controller);
        if (controller.playerModel.currAnimationState != EnumPlayerAnimationStates.weaponBWwalk)
        {
            AnimatorTransitionInfo transInfo = controller.playerModel.playerArmature.GetComponent<Animator>().GetAnimatorTransitionInfo(0);
            Debug.Log("Trans info: " + transInfo.normalizedTime);
            controller.SetAnimationState(EnumPlayerAnimationStates.weaponBWwalk);
        }
    }

    public override void OnStateExit()
    {
        return;
    }

    protected override void TickableFunction()
    {
        
        controller.Move(controller.playerModel.player.transform.forward);
        controller.Orient(InputServices.instance.GetMousePosOnWorld());
    }
}
