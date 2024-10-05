using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Chair> chairs;
    [SerializeField]
    protected List<Chair> availableChairs;
    [SerializeField]
    private List<Chair> occupiedChairs = new List<Chair>();

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
    public Chair GetRandomAvailableChair()
    {
        UpdateChairStatus(); // Ensure the status of the chairs is up-to-date
        if (availableChairs.Count > 0)
        {
            Chair selectedChair = availableChairs[Random.Range(0, availableChairs.Count)];
            MarkChairAsOccupied(selectedChair);
            return selectedChair;
        }
        return null; // No available chair
    }

    public void MarkChairAsOccupied(Chair chair)
    {
        if (!occupiedChairs.Contains(chair))
        {
            occupiedChairs.Add(chair);
            availableChairs.Remove(chair);
        }
    }

    public void ReleaseChair(Chair chair)
    {
        if (occupiedChairs.Contains(chair))
        {
            occupiedChairs.Remove(chair);
            availableChairs.Add(chair);
        }
    }

    public List<Chair> GetAvailableChairs()
    {
        return availableChairs;
    }
}