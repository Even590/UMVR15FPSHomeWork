using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AIController : MonoBehaviour
{
    [SerializeField] float ChaseDistance = 10f;
    [SerializeField] float AttackRange = 1.5f;
    // [SerializeField] float ConfuseTime = 5f;

    // [SerializeField] PatorlPath patorl;
    // [SerializeField] float WayPointToStay = 0.5f;
    // [SerializeField] float WayPointToWaitTime = 3f;
    // [Range(0, 1)]
    // [SerializeField] float PatorlSpeedRatio = 0.5f;

    // [SerializeField] float enemyRatius = 0.5f;
    // [SerializeField] float playerRatius = 0.5f;

    GameObject player;
    AIMover aIMover;
    Animator animator;
    Health health;
    Fighter fighter;

    // private float TimeSinceLastSawPlayer = Mathf.Infinity;
    // private Vector3 BeginPosition;
    private Rigidbody[] rigidbodies;
    // int CurrentWayPointIndex = 0;
    // float TimeSinceArriveWayPoint = 0;

    public ZombieSpawn zombieSpawn;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aIMover = GetComponent<AIMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<Fighter>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        // BeginPosition = transform.position;
        health.OnDamage += onDamage;
        health.OnDead += onDead;

        foreach(var rb in rigidbodies)
        {
            rb.isKinematic = true;
            Collider collider = rb.GetComponent<Collider>();
            if(collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    private void Update()
    {
        if(health.IsDead()) return;
        Debug.Log("inattackRange" + IsInAttackRange());

        if (IsInRange() && !IsInAttackRange())
        {
            animator.SetBool("IsConfuse", false);
            // TimeSinceLastSawPlayer = 0;
            aIMover.MoveTo(player.transform.position, 1);
        }
        else if(IsInAttackRange())
        {
            aIMover.CancelMove();
            AttackBehaviour();
        }
        // else if (TimeSinceLastSawPlayer < ConfuseTime)
        // {
        //     ConfuseBehaviour();
        // }
        // else
        // {
        //     PatorlBehaviour();
        // }

        UpdateTime();
    }

    private bool IsInAttackRange()
    {
        Vector3 flatPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayerPostion = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        
        return Vector3.Distance(flatPosition,flatPlayerPostion) < AttackRange;
    } 

    private void AttackBehaviour()
    {
        animator.SetBool("IsCoufuse", false);
        // TimeSinceLastSawPlayer = 0;
        fighter.Attack(player.GetComponent<PlayerHealth>());

        animator.SetTrigger("Attack");
    }

    // private void PatorlBehaviour()
    // {
    //     if (patorl != null)
    //     {
    //         if (IsAtWayPoint())
    //         {
    //             aIMover.CancelMove();
    //             animator.SetBool("IsConfuse", true);
    //             TimeSinceArriveWayPoint = 0;
    //             CurrentWayPointIndex = patorl.GetNextWayPointNumble(CurrentWayPointIndex);
    //         }
    //         if (TimeSinceArriveWayPoint > WayPointToWaitTime)
    //         {
    //             animator.SetBool("IsConfuse", false);
    //             aIMover.MoveTo(patorl.GetWayPointPosition(CurrentWayPointIndex), PatorlSpeedRatio);
    //         }
    //     }
    //     else
    //     {
    //         animator.SetBool("IsConfuse", false);
    //         aIMover.MoveTo(BeginPosition, 0.5f);
    //     }
    // }

    // private bool IsAtWayPoint()
    // {
    //     return Vector3.Distance(transform.position, patorl.GetWayPointPosition(CurrentWayPointIndex)) < WayPointToStay;
    // }

    // private void ConfuseBehaviour()
    // {
    //     aIMover.CancelMove();
    //     fighter.CancelTarget();
    //     animator.SetBool("IsConfuse", true);
    // }

    private bool IsInRange()
    {
        Vector3 flatPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayerPostion = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        return Vector3.Distance(flatPosition, flatPlayerPostion) < ChaseDistance;
    }

    private void UpdateTime()
    {
        // TimeSinceLastSawPlayer += Time.deltaTime;
        // TimeSinceArriveWayPoint += Time.deltaTime;
    }

    private void onDamage()
    {
        //受到攻擊時觸發
    }

    private void onDead()
    {
        zombieSpawn.OnZombieKilled();
        animator.enabled = false;
        aIMover.CancelMove();
        CapsuleCollider Collider = GetComponent<CapsuleCollider>();
        Collider.enabled = false;
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            Collider collider = rb.GetComponent<Collider>();
            if(collider != null)
            {
                collider.enabled = true;
            }
        }

        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseDistance);
    }
}