using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("攻擊力")]
    [SerializeField] float attackDamage = 10f;
    [Header("攻擊範圍")]
    [SerializeField] float attackRange = 2f;
    [Header("攻擊間隔")]
    [SerializeField] float timeBetweenAttacks = 2f;
    [Header("轉向速度")]
    [SerializeField] float turnSpeed = 5f;  // 新增轉向速度參數

    Mover mover;
    Animator animator;
    Health health;
    Health targetHealth;

    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetHealth == null || targetHealth.IsDead()) return;

        if (IsInAttackRange())
        {
            FaceTarget();  // 使用自定義函數進行平滑轉向

            mover.CancelMove();
            AttaclBehaviour();
        }
        else if (timeSinceLastAttack > timeBetweenAttacks)
        {
            mover.MoveTo(targetHealth.transform.position, 1f);
        }

        UpdateTimer();
    }

    //GPT大神(解決玩家跳躍敵人物件bug以及瞬間看向玩家的bug)
    private void FaceTarget()
    {
        Vector3 direction = (targetHealth.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    private void AttaclBehaviour()
    {
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            timeSinceLastAttack = 0;
            TriggerAttack();
        }
    }

    private void TriggerAttack()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }

    private void Hit()
    {
        if (targetHealth == null) return;

        if (IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }

    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    public void Attack(Health target)
    {
        targetHealth = target;
    }

    public void cancelTarget()
    {
        targetHealth = null;
    }


    private void OnDie()
    {
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
