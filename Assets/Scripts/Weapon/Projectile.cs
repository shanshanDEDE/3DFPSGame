using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("子彈速度")]
    [SerializeField] float prjectileSpeed;

    [Header("Projectile的存活時間")]
    [SerializeField] float maxLifeTime = 3f;
    [Header("重力")]
    [SerializeField] float gravityDownForce = 0f;

    //當前速度
    Vector3 currentVelocity;

    private void OnEnable()
    {
        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        transform.position += currentVelocity * Time.deltaTime;

        if (gravityDownForce > 0)
        {
            currentVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }
    }

    public void Shoot()
    {
        currentVelocity = transform.forward * prjectileSpeed;
    }
}
