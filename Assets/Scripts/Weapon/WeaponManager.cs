using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("初始武器")]
    [SerializeField] List<WeaponController> startingWeapons = new List<WeaponController>();

    [Header("儲存武器位置的Parent,武器會被加在這裡")]
    [SerializeField] Transform equipWeaponParent;

    [Header("瞄準時間")]
    [SerializeField] float aimTime = 2f;

    public event Action<WeaponController, int> onAddWeapon;

    // 目前裝備的武器清單位置
    int activeWeaponIndex;

    //武器最多三個
    WeaponController[] weapons = new WeaponController[3];
    PlayerController player;
    InputController input;

    bool isAim;

    void Start()
    {
        // 初始狀態
        activeWeaponIndex = -1;

        input = GameManagerSingleton.Instance.InputController;
        player = GetComponent<PlayerController>();
        player.onAim += OnAim;

        foreach (WeaponController weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }

        SwitchWeapon(1);
    }

    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        if (activeWeapon && isAim)
        {
            activeWeapon.HandleShootInput(
                input.GetFireInputDown(),
                input.GetFireInputHeld(),
                input.GetFireInputUp()
            );
        }


        int switchWeaponInput = input.GetSwitchWeaponInput();
        if (switchWeaponInput != 0)
        {
            SwitchWeapon(switchWeaponInput);
        }
    }

    //切換武器
    public void SwitchWeapon(int addIndex)
    {
        int newWeaponIndex = -1;
        if (activeWeaponIndex + addIndex > weapons.Length - 1)
        {
            newWeaponIndex = 0;
        }
        else if (activeWeaponIndex + addIndex < 0)
        {
            newWeaponIndex = weapons.Length - 1;
        }
        else
        {
            newWeaponIndex = activeWeaponIndex + addIndex;
        }

        SwitchToWeaponIndex(newWeaponIndex);
    }

    private void SwitchToWeaponIndex(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            if (GetWeaponAtSlotIndex(index) != null)
            {
                //如果目前已經裝備武器,就隱藏原武器
                if (GetActiveWeapon() != null)
                {
                    GetActiveWeapon().ShowWeapon(false);
                }

                //顯示武器
                activeWeaponIndex = index;
                GetActiveWeapon().ShowWeapon(true);
            }

        }
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        //找到weapon在slot的位置並回傳該武器
        if (index >= 0 && index < weapons.Length - 1 && weapons[index] != null)
        {
            return weapons[index];
        }

        //如果沒有找到該武器 就回傳Null
        return null;
    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {
        //確認目前Slot裡沒有該武器
        if (HasWeapon(weaponPrefab))
        {
            return false;
        }

        //找到下一個空間沒有裝武器
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                //產生Weapon到設定好的位置底下
                WeaponController weaponInstance = Instantiate(weaponPrefab, equipWeaponParent);

                weaponInstance.sourcePrefab = weaponPrefab.gameObject;
                //隱藏武器
                weaponInstance.ShowWeapon(false);

                weapons[i] = weaponInstance;

                onAddWeapon?.Invoke(weaponInstance, i);

                return true;
            }
        }

        return false;

    }

    private bool HasWeapon(WeaponController weaponPrefab)
    {
        foreach (WeaponController weapon in weapons)
        {
            if (weapon != null && weapon.sourcePrefab == weaponPrefab)
            {
                return true;
            }
        }

        return false;
    }

    bool isAimingCoroutineRunning = false;

    private void OnAim(bool value)
    {
        if (value)
        {
            if (!isAimingCoroutineRunning)
            {
                StopAllCoroutines();
                StartCoroutine(DelayAim());
            }
        }
        else
        {
            StopAllCoroutines();
            isAimingCoroutineRunning = false;
            isAim = value;
        }
    }

    IEnumerator DelayAim()
    {
        isAimingCoroutineRunning = true;
        yield return new WaitForSecondsRealtime(aimTime);
        isAim = true;
        isAimingCoroutineRunning = false;
    }
}
