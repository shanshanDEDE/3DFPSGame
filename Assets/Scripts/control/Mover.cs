using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [Tooltip("最大移動速度")]
    [SerializeField] float maxSpeed = 6f;

    [SerializeField] float animatorChangeRatio = 0.1f;

    NavMeshAgent navMeshAgent;
    float lastFramSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFramSpeed = Mathf.Lerp(lastFramSpeed, localVelocity.z, animatorChangeRatio);

        this.GetComponent<Animator>().SetFloat("WalkSpeed", lastFramSpeed / maxSpeed);
    }

    public void MoveTo(Vector3 destination, float speedRatio)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = destination;
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }

}
