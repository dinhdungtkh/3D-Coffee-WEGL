using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TKH3DCoffee
{
    public class CharacterAnimator : MonoBehaviour
    {
        public Animator animator;
        const float lomotionAnimationSmoothTime = .1f;
        NavMeshAgent navmeshAgent;

        protected virtual void Start()
        {
            navmeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            float speedPercent = navmeshAgent.velocity.magnitude / navmeshAgent.speed;

            animator.SetFloat("SpeedPercent", speedPercent, lomotionAnimationSmoothTime, Time.deltaTime);
        }

    }
}
