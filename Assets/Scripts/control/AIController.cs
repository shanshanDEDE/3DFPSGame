using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("追趕距離")]
    [SerializeField] float chaseDistance = 10f;
    [Header("失去目標後困惑的時間")]
    [SerializeField] float confuseTime = 5f;

    [Header("patrol的GameObject物件")]
    [SerializeField] PatrolPath patrol;
    [Header("需要到達waypoint的距離")]
    [SerializeField] float wayPointToStay = 3f;
    [Header("待在WayPoint的時間")]
    [SerializeField] float wayPointToWaitTime = 3f;
    [Header("巡邏時的速度")]
    [Range(0f, 1f)]
    [SerializeField] float patrolPathSpeedRatio = 0.5f;

    private float timeSinceLastSawPlayer;
    private Vector3 beginPosition;
    int currentWayPointIndex = 0;
    float timeSinceArrivedWayPoint = 0f;
    bool isBeHit;

    GameObject player;
    Mover mover;
    Animator animator;
    Health health;
    Fighter fighter;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<Fighter>();

        beginPosition = transform.position;
        health.onDamage += OnDamage;
        health.onDie += OnDie;
    }

    private void Update()
    {
        if (health.IsDead()) return;

        if (IsInRange() || isBeHit)
        {
            AttackBehaviour();
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            ConfuseBeHaviour();
        }
        else
        {
            PatrolBeHaviour();
        }

        UpdateTimer();
    }

    private void AttackBehaviour()
    {
        animator.SetBool("IsConfuse", false);
        timeSinceLastSawPlayer = 0;
        fighter.Attack(player.GetComponent<Health>());
    }

    //巡邏行為
    private void PatrolBeHaviour()
    {
        Vector3 nexxtWayPointPosition = beginPosition;
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                animator.SetBool("IsConfuse", true);
                timeSinceArrivedWayPoint = 0f;
                currentWayPointIndex = patrol.GetNextWayPointNumber(currentWayPointIndex);
            }

            if (timeSinceArrivedWayPoint > wayPointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWayPointIndex), patrolPathSpeedRatio);
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(beginPosition, 0.5f);
        }

    }

    private bool IsAtWayPoint()
    {
        return Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWayPointIndex)) < wayPointToStay;
    }

    //困惑動作行為
    private void ConfuseBeHaviour()
    {
        mover.CancelMove();
        fighter.cancelTarget();
        animator.SetBool("IsConfuse", true);
    }

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void UpdateTimer()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArrivedWayPoint += Time.deltaTime;
    }

    private void OnDamage()
    {
        isBeHit = true;
    }

    private void OnDie()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
