using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PoliceController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform rotationParent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationTime;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Animator animator;

    [SerializeField] private string runAnimationName;
    [SerializeField] private string idleAnimationName;

    private void Start()
    {
        navMeshAgent.enabled = false;
    }

    public void ActivateMovement()
    {
        navMeshAgent.enabled = true;
        navMeshAgent.speed = moveSpeed;
        animator.Play(runAnimationName);
        navMeshAgent.SetDestination(targetPosition.position);
        StartCoroutine(MovementWait());
    }
    
    private bool ReachedDestination()
    {
        if (navMeshAgent.pathPending) return false;
        //
        if (navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance) return false;
        
        if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0f) return false;

        // if (!transform.rotation.Equals(_targetRotation)) return false;
        
        return true;
    }

    private void LookAtPlayer()
    {
        animator.Play(idleAnimationName);
        rotationParent.DOLookAt((playerTransform.position).Vector3ToFlat(), rotationTime);
        Debug.Log("Look At");
    }

    private IEnumerator MovementWait()
    {
        while (!ReachedDestination())
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("A");
        }
        
        LookAtPlayer();
    }
}
