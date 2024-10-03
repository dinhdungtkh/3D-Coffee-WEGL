using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMove : Character
{
    public Table table; 
    [SerializeField]
    private float updateInterval = 5f; 
    private float nextUpdateTime = 0f;

    public Chair targetChair;
    public Transform[] points;
    private int destPoint = 0;
    private float sitDuration = 5f; 
    private float sitStartTime;

    public float moveSpeed = 3f;
    public Vector3 randomPos;
    // --------------------------------------------------------------------- 
    public float radiusRange;
    public Transform centrePoint;

    protected override void Start()
    {
        base.Start();
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
    private void FindTargetChair()
    {
        List<Chair> availableChairs = table.GetAvailableChairs();
        if (availableChairs.Count > 0)
        {
            targetChair = availableChairs[Random.Range(0, availableChairs.Count)];
            MoveTo(targetChair.transform.position);
        }
    }

    protected override void StandUp()
    {
        
        base.StandUp();
    }

    protected void FixedUpdate()
    {
        if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
        {
            Vector3 point = GetRandomPosition();
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                MoveTo(point);
        }
    }

    private IEnumerator BotBehavior()
    {
        while (true)
        {
            MoveTo(GetRandomPosition());
            yield return new WaitUntil(() => IsFinishMove());

            yield return new WaitForSeconds(Random.Range(4f, 5f));
            FindTargetChair();
            yield return new WaitUntil(() => IsFinishMove());
            
            yield return new WaitForSeconds(3f);
            StandUp(); 
        }
    }


    private void StopMove()
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

    
    public bool IsFinishMove()
    {
        if (!navmeshAgent.pathPending && navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
        {
            return true;
        }
        return false;
    }

}