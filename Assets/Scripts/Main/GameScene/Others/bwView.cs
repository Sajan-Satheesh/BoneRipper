using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bwView : MonoBehaviour
{
    public int dealtDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyView>(out EnemyView enemyView))
        {
            Debug.Log("hit " + enemyView);
            WeaponServices.instance.hitDestructable(enemyView, Weapons.boneBreaker);
        }
    }
}
