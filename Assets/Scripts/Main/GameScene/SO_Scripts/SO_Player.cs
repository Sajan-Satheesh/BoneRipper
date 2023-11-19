using UnityEngine;

[CreateAssetMenu(fileName ="Player Details",menuName ="Player")]
public class SO_Player : ScriptableObject
{
    public PlayerView playerView;
    public float playerSpeed;
    public float jumpSpeed;
    public float playerHealth;
    public Weapons defaultWeapon; 
    public PlayerState defaultState;
    public LayerMask pointerDetetionLayer;
    
}