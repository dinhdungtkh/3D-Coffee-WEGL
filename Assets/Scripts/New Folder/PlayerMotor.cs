using UnityEngine;
using UnityEngine.AI;

namespace TKH3DCoffee
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMotor : MonoBehaviour
    {
        public Transform target;
        [SerializeField] 
        protected NavMeshAgent agent;     // Reference to our NavMeshAgent
        [SerializeField]
        private string CurrentState;
        private Animator animator;

        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        public void MoveToPoint(Vector3 point)
        {
            agent.SetDestination(point);
          
        }


        void Update()
        {
            if (target != null)
            {
                MoveToPoint(target.position);
                FaceTarget();
            } 
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        public void ChangeAnimationState(string newState)
        {
            if (CurrentState == newState) return;
            animator.Play(newState);
            CurrentState = newState;
        } 

        public void TempDisable(bool isSitting)
        {
            isSitting = false;
            if (!isSitting)
            {
               
            }
        }
    }
}
