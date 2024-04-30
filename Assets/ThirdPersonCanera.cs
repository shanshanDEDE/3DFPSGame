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

    [Header("offset")]
    [SerializeField] Vector3 offset;

    InputController input;

    float mouse_X = 0;
    float mouse_Y = 30;

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
    }

    private void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            mouse_X += input.GetMouseXAxis() * sensitivity_X;
            mouse_Y += input.GetMouseYAxis() * sensitivity_Y;

            mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);

            transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);
            transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(0, 0, -camaraToTargetDistance) + target.position + Vector3.up * offset.y;

            camaraToTargetDistance += input.GetMouseScrollWheelAxis() * sensitivity_Z;
            camaraToTargetDistance = Mathf.Clamp(camaraToTargetDistance, minDistance, maxDistance);
        }
    }
}
