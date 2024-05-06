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
    [Header("Icon")]
    public Sprite weaponIcon;

    [Header("武器的主要GameObject,不使用時將被隱藏")]
    [SerializeField] GameObject weaponRoot;
    [Header("槍口位置,發射子彈的位置")]
    [SerializeField] Transform weaponMuzzle;

    [Space(5)]
    [Header("射擊形式")]
    [SerializeField] WeaponShootType shootType;
    [Header("Projectile的Prefab")]
    [SerializeField] Projectile projectilePrefab;
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

    [Space(5)]
    [Header("槍口發射時產生的特效")]
    [SerializeField] GameObject muzzleFlashPrefab;
    [Header("Shoot的音效")]
    [SerializeField] AudioClip shootSFX;
    [Header("切換到這個武器時的音效")]
    [SerializeField] AudioClip changeWeaponSFX;

    public GameObject sourcePrefab { get; set; }
    public float currentAmmoRatio { get; private set; }
    public bool isCooling { get; private set; }

    AudioSource audioSource;

    // 紀錄當前彈藥數量
    float currentAmmo;
    // 紀錄最後一次發射的時間
    float timeSinceLastShot = 0f;
    //是否在瞄準狀態
    bool isAim;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        if (timeSinceLastShot + ammoReloadDelay < Time.time && currentAmmo < maxAmmo)
        {
            // 開始reload
            currentAmmo += ammoReloadRate * Time.deltaTime;

            currentAmmo = Mathf.Clamp(currentAmmo, 0f, maxAmmo);

            isCooling = true;
        }
        else
        {
            isCooling = false;
        }

        if (maxAmmo == Mathf.Infinity)
        {
            currentAmmoRatio = 1f;
        }
        else
        {
            currentAmmoRatio = currentAmmo / maxAmmo;
        }
    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);

        if (value && changeWeaponSFX)
        {
            audioSource.PlayOneShot(changeWeaponSFX);
        }
    }

    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.single:
                if (inputDown)
                {
                    TryShoot();
                }
                return;
            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    TryShoot();
                }
                return;
            default:
                return;
        }
    }

    private void TryShoot()
    {
        if (currentAmmo >= 1f && timeSinceLastShot + delayBetweenShots < Time.time)
        {
            HandleShootInput();
            currentAmmo -= 1f;
        }
    }

    private void HandleShootInput()
    {
        for (int i = 0; i < bulletPerShot; i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            newProjectile.Shoot(GameObject.FindGameObjectWithTag("Player"));
        }

        if (muzzleFlashPrefab != null)
        {
            /*    GameObject newProjectile=Instantiate(muzzleFlashPrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward)); */
            GameObject newProjectile = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
            Destroy(newProjectile, 1.5f);
        }

        if (shootSFX != null)
        {
            audioSource.PlayOneShot(shootSFX);
        }



        timeSinceLastShot = Time.time;
    }
}
