using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowView : Shootable
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * 10 * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerView player))
        {
            WeaponServices.instance.hitDestructable(player, Weapons.spear);
        }
    }
}
