using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
     [SerializeField]
    private   int IsMovingBoolHash = Animator.StringToHash("isMoving");
    [SerializeField]
    private   int SitAnimationTriggerHash = Animator.StringToHash("sit");
     [SerializeField]
    private   int StandAnimationTriggerHash = Animator.StringToHash("stand");
     [SerializeField]
    private   int IdleTalkAnimationTriggerHash = Animator.StringToHash("idleTalk");
    [SerializeField]
    private string CurrentState;
    private void Awake()
    {
       //   animator = GetComponent<Animator>();
    }

    public void SetMoving(bool isMoving)
    {
        animator.SetBool(IsMovingBoolHash, isMoving);
        Debug.Log(IsMovingBoolHash);
        Debug.Log(isMoving);
        SetTriggerAnim(IsMovingBoolHash);
      
    }

    public void Sit()
    {
        SetTriggerAnim(SitAnimationTriggerHash);
    }

    public void Stand()
    {
        SetTriggerAnim(StandAnimationTriggerHash);
    }

    public void IdleTalk()
    {
        SetTriggerAnim(IdleTalkAnimationTriggerHash);
    }

    private void SetTriggerAnim(int triggerHash)
    {
        animator.SetTrigger(triggerHash);
    }

    public void ChangeAnimationState( string newState)
    {
        if (CurrentState == newState) return; 
        animator.Play(newState);
        CurrentState =  newState;
    }
   
}
