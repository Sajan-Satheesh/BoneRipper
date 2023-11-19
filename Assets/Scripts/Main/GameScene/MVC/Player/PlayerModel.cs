using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    internal List<Vector3> jumpPath = new List<Vector3>();
    public Vector3 mousePosInWorld;
    public LayerMask destructibleLayers;
    public LayerMask directionHit;
    public bool isRunnable = false;
    public bool isJumpable = true;
    public Coroutine jumping;
    public float speed;
    public float jumpSpeed;
    public float health;
    public Weapons currWeapon;
    public PlayerState currState;
    public GameObject player;
    public GameObject playerArmature;
    public PlayerAnimationStates currAnimationState = PlayerAnimationStates.idle;
    

    public PlayerModel(SO_Player playerDetails, PlayerView playerView)
    {
        speed = playerDetails.playerSpeed;
        jumpSpeed = playerDetails.jumpSpeed;
        health = playerDetails.playerHealth;
        currWeapon = playerDetails.defaultWeapon;
        currState = playerDetails.defaultState;
        player = playerView.gameObject;
        playerArmature = player.transform.GetChild(0).gameObject;
        directionHit = playerDetails.pointerDetetionLayer;
    }

    public void destructModel()
    {
        jumping = null;
        player = null;
        playerArmature = null;
    }

}
