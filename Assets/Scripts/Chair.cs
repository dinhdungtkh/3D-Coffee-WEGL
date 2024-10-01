using System.Collections;
using System.Collections.Generic;
using TKH3DCoffee;
using UnityEditor.UI;
using UnityEngine;

public class Chair : MonoBehaviour
{

    protected bool isEmpty;
    [SerializeField]
    private Transform chairFrontposition;
   
    public Transform chairPosition;
    [SerializeField]
    public bool fill;
    private Character occupyingCharacter;

    public void Awake()
    {
        fill = false;
    }
    public  Transform getchairPosition() {  return this.transform; }
    public Transform getchairFrontPosition()
    {
        return chairFrontposition;
    }
    public bool IsOccupied()
    {
        return fill;
    }

    public void SetOccupyingCharacter(Character character)
    {
        occupyingCharacter = character;
        fill = true; 
    }

    public void ClearOccupyingCharacter()
    {
        occupyingCharacter = null;
        fill = false; 
    }

}
