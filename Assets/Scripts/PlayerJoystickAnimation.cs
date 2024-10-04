using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystickAnimation : MonoBehaviour
{
   
    [SerializeField] private PLayerMoveJoystick moveJoystick;
    [SerializeField] private CharacterController character;
    //[SerializeField] private float animSmoothTime = 0.03f;
    private void StatusAnim()
    {
       
    }
    public void Sitdown()
    {
    }
    public void StandUp()
    {
    }
    private void Update()
    {
        StatusAnim();
    }
}
