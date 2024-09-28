using UnityEngine;
using UnityEngine.AI;
using System.Collections;


namespace TKH3DCoffee
{
    public class NonPlayableCharController : MonoBehaviour
    {
        public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;
        public Vector3 velocity;
        public Animator animator;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // Kiểm tra xem agent có được đặt trên NavMesh không
            if (!agent.isOnNavMesh)
            {
                Debug.LogError("Agent is not on NavMesh!");
                return; // Dừng nếu agent không trên NavMesh
            }

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            GotoNextPoint();
        }


        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (points.Length == 0 || !agent.isActiveAndEnabled)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }


        void Update()
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
            velocity = agent.velocity;
            if (velocity.z != 0 || velocity.x != 0) { animator.SetBool("IsMoving", true); }
        }
    }
}
