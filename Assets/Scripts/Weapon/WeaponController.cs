using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponShootType
{
    single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{

    [Header("武器的主要GameObject,不使用時將被隱藏")]
    [SerializeField] GameObject weaponRoot;
    [Header("槍口位置,發射子彈的位置")]
    [SerializeField] Transform weaponMuzzle;

    [Space(5)]
    [Header("射擊形式")]
    [SerializeField] WeaponShootType shootType;
    [Header("兩次射擊之間的Delay時間")]
    [SerializeField] float delayBetweenShots = 0.5f;
    [Header("射一發所需的子彈數量")]
    [SerializeField] int bulletPerShot;

    [Space(5)]
    [Header("每秒Reload彈藥的數量")]
    [SerializeField] float ammoReloadRate = 1f;
    [Header("兩次Reload之間的Delay時間")]
    [SerializeField] float ammoReloadDelay = 2f;
    [Header("最大彈藥數量")]
    [SerializeField] float maxAmmo = 8;

    public GameObject sourcePrefab { get; set; }

    // 紀錄當前彈藥數量
    float currentAmmo;
    // 紀錄最後一次發射的時間
    float timeSinceLastShot = 0f;
    //是否在瞄準狀態
    bool isAim;


    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {

    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);
    }

    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.single:
                if (inputDown)
                {
                    print("single射擊");
                }
                return;
            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    print("Auto射擊");
                }
                return;
            default:
                return;
        }
    }
}
