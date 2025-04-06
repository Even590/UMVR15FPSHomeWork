using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Animations;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] float attackDamge = 10f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float timeBetweenAttack = 2f;

    AIMover aIMover;
    Animator animator;
    Health health;
    PlayerHealth TargetHealth;
    AnimatorStateInfo BaseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        aIMover = GetComponent<AIMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.OnDead += ondead;
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetHealth == null || TargetHealth.IsDead()) return;

        if(IsInAttackRange())
        {
            aIMover.CancelMove();
            AttackBehaviour();
        }
        else if(CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            aIMover.MoveTo(TargetHealth.transform.position,1);
        }

        UpdateTimer();
    }

    //檢查攻擊動畫是否結束
    private bool CheckHasAttack()
    {
        BaseLayer = animator.GetCurrentAnimatorStateInfo(0);
        
        //如果當前動畫處於攻擊狀態，就回傳false去避免AI提前執行追擊行為
        if(BaseLayer.fullPathHash == Animator.StringToHash("Base Layer.Attack"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    //攻擊行為判定
    private void AttackBehaviour()
    {
        //鎖定AI的Y軸，避免AI碰撞會倒地
        Vector3 targetPosition = TargetHealth.transform.position;
        targetPosition.y = transform.position.y;

        //讓AI攻擊玩家時永遠面向玩家
        transform.LookAt(targetPosition);

        if(timeSinceLastAttack > timeBetweenAttack)
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
        if(TargetHealth == null) return;

        if(IsInAttackRange())
        {
            TargetHealth.TakeDamage(attackDamge);
        }
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, TargetHealth.transform.position) < attackRange;
    }

    public void Attack(PlayerHealth Target)
    {
        TargetHealth = Target;
    }

    public void CancelTarget()
    {
        TargetHealth = null;
    }

    private void ondead()
    {
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
