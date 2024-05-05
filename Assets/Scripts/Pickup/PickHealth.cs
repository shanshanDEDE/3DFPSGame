using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickHealth : MonoBehaviour
{
    [Header("需要恢復的血量")]
    [SerializeField] float healthAmount;

    [Header("要銷毀的gameobjec根節點")]
    [SerializeField] GameObject pickupRoot;

    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();
        pickup.onPick += OnPick;
    }

    private void OnPick(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        if (health)
        {
            health.Heal(healthAmount);
            Destroy(pickupRoot);
        }
    }

}
