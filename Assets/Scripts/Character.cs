using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected NavMeshAgent navmeshAgent;
    [SerializeField]
    protected bool isSitting = false;
    [SerializeField]
    protected Transform chairPosition;



    
    protected virtual void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (!isSitting)
        {
            animator.SetFloat("speedPercent", navmeshAgent.velocity.magnitude / navmeshAgent.speed, .1f, Time.deltaTime);
        }
    }

    protected virtual void MoveTo(Vector3 position)
    {
        if (!isSitting)
        {
            navmeshAgent.SetDestination(position);
        }
         else
        {
            Debug.Log("Nhân vật đang ngồi, không thể di chuyển."); 
        }
    }

    protected virtual void Sit(Chair chair)
    {
        if (!isSitting)
        {
            isSitting = true;
            Vector3 frontPoint = chair.chairPosition().position;
            Debug.Log(frontPoint);
           MoveTo(frontPoint);
            navmeshAgent.isStopped = true;
            animator.SetFloat("speedPercent",0, .1f, Time.deltaTime);
            transform.position = chair.transform.position;
            OnSitDown();
           // StartCoroutine(WaitAndStand());
        }
    }

    private IEnumerator WaitAndStand()
    {
        yield return new WaitForSeconds(3);
        StandUp();
    }

    protected virtual void StandUp()
    {
        isSitting = false;
        navmeshAgent.isStopped = false;
        OnExit();
    }

    protected virtual void InteractWithChair(Chair chair)
    {
        if (chair != null)
        {
            Sit(chair);
        }
    }


    protected virtual void OnSitDown()
    {
        animator.SetTrigger("SitDown"); 
        
    }

    protected virtual void OnExit()
    {
        animator.SetTrigger("StandUp");
    }
}
