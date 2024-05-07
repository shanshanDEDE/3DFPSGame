using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RootMover : MonoBehaviour
{
    [Header("旋轉速度")]
    [SerializeField] float rotateSpeed = 3f;

    NavMeshAgent navMeshAgent;
    Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        if (!navMeshAgent.isStopped)
        {
            Vector3 targetPosition = navMeshAgent.steeringTarget - transform.position;

            if (targetPosition == Vector3.zero) return;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition, Vector3.up), rotateSpeed * Time.deltaTime);

        }
    }

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = destination;
        animator.SetBool("Move", true);
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("Move", false);
    }

}
