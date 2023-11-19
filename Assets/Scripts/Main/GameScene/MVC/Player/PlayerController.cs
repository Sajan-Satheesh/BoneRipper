
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController 
{
    PlayerModel playerModel { get; set; }
    PlayerView playerView;

    public PlayerController(SO_Player playerDetails, Vector3 spwanPosition)
    {
        playerView = Object.Instantiate<PlayerView>(playerDetails.playerView,spwanPosition,Quaternion.identity, BoatService.instance.getBoatSeatTransform());
        playerView.getPlayerController(this);
        playerModel = new PlayerModel(playerDetails, playerView);
        setState(PlayerState.inBoat);
        PlayerService.instance.jumpPressed += jumpToTarget; 
    }

    internal Transform setPlayerRoot(Transform root)
    {
        return playerModel.player.gameObject.transform.parent = root;
    }

    internal void onUpdate()
    {
        runStateMachine();
    }

    private void setState(PlayerState playerState)
    {
        playerModel.currState = playerState;
    }

    private void setAnimationState(PlayerAnimationStates state , bool blend = false, float transitionTime = 5f)
    {
        if (playerModel.currAnimationState == state) return;

        playerModel.currAnimationState = state;
        if(!blend)
            playerModel.playerArmature.GetComponent<Animator>().Play(state.ToString());
        else playerModel.playerArmature.GetComponent<Animator>().CrossFade(state.ToString(),transitionTime);
    }
    private void runStateMachine()
    {
        switch (playerModel.currState)
        {
            case PlayerState.load:
                break;
            case PlayerState.running:
                run();
                break;
            case PlayerState.dead:
                break;
            case PlayerState.jumping:
                jump();
                break;
            case PlayerState.inBoat:
                inBoat();
                break;
            case PlayerState.acting:
                break;
            default:
                break;
        }
    }

    #region jumpingState
    private void jumpToTarget(List<Vector3> jumpPath)
    {
        playerModel.jumpPath = jumpPath;
        setState(PlayerState.jumping);

    }
    private void jump()
    {
        PlayerService.instance.setPlayerRoot(this);
        if (playerModel.isJumpable)
        {
            setAnimationState(PlayerAnimationStates.jumpStart);
            playerModel.isJumpable = false;
        }
            
        //playerView.beginCoroutine(playerModel.jumping, jumpingMotion);
        jumpingMovement();
    }


    private IEnumerator jumpingMotion()
    {
        float defaultSpeed = Time.deltaTime;
        float speed = defaultSpeed;
        while (playerModel.jumpPath.Count != 1)
        {
            //speed += defaultSpeed;
            playerModel.player.transform.Translate((playerModel.jumpPath[1] - playerModel.player.transform.position) * speed);
            //playerModel.player.transform.position = Vector3.Lerp(playerModel.jumpPath[0], playerModel.jumpPath[1], speed);
            float prevToPlayerDist = Vector3.Distance(playerModel.player.transform.position, playerModel.jumpPath[0]);
            float preToTargetDist = Vector3.Distance(playerModel.jumpPath[1], playerModel.jumpPath[0]);
            if (prevToPlayerDist >= preToTargetDist)
            {
                playerModel.jumpPath.RemoveAt(0);
            }
            yield return null;
        }
        setState(PlayerState.running);
    }

    private void jumpingMovement()
    {
        playerModel.player.transform.Translate((playerModel.jumpPath[1] - playerModel.jumpPath[0]) * playerModel.jumpSpeed * Time.deltaTime);
        float prevToPlayerDist = Vector3.Distance(playerModel.player.transform.position, playerModel.jumpPath[0]);
        float preToTargetDist = Vector3.Distance(playerModel.jumpPath[1], playerModel.jumpPath[0]);
        if (playerModel.jumpPath.Count > 3 && playerModel.jumpPath[1].y > playerModel.jumpPath[0].y && playerModel.jumpPath[1].y > playerModel.jumpPath[2].y)
            setAnimationState(PlayerAnimationStates.aboutToLand,true,10f);
        if(playerModel.jumpPath.Count == 7)
            setAnimationState(PlayerAnimationStates.landing, true, 5f);
        if (prevToPlayerDist >= preToTargetDist)
        {
            playerModel.jumpPath.RemoveAt(0);
        }
        if (playerModel.jumpPath.Count == 1)
        {
            setState(PlayerState.running);
        }
            
    }
    #endregion

    #region inBoatState
    private void inBoat()
    {
        setAnimationState(PlayerAnimationStates.restInBoat);
    }
    #endregion

    #region runState
    private void run()
    {
        PlayerService.instance.setPlayerRoot(this);
        setAnimationState(PlayerAnimationStates.weaponBWwalk, true, 1f);
        move();
        orient();
    }

    void move()
    {
        playerModel.player.transform.Translate(playerModel.player.transform.forward * playerModel.speed * Time.deltaTime, Space.World);
    }

    void orient()
    {
        Vector3 playerPosInZeroPlane = new Vector3(playerModel.player.transform.position.x, 0f, playerModel.player.transform.position.z);
        Vector3 toLookForwardVector = getUpdateMousePosInWorld_ZeroPlane() - playerPosInZeroPlane;
        Quaternion toLookDirection = Quaternion.LookRotation(toLookForwardVector, Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(playerModel.player.transform.rotation, toLookDirection, 0.1f);
        playerModel.player.transform.rotation = finalRotation;
    }

    Vector3 getUpdateMousePosInWorld_ZeroPlane()
    {
        Ray CamToWorld = Camera.main.ScreenPointToRay(InputController.instance.getMousePostion());
        RaycastHit mouseHitOnWorld;
        Physics.Raycast(CamToWorld, out mouseHitOnWorld, float.MaxValue, playerModel.directionHit);
        playerModel.mousePosInWorld = new Vector3(mouseHitOnWorld.point.x, 0f, mouseHitOnWorld.point.z);
        Debug.DrawRay(Camera.main.transform.position, playerModel.mousePosInWorld - Camera.main.transform.position, Color.red);
        return playerModel.mousePosInWorld;
    } 
    #endregion

    internal Vector3 getCurrentLocInWorld()
    {
        if (playerModel != null)
            return playerModel.player.transform.position;
        else return default;
    }

    

    public void destuctPlayer() 
    {
        Object.Destroy(playerView);
        playerView = null;
        playerModel.destructModel();
        playerModel = null;
        
        PlayerService.instance.jumpPressed -= jumpToTarget;
    }

    internal PlayerState getPlayerState()
    {
        return playerModel.currState;
    }

    internal PlayerAnimationStates getAnimState()
    {
        return playerModel.currAnimationState;
    }
}
