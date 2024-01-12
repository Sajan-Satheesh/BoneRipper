
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
    [field:SerializeField] public PlayerModel playerModel { get; set; }
    PlayerView playerView;

    public PlayerController(PlayerData playerDetails, Vector3 spwanPosition)
    {
        playerView = Object.Instantiate<PlayerView>(playerDetails.playerView,spwanPosition,Quaternion.identity, BoatService.instance.getBoatSeatTransform());
        playerView.getPlayerController(this);
        playerModel = new PlayerModel(playerDetails, playerView, this);
        playerModel.playerStateMachine.SetState(EnumPlayerStates.IN_BOAT);
        SetState(EnumPlayerStates.IN_BOAT);
        PlayerService.instance.events.Subscribe_OnInitialtingJump(ReactJumpToTarget);
        InitializeDefaultPlayer();
    }
    public void InitializeDefaultPlayer()
    {
        playerModel.isRunnable = false;
        playerModel.isJumpable = true;
        playerModel.currWeapon = EnumWeapons.BONE_WRACKER;
        SetState(EnumPlayerStates.IN_BOAT);
        playerModel.currAnimationState = EnumPlayerAnimationStates.restInBoat;
        playerModel.afterJumpState = EnumPlayerStates.RUNNING;
        SetNonKinematicPlayer();
    }

    #region getters
    private Vector3 GetPosOnHeight(Vector3 pos, float height)
    {
        return new Vector3(pos.x, height, pos.z);
    }
    public Vector3 GetCurrentLocInWorld()
    {
        if (playerModel.player != null)
            return playerModel.player.transform.position;
        else return default;
    }

    public float GetCurrentSpeed()
    {
        if (playerModel != null)
            return playerModel.speed;
        else return 0;
    }


    public Vector3 GetForwardDirection()
    {
        if (playerModel != null)
            return playerModel.player.transform.forward;
        else return default;
    }
    #endregion

    #region setters
    public void SetNonKinematicPlayer()
    {
        playerModel.player.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void SetkinematicPlayer()
    {
        playerModel.player.GetComponent<Rigidbody>().isKinematic = true;
    }
    public Transform SetPlayerRoot(Transform root)
    {
        return playerModel.player.gameObject.transform.parent = root;
    }

    public void SetAnimationState(
    EnumPlayerAnimationStates state,
    bool blend = false,
    float transitionTime = 1f)
    {
        if (playerModel.currAnimationState == state) return;

        playerModel.currAnimationState = state;

        if (!blend)
            playerModel.playerArmature.GetComponent<Animator>().Play(playerModel.currAnimationState.ToString());
        else playerModel.playerArmature.GetComponent<Animator>().CrossFade(playerModel.currAnimationState.ToString(), transitionTime);

    }

    public void SetState(EnumPlayerStates newPlayerState)
    {
        if (playerModel.playerStateMachine.currentState.state == newPlayerState) return;
        playerModel.playerStateMachine.SetState(newPlayerState);
    }

    #endregion

    public void OnUpdate()
    {
        playerModel?.playerStateMachine?.currentState?.OnTick();
    }


    #region State Connections

    #region Jumping
    private void ReactJumpToTarget(List<Vector3> jumpPath)
    {
        playerModel.isJumpable = true;
        playerModel.jumpPath = jumpPath;
        playerModel.afterJumpState = (jumpPath[0].y < jumpPath[jumpPath.Count - 1].y) ? EnumPlayerStates.RUNNING : EnumPlayerStates.IN_BOAT;
        SetkinematicPlayer();
        SetState(EnumPlayerStates.JUMPING);

    }
    #endregion

    #region Running

    public void Move(Vector3 moveDirection)
    {
        playerModel.player.transform.Translate(moveDirection * playerModel.speed * Time.deltaTime, Space.World);
    }

    public void Orient(Vector3 target, bool toLook = false)
    {
        Vector3 playerPosInZeroPlane = GetPosOnHeight(playerModel.player.transform.position, 0f);
        Vector3 toLookForwardVector = GetPosOnHeight(target, 0f);
        if (!toLook)
        {
            toLookForwardVector -= playerPosInZeroPlane;
        }
        Quaternion toLookDirection = Quaternion.LookRotation(toLookForwardVector, Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(playerModel.player.transform.rotation, toLookDirection, 0.1f);
        playerModel.player.transform.rotation = finalRotation;
    }

    #endregion

    #endregion

    public void RequestGameOver()
    {
        PlayerService.instance.gameOver();
    }

    public Transform GetDefaultWeaponSpot()
    {
        return playerModel.playerWeaponSpot;
    }

    public void DestuctPlayer()
    {
        Object.Destroy(playerView.gameObject);
        playerView = null;
        playerModel.DestructModel();
        playerModel = null;

        PlayerService.instance.events.UnSubscribe_OnInitialtingJump(ReactJumpToTarget);
    }

    public PlayerShopSlots GetPlayerSlot()
    {
        return playerModel.player.GetComponent<PlayerShopSlots>();
    }
}
