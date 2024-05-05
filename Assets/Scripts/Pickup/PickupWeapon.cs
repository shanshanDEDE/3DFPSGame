using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [Header("撿起來會得到的武器")]
    [SerializeField] WeaponController weaponPrefab;

    [Header("要銷毀的gameobjec根節點")]
    [SerializeField] GameObject pickupRoot;

    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    void OnPick(GameObject player)
    {
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();

        if (weaponManager)
        {
            if (weaponManager.AddWeapon(weaponPrefab))
            {
                if (weaponManager.GetActiveWeapon() == null)
                {
                    weaponManager.SwitchWeapon(1);
                }

                Destroy(pickupRoot);
            }
        }
    }

}
