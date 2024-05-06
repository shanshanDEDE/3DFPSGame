using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCanera : MonoBehaviour
{
    [Header("Camera跟隨的目標")]
    [SerializeField] Transform target;

    [Header("水平軸靈敏度")]
    [SerializeField] float sensitivity_X = 2;
    [Header("垂直軸靈敏度")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensitivity_Z = 5;

    [Header("最小垂直角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("最大垂直角度")]
    [SerializeField] float maxVerticalAngle = 85;
    [Header("相機與目標距離")]
    [SerializeField] float camaraToTargetDistance = 10;
    [Header("最小相機與目標距離")]
    [SerializeField] float minDistance = 2;
    [Header("最大相機與目標距離")]
    [SerializeField] float maxDistance = 25;
    [Header("PlaterTarget")]
    [SerializeField] GameObject player;
    [Header("受傷時撥放的特效")]
    [SerializeField] ParticleSystem beHitParticle;
    [Header("衝刺時撥放的特效")]
    [SerializeField] ParticleSystem springParticle;
    [Header("暫停UI")]
    [SerializeField] GameObject pauseUI;

    [Header("offset")]
    [SerializeField] Vector3 offset;

    [Header("Pause的音效")]
    [SerializeField] AudioClip pauseSFX;


    InputController input;
    AudioSource audioSource;

    float mouse_X = 0;
    float mouse_Y = 30;

    bool isChange;

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onSprint += OnSprint;

        audioSource = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        bool isLocked = false;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;

            mouse_X += input.GetMouseXAxis() * sensitivity_X;
            mouse_Y += input.GetMouseYAxis() * sensitivity_Y;

            mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);

            transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);
            transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(0, 0, -camaraToTargetDistance) + target.position + Vector3.up * offset.y;

            camaraToTargetDistance += input.GetMouseScrollWheelAxis() * sensitivity_Z;
            camaraToTargetDistance = Mathf.Clamp(camaraToTargetDistance, minDistance, maxDistance);

            isLocked = false;
        }
        else
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
            isLocked = true;
        }

        if (isLocked != isChange)
        {
            audioSource.PlayOneShot(pauseSFX);
            isChange = isLocked;
        }

        /*  if (Input.GetKeyDown(KeyCode.O))
         {
           //  Time.timeScale += 0.5f;
         }
         if (Input.GetKeyDown(KeyCode.P))
         {
            // Time.timeScale -= 0.5f;
         } */
    }

    private void OnDamage()
    {
        if (beHitParticle == null) return;

        beHitParticle.Play();
    }

    private void OnSprint()
    {
        if (springParticle == null) return;

        springParticle.Play();
    }
}
