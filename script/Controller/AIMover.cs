using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIMover : MonoBehaviour
{
    [Tooltip("最大移動速度")]
    [SerializeField] float MaxSpeed = 6f;
    [SerializeField] float animatorChangeRatio = 0.2f;

    NavMeshAgent navMeshAgent;
    float lastFrameSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 1.8f;
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localvelocity = transform.InverseTransformDirection(velocity);
        //將全局的速度變量，轉變為local的速度變量

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localvelocity.z, animatorChangeRatio);

        this.GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed / MaxSpeed);
    }

    public void MoveTo(Vector3 destination, float SpeedRatio)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = MaxSpeed * Mathf.Clamp01(SpeedRatio);
        navMeshAgent.destination = destination;

        StartCoroutine(CheckDestinationReached(destination));
    }

    private IEnumerator CheckDestinationReached(Vector3 destination)
    {
        while(true)
        {
            if(Vector3.Distance(transform.position, destination) <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.isStopped = true;
                yield break;
            }
            yield return null;
        }
    }

    public void CancelMove()
    {
        navMeshAgent.isStopped = true;
    }
}
