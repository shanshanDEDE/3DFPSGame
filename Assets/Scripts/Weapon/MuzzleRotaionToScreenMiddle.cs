using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleRotaionToScreenMiddle : MonoBehaviour
{
    [Header("Ray最大距離")]
    [SerializeField] float maxDistance;

    Ray ray;
    RaycastHit hit;

    [SerializeField] float offset_Y = 10f;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + offset_Y, 0));

        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.blue);
    }
}
