using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFighter : MonoBehaviour
{
    [Header("角色攻擊類型")]
    [SerializeField] Actor actorType;

    [Header("攻擊力")]
    [SerializeField] float attackDamage = 10f;
    [Header("跳躍攻擊力")]
    [SerializeField] float jumpAttackDamage = 10f;
    [Header("攻擊範圍")]
    [SerializeField] float attackRange = 2f;
    [Header("跳躍攻擊距離")]
    [SerializeField] float jumpAttackRange = 2f;

    [Header("攻擊間隔")]
    [SerializeField] float timeBetweenAttacks = 2f;

    [Header("轉向速度")]
    [SerializeField] float turnSpeed = 5f;  // 新增轉向速度參數

    [Space(20)]
    [Header("要丟出去的Projectile")]
    [SerializeField] Projectile throwProjectile;
    [Header("手部座標")]
    [SerializeField] Transform hand;


    RootMover mover;
    Animator animator;
    Health health;
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<RootMover>();
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
            AttaclBehaviour("Attack");
        }
        else if (IsInJumpAttackRange())
        {
            mover.CancelMove();
            AttaclBehaviour("JumpAttack");
        }
        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttacks)
        {
            mover.MoveTo(targetHealth.transform.position);
        }

        UpdateTimer();
    }

    // 檢查攻擊動作是否已經結束
    private bool CheckHasAttack()
    {
        baseLayer = animator.GetCurrentAnimatorStateInfo(0);
        // 確認目前動畫是否為攻擊動畫
        if (baseLayer.fullPathHash == Animator.StringToHash("Base Layer.Attack"))
        {
            return false;
        }
        else
        {
            return true;
        }

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

    private void AttaclBehaviour(String attackName)
    {
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            timeSinceLastAttack = 0;
            TriggerAttack(attackName);
        }
    }

    private void TriggerAttack(String attackName)
    {
        animator.ResetTrigger(attackName);
        animator.SetTrigger(attackName);
    }

    private void Hit()
    {
        if (targetHealth == null || (actorType != Actor.Melee && actorType != Actor.Boss)) return;

        if (IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }

    }

    private void JumpHit()
    {
        if (targetHealth == null || actorType != Actor.Boss) return;

        if (IsInJumpAttackRange())
        {
            targetHealth.TakeDamage(jumpAttackDamage);
        }

    }

    private void Shoot()
    {
        if (actorType != Actor.Acher) return;

        if (throwProjectile != null)
        {
            Projectile newProjectile = Instantiate(throwProjectile, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }

    private bool IsInJumpAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < jumpAttackRange;
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpAttackRange);
    }
}