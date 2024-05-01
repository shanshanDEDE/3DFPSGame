using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollowSmooth : MonoBehaviour
{
    [Tooltip("跟隨的player目標")]
    [SerializeField] Transform player;
    [Tooltip("跟目標的最大距離")]
    [SerializeField] float distanceToTarget;
    [Tooltip("起始高度")]
    [SerializeField] float startHeight;
    [Tooltip("平滑的移動時間")]
    [SerializeField] float smoothTime;
    [Tooltip("滾輪軸靈敏度")]
    [SerializeField] float sensitivityOffset_Z;
    [Tooltip("最小垂直 Y Offset")]
    [SerializeField] float minOffset_Y;
    [Tooltip("最大垂直 Y Offset")]
    [SerializeField] float maxOffset_Y;

    float offset_Y;

    InputController input;
    Vector3 smoothPosition = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        transform.position = player.position + Vector3.up * startHeight;
        offset_Y = startHeight;
    }

    private void LateUpdate()
    {
        if (input.GetMouseScrollWheelAxis() != 0)
        {
            offset_Y += input.GetMouseScrollWheelAxis() * sensitivityOffset_Z;
            offset_Y = Mathf.Clamp(offset_Y, minOffset_Y, maxOffset_Y);
            Vector3 offsetTarget = player.position + player.up * offset_Y;
            transform.position = Vector3.Lerp(transform.position, offsetTarget, smoothTime);
        }

        if (CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up * offset_Y, ref currentVelocity, smoothTime);
            transform.position = smoothPosition;
        }
    }

    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }
}
