using System.Collections;
using System.Collections.Generic;
using TKH3DCoffee;
using UnityEngine;

public class Chair : Interactable
{

    protected bool isEmpty;
    [SerializeField]
    private Transform chairFrontPosition;

    public  Transform chairPosition() {  return chairFrontPosition; }
}
