using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotChar : Character
{
    public Table table;
    public Chair targetChair;

    public float radiusRange;
    public Transform centrePoint;

    protected override void Start()
    {
        base.Start();
        StopMove();
        StartCoroutine(BotBehavior());
    }

    protected override void Update()
    {
        base.Update();
        if (targetChair != null && !IsFinishMove() && currentAnimName != "StandUp")
        {
            MoveTo(targetChair.transform.position);
        }
    }

    private IEnumerator BotBehavior()
    {
        while (true)
        {
            yield return MoveToRandomPosition();
            yield return StartCoroutine(FindAndMoveToChair());
            yield return StartCoroutine(SitAndWait());
            StandUp();
        }
    }

    private IEnumerator MoveToRandomPosition()
    {
        Vector3 randomPosition = GetRandomPosition();
        MoveTo(randomPosition);
        yield return new WaitUntil(() => IsFinishMove());
    }

    private IEnumerator FindAndMoveToChair()
    {
        targetChair = table.GetRandomAvailableChair();

        if (targetChair != null)
        {
            MoveTo(targetChair.transform.position);
            yield return new WaitUntil(() => IsFinishMove());
        }
    }

    private IEnumerator SitAndWait()
    {
        if (targetChair != null)
        {
            chairPosition = targetChair.getchairPosition();
            Vector3 frontPoint = targetChair.getchairFrontPosition().position;
            Quaternion rotation = targetChair.chairPosition.rotation;
            Vector3 rotationAngle = rotation.eulerAngles;
            MoveTo(frontPoint);
            yield return new WaitUntil(() => IsFinishMove());
            this.characterTransform.position = chairPosition.position + new Vector3(0, 0.03f, 0);
            this.characterTransform.rotation = Quaternion.Euler(characterTransform.rotation.x, rotationAngle.y, characterTransform.rotation.z);
            navmeshAgent.isStopped = true;
            ChangeAnim("SitDown");

            isSitting = true;
            targetChair.fill = true;
        }
        yield return new WaitForSeconds(3f);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        bool validPosition = false;
        while (!validPosition)
        {
            Vector3 randomPoint = centrePoint.position + Random.insideUnitSphere * radiusRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
                validPosition = true;
            }
        }
        return randomPosition;
    }

    protected override void StandUp()
    {
        base.StandUp();
        if (targetChair != null)
        {
            table.ReleaseChair(targetChair);
        }
        isSitting = false;
        targetChair = null;
    }

    public void StopMove()
    {
        MoveTo(this.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Chair"))
        //{
        //    Chair newtargetChair = other.GetComponentInParent<Chair>();
        //    if (targetChair != null && newtargetChair == targetChair )
        //    {
        //        if (!newtargetChair.IsOccupied())
        //            StartCoroutine(Sit(newtargetChair));
        //    } 

        //}
    }
}
