using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    public List<Vector3> jumpPath = new List<Vector3>();
    public bool isRunnable = false;
    public bool isJumpable = true;
    public float speed;
    public float jumpSpeed;
    public float health;
    public EnumWeapons currWeapon;
    public GameObject player;
    public GameObject playerArmature;
    public Transform playerWeaponSpot;
    public EnumPlayerAnimationStates currAnimationState = EnumPlayerAnimationStates.idle;
    public EnumPlayerStates afterJumpState;
    public PlayerStateMachine playerStateMachine;

    public PlayerModel(PlayerData playerDetails, PlayerView playerView, PlayerController playerController)
    {
        speed = playerDetails.playerSpeed;
        jumpSpeed = playerDetails.jumpSpeed;
        health = playerDetails.playerHealth;
        currWeapon = playerDetails.defaultWeapon;
        playerStateMachine = new PlayerStateMachine(playerController, playerDetails.defaultState);
        player = playerView.gameObject;
        playerArmature = player.transform.GetChild(0).gameObject;
        playerWeaponSpot = playerView.transform.GetChild(1).transform;
    }

    public void DestructModel()
    {
        player = null;
        playerArmature = null;
    }

}
