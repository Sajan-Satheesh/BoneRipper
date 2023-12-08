
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
    [field:SerializeField] PlayerModel playerModel { get; set; }
    PlayerView playerView;

    public PlayerController(SO_Player playerDetails, Vector3 spwanPosition)
    {
        playerView = Object.Instantiate<PlayerView>(playerDetails.playerView,spwanPosition,Quaternion.identity, BoatService.instance.getBoatSeatTransform());
        playerView.getPlayerController(this);
        playerModel = new PlayerModel(playerDetails, playerView);
        setState(PlayerState.inBoat);
        PlayerService.instance.onInitialtingJump += reactJumpToTarget;
        initializeDefaultPlayer();
    }
    public void initializeDefaultPlayer()
    {
        playerModel.isRunnable = false;
        playerModel.isJumpable = true;
        playerModel.currWeapon = Weapons.boneBreaker;
        playerModel.currState = PlayerState.inBoat;
        playerModel.currAnimationState = PlayerAnimationStates.restInBoat;
        playerModel.afterJumpState = PlayerState.running;
        setNonKinematicPlayer();
    }

    #region getters
    private Vector3 getPosOnHeight(Vector3 pos, float y)
    {
        return new Vector3(pos.x, y, pos.z);
    }
    public Vector3 getCurrentLocInWorld()
    {
        if (playerModel != null)
            return playerModel.player.transform.position;
        else return default;
    }

    public float getCurrentSpeed()
    {
        if (playerModel != null)
            return playerModel.speed;
        else return 0;
    }


    public Vector3 getForwardDirection()
    {
        if (playerModel != null)
            return playerModel.player.transform.forward;
        else return default;
    }
    #endregion

    #region setters
    public void setNonKinematicPlayer()
    {
        playerModel.player.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void setkinematicPlayer()
    {
        playerModel.player.GetComponent<Rigidbody>().isKinematic = true;
    }
    public Transform setPlayerRoot(Transform root)
    {
        return playerModel.player.gameObject.transform.parent = root;
    }

    private void setAnimationState(
    PlayerAnimationStates state,
    bool blend = false,
    float transitionTime = 1f)
    {
        if (playerModel.currAnimationState == state) return;

        playerModel.currAnimationState = state;

        if (!blend)
            playerModel.playerArmature.GetComponent<Animator>().Play(state.ToString());
        else playerModel.playerArmature.GetComponent<Animator>().CrossFade(state.ToString(), transitionTime);

    }

    public void setState(PlayerState playerState)
    {
        if (playerModel.currState == playerState) return;
        prepareState(playerState);
        playerModel.currState = playerState;
    }
    #endregion

    public void onUpdate()
    {
        updateStateMachine();
    }

    private void prepareState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.load:
                break;
            case PlayerState.running:
                break;
            case PlayerState.dead:
                break;
            case PlayerState.jumping:
                prepareJumping();
                break;
            case PlayerState.inBoat:
                break;
            case PlayerState.levelEnd:
                break;
            default:
                break;
        }
    }
    private void updateStateMachine()
    {
        switch (playerModel.currState)
        {
            case PlayerState.load:
                break;
            case PlayerState.running:
                updateRunning();
                break;
            case PlayerState.dead:
                break;
            case PlayerState.jumping:
                updateJumping();
                break;
            case PlayerState.inBoat:
                inBoat();
                break;
            case PlayerState.levelEnd:
                updateRunningToExit();
                break;
            default:
                break;
        }
    }

    #region State Functions

    #region move To LevelExit State
    private void updateRunningToExit()
    {
        moveToLevelExit();
    }
    private void moveToLevelExit()
    {
        if (Vector3.Distance(playerModel.player.transform.position, WorldService.instance.playerExit) < 0.3f)
        {
            setAnimationState(PlayerAnimationStates.idle, true, 1f);
            orient(Vector3.forward, true);
            if (playerModel.player.transform.forward == Vector3.forward)
            {
                PlayerService.instance.ReachedExit();
            }
            return;
        }
        Vector3 toMoveDir = (PlayerService.instance.requestPlayerExitPos() - playerModel.player.transform.position).normalized;
        move(toMoveDir);
        orient(PlayerService.instance.requestPlayerExitPos());
    }
    #endregion

    #region jumping State
    private void reactJumpToTarget(List<Vector3> jumpPath)
    {
        playerModel.isJumpable = true;
        playerModel.jumpPath = jumpPath;
        playerModel.afterJumpState = (jumpPath[0].y < jumpPath[jumpPath.Count - 1].y) ? PlayerState.running : PlayerState.inBoat;
        setkinematicPlayer();
        setState(PlayerState.jumping);

    }

    private void prepareJumping()
    {
        PlayerService.instance.setPlayerRoot(this);
        if (playerModel.isJumpable)
        {
            setAnimationState(PlayerAnimationStates.jumpStart);
            playerModel.isJumpable = false;
        }
    }
    private void updateJumping()
    {
        jumpingMovement();
    }

    private void jumpingMovement()
    {
        playerModel.player.transform.Translate((playerModel.jumpPath[1] - playerModel.jumpPath[0]) * playerModel.jumpSpeed * Time.deltaTime);
        float prevToPlayerDist = Vector3.Distance(playerModel.player.transform.position, playerModel.jumpPath[0]);
        float preToTargetDist = Vector3.Distance(playerModel.jumpPath[1], playerModel.jumpPath[0]);
        if (playerModel.jumpPath.Count > 3 && playerModel.jumpPath[1].y > playerModel.jumpPath[0].y && playerModel.jumpPath[1].y > playerModel.jumpPath[2].y)
            setAnimationState(PlayerAnimationStates.aboutToLand, true, 3f);
        if (playerModel.jumpPath.Count == 7)
            setAnimationState(PlayerAnimationStates.landing);
        if (prevToPlayerDist >= preToTargetDist)
        {
            playerModel.jumpPath.RemoveAt(0);
        }
        if (playerModel.jumpPath.Count == 1)
        {
            playerModel.player.transform.position = playerModel.jumpPath[0];
            if (playerModel.afterJumpState == PlayerState.running)
                PlayerService.instance.reachedLand();

            setState(playerModel.afterJumpState);
        }

    }
    #endregion

    #region in Boat State
    private void inBoat()
    {
        if (playerModel.player.transform.parent != BoatService.instance.getBoatSeatTransform())
        {
            playerModel.player.transform.parent = BoatService.instance.getBoatSeatTransform();
            PlayerService.instance.onReachingBoat();
        }
        setAnimationState(PlayerAnimationStates.restInBoat);
    }
    #endregion

    #region running State
    private void updateRunning()
    {
        PlayerService.instance.setPlayerRoot(this);
        if (playerModel.currAnimationState != PlayerAnimationStates.weaponBWwalk)
        {
            AnimatorTransitionInfo transInfo = playerModel.playerArmature.GetComponent<Animator>().GetAnimatorTransitionInfo(0);
            Debug.Log("Trans info: " + transInfo.normalizedTime);
            //if (transInfo.normalizedTime < 0.9f ) return;

            setAnimationState(PlayerAnimationStates.weaponBWwalk);
        }
        move(playerModel.player.transform.forward);
        orient(getUpdateMousePosInWorld_ZeroPlane());
    }

    void move(Vector3 moveDirection)
    {
        playerModel.player.transform.Translate(moveDirection * playerModel.speed * Time.deltaTime, Space.World);
    }

    void orient(Vector3 target, bool toLook = false)
    {
        Vector3 playerPosInZeroPlane = getPosOnHeight(playerModel.player.transform.position, 0f);
        Vector3 toLookForwardVector = getPosOnHeight(target, 0f);
        if (!toLook)
        {
            toLookForwardVector -= playerPosInZeroPlane;
        }
        Quaternion toLookDirection = Quaternion.LookRotation(toLookForwardVector, Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(playerModel.player.transform.rotation, toLookDirection, 0.1f);
        playerModel.player.transform.rotation = finalRotation;
    }

    Vector3 getUpdateMousePosInWorld_ZeroPlane()
    {
        Ray CamToWorld = Camera.main.ScreenPointToRay(InputController.instance.getMousePostion());
        RaycastHit mouseHitOnWorld;
        Physics.Raycast(CamToWorld, out mouseHitOnWorld, float.MaxValue, playerModel.directionHit);
        playerModel.mousePosInWorld = getPosOnHeight(mouseHitOnWorld.point, 0f);
        Debug.DrawRay(Camera.main.transform.position, playerModel.mousePosInWorld - Camera.main.transform.position, Color.red);
        return playerModel.mousePosInWorld;
    }
    #endregion

    #endregion

    public void requestGameOver()
    {
        PlayerService.instance.gameOver();
    }

    public Transform getDefaultWeaponSpot()
    {
        return playerModel.playerWeaponSpot;
    }

    public void destuctPlayer()
    {
        Object.Destroy(playerView.gameObject);
        playerView = null;
        playerModel.destructModel();
        playerModel = null;

        PlayerService.instance.onInitialtingJump -= reactJumpToTarget;
    }

}
