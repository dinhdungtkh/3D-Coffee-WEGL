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
    [SerializeField]
    private Collider chairCollider;

    public void Awake()
    {
        fill = false;
    }
    public Transform getchairPosition() { return this.transform; }
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
        // chairCollider.isTrigger = false;
    }

    public void ClearOccupyingCharacter()
    {
        occupyingCharacter = null;
        fill = false;
        // chairCollider.isTrigger = true;
    }

}