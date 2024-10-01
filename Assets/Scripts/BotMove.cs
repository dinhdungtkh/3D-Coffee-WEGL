using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotMove : Character
{
    public Transform[] points;
    private int destPoint = 0;
    public float moveSpeed = 3f;
    private bool isSitting = false;

    void Start()
    {
        GotoNextPoint();
        
    }

    void GotoNextPoint()
    {
        if (points.Length == 0) return;

        transform.position = Vector3.MoveTowards(transform.position, points[destPoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, points[destPoint].position) < 0.1f)
        {
            destPoint = (destPoint + 1) % points.Length;
        }
    }

    protected override void Update()
    {
        base.Update();
        GotoNextPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSitting && other.CompareTag("Chair"))
        {
            Chair chair = other.GetComponentInParent<Chair>();
            if (chair != null && !chair.IsOccupied())
            {
                InteractWithChair(chair);
                StartCoroutine(SitDownAndStandUp(chair));
            }
        }
    }

    private IEnumerator SitDownAndStandUp(Chair chair)
    {
        isSitting = true;
        animator.SetBool("IsSitting", true);
        yield return new WaitForSeconds(3f);
        animator.SetBool("IsSitting", false);
        chair.ClearOccupyingCharacter();
        MoveToRandomPosition();
        isSitting = false;
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 5f;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }
}