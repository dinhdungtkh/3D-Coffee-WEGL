using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Chair> chairs;
    [SerializeField]
    protected List<Chair> availableChairs;

    private void Awake()
    {
        availableChairs = new List<Chair>(chairs);
    }

    public void UpdateChairStatus()
    {
        availableChairs.Clear();

        foreach (Chair chair in chairs)
        {
            if (!chair.IsOccupied())
            {
                availableChairs.Add(chair);
            }
        }
    }

    public List<Chair> GetAvailableChairs()
    {
        return availableChairs;
    }
}