using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Details",menuName ="Player")]
public class PlayerData : ScriptableObject
{
    public PlayerView playerView;
    public float playerSpeed;
    public float jumpSpeed;
    public float playerHealth;
    public EnumWeapons defaultWeapon; 
    public EnumPlayerStates defaultState;
    public LayerMask pointerDetetionLayer;
}