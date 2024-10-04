using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    public string Name;
    [SerializeField]
    protected Animator animator;
    protected NavMeshAgent navmeshAgent;
    [SerializeField]
    protected Transform chairPosition;

    [SerializeField]
    public string currentAnimName = "Idle";
    protected Transform characterTransform;
    private Character mycharacter;

    protected virtual void Start()
    {
        Character mycharacter = GetComponent<Character>();
        Name = mycharacter.gameObject.name;
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterTransform = GetComponent<Transform>();
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
    protected virtual void InteractWithChair(Chair chair)
    {
        if (chair != null)
        {
            if (!chair.IsOccupied())
                StartCoroutine(Sit(chair));
        }
    }

    private IEnumerator Sit(Chair chair)
    {
            chairPosition = chair.getchairPosition();
            Vector3 frontPoint = chair.getchairFrontPosition().position;
            Quaternion rotation = chair.chairPosition.rotation;
            Vector3 rotationAngle = rotation.eulerAngles;
            MoveTo(frontPoint);
            yield return new WaitUntil(() => IsFinishMove() == true);
            this.characterTransform.position = chairPosition.position + new Vector3(0, 0.02f, 0);
            this.characterTransform.rotation = Quaternion.Euler(characterTransform.rotation.x, rotationAngle.y,
                                      characterTransform.rotation.z);
            
            Vector3 tempPos = this.characterTransform.position;

            navmeshAgent.isStopped = true;
            ChangeAnim("SitDown");
            chair.fill = true;
            chair.SetOccupyingCharacter(mycharacter);

           
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
