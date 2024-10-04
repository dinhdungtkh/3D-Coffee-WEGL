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
        //Debug.Log(currentSpeed);    
        //   Debug.Log($"IsSitting: {isSitting}, Speed: {navmeshAgent.velocity.magnitude}");
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
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 3.0f);
            isSitting = true;
            chair.fill = true;
            chair.SetOccupyingCharacter(this);
           
            if (gameObject.tag == "Bot"  && 0 < 1 )
            {
                Wait();
                chair.ClearOccupyingCharacter();
               
                isSitting = false;
                ChangeAnim("StandUp");

                animator.SetBool("StandUp", true);
                yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
                navmeshAgent.isStopped = false;
                navmeshAgent.ResetPath();
               
                //this.characterTransform.position = tempPos;
                // ChangeAnim("Idle");

            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
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
        navmeshAgent.isStopped = false;
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
        animator.SetBool("sitDown", true);
    }



    protected IEnumerator OnExit()
    {
        ChangeAnim("StandUp");
        animator.SetBool("standUp", true);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
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