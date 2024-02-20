using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordView : Shootable
{
    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerView player))
        {
            WeaponServices.instance.HitDestructable(player, EnumWeapons.SPEAR);
        }
        /*if (collision.gameObject.layer == 10)
        {
            WeaponServices.instance.hitDestructable(collision.rigidbody.gameObject.GetComponent<PlayerView>(), Weapons.spear);
        }*/
    }
}
