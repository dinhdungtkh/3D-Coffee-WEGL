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

    [SerializeField]
    public string currentAnimName = "Idle";
    [SerializeField]
    private float currentSpeed;
    public float CharacterSpeed { get => currentSpeed; }

    protected virtual void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterTransform = GetComponent<Transform>();

    }

    protected virtual void Update()
    {
        currentSpeed = navmeshAgent.velocity.magnitude / navmeshAgent.speed;
        if (!isSitting)
        {
            animator.SetFloat("speedPercent", currentSpeed, 0.03f, Time.deltaTime);
        }
    }

    protected virtual void MoveTo(Vector3 position)
    {
        navmeshAgent.SetDestination(position);

    }

    protected void ChangeAnim(string newAnimName)
    {
        if (currentAnimName != newAnimName)
        {
            animator.ResetTrigger(currentAnimName);
            currentAnimName = newAnimName;
            animator.SetTrigger(newAnimName);
        }
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
            this.characterTransform.position = chairPosition.position + new Vector3(0,0.03f,0);
            this.characterTransform.rotation = Quaternion.Euler(characterTransform.rotation.x, rotationAngle.y,
                                      characterTransform.rotation.z);
            Vector3 tempPos = Vector3.zero;
            navmeshAgent.isStopped = true;
            OnSitDown();
            isSitting = true;
            chair.fill = true;
            chair.SetOccupyingCharacter(this);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(4f);
    }

    protected virtual void StandUp()
    {
        StartCoroutine(OnExit());
        if (chairPosition != null)
        {
            Chair chair = chairPosition.GetComponent<Chair>();
            if (chair != null)
            {
                chair.ClearOccupyingCharacter();
            }
        }
        isSitting = false;
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
        ChangeAnim("SitDown");
       // animator.SetBool("sitDown", true);
    }



    protected IEnumerator OnExit()
    {
        yield return new WaitForEndOfFrame();
        //navmeshAgent.ResetPath();
        ChangeAnim("StandUp");
        navmeshAgent.isStopped = true;
        // animator.SetBool("standUp", true);
       //  Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0f);
         yield return new WaitForSeconds(1.88f);
        navmeshAgent.isStopped = false;
    }


    public bool IsFinishMove()
    {
        if (!navmeshAgent.pathPending && navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance )
        {
            return true;
        }
        return false;
    }
}