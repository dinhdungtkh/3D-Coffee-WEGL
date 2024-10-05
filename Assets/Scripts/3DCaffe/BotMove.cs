using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMove : Character
{
    public Table table;

    public Chair targetChair;
    // --------------------------------------------------------------------- 
    public float radiusRange;
    public Transform centrePoint;
    // ---------------------------------------------------------------------
    
    protected override void Start()
    {
        base.Start();
        StopMove();
        StartCoroutine(BotBehavior());
    }

    protected override void Update()
    {
        base.Update();
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
    public Chair FindTargetChair()
    {
        List<Chair> availableChairs = table.GetAvailableChairs();
        if (availableChairs.Count > 0 )
        { 
            targetChair = availableChairs[Random.Range(0, availableChairs.Count)];
            MoveTo(targetChair.transform.position);
        }
        return targetChair;
    }

    protected override void StandUp()
    {

        base.StandUp();
    }

    protected void FixedUpdate()
    {
        if (targetChair != null && currentAnimName != "StandUp")
        {
            MoveTo(targetChair.transform.position);
        }
        else
            StartCoroutine(FindNewTarget());
        
    }

    private IEnumerator BotBehavior()
    {
            MoveTo(GetRandomPosition());
            yield return new WaitForSeconds(Random.Range(4f, 5f));
    }

    private IEnumerator FindNewTarget()
    {
        List<Chair> availableChairs = table.GetAvailableChairs();
        if (availableChairs.Count > 0)
        {
             targetChair = availableChairs[Random.Range(0, availableChairs.Count)];
            if (targetChair.IsOccupied())
            {
                availableChairs.Remove(targetChair);
                if (availableChairs.Count > 0)
                {
                    targetChair = availableChairs[Random.Range(0, availableChairs.Count)];
                }
                else
                {
                    yield break; 
                }
            }

            MoveTo(targetChair.transform.position);
        }

        yield return new WaitUntil(() => IsFinishMove());
    }

    public void StopMove()
    {
        MoveTo(this.transform.position);
        ChangeAnim("Idle");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            targetChair = other.GetComponentInParent<Chair>();
            InteractWithChair(targetChair);
        }
    }

}