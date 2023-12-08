using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    internal List<Vector3> jumpPath = new List<Vector3>();
    public Vector3 mousePosInWorld;
    public LayerMask destructibleLayers;
    public LayerMask directionHit;
    public bool isRunnable = false;
    public bool isJumpable = true;
    public float speed;
    public float jumpSpeed;
    public float health;
    public Weapons currWeapon;
    public PlayerState currState;
    public GameObject player;
    public GameObject playerArmature;
    public Transform playerWeaponSpot;
    public PlayerAnimationStates currAnimationState = PlayerAnimationStates.idle;
    public PlayerState afterJumpState;

    public PlayerModel(SO_Player playerDetails, PlayerView playerView)
    {
        speed = playerDetails.playerSpeed;
        jumpSpeed = playerDetails.jumpSpeed;
        health = playerDetails.playerHealth;
        currWeapon = playerDetails.defaultWeapon;
        currState = playerDetails.defaultState;
        player = playerView.gameObject;
        playerArmature = player.transform.GetChild(0).gameObject;
        playerWeaponSpot = playerView.transform.GetChild(1).transform;
        directionHit = playerDetails.pointerDetetionLayer;
    }

    public void destructModel()
    {
        player = null;
        playerArmature = null;
    }

}
