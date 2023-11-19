using UnityEngine;

[CreateAssetMenu(fileName ="Enemy Details",menuName ="Enemy")]
public class SO_Enemy : ScriptableObject
{
    public EnemyView enemyView;
    public float enemySpeed;
    public float enemyHealth;
    public Weapons defaultWeapon; 
    public EnemyState defaultState;
}