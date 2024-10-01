using System.Collections;
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

    [SerializeField]
    protected Transform characterTransform;


    protected virtual void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterTransform = GetComponent<Transform>();
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
        navmeshAgent.SetDestination(position);
    }

    protected virtual IEnumerator Sit(Chair chair)
    {
        if (!isSitting)
        {
            chairPosition = chair.getchairPosition();
            Vector3 frontPoint = chair.getchairFrontPosition().position;
            Quaternion rotation = chair.chairPosition.rotation;
            Vector3 rotationAngle = rotation.eulerAngles;
            MoveTo(frontPoint);
            yield return new WaitUntil(() => IsFinishMove() == true);
            this.characterTransform.rotation = Quaternion.Euler(characterTransform.rotation.x, rotationAngle.y,
                                      characterTransform.rotation.z);
            navmeshAgent.isStopped = true;
            OnSitDown();
            isSitting = true;
            chair.fill = true;
            chair.SetOccupyingCharacter(this);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

    protected virtual void StandUp()
    {
        isSitting = false;
        navmeshAgent.isStopped = false;
        OnExit();
        if (chairPosition != null)
        {
            Chair chair = chairPosition.GetComponent<Chair>();
            if (chair != null)
            {
                chair.ClearOccupyingCharacter();
            }
        }
    }

    protected virtual void InteractWithChair(Chair chair)
    {
        if (chair != null)
        {   
            if (!chair.IsOccupied())
            StartCoroutine(Sit(chair));
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


    public bool IsFinishMove()
    {
        if (!navmeshAgent.pathPending && navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
}
