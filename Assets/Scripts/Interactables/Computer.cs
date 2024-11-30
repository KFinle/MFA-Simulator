
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    public void Interact(LevelManager levelManager)
    {
        // Trigger the LevelManager to display the canvas corresponding to the "PC"
        levelManager.ShowCanvas(CanvasType.ComputerHome);
        Debug.Log("You interacted with the computer.");
    }
    public void CancelInteraction(LevelManager levelManager)
    {
        Debug.Log("Interaction with ComputerHome ended");
        levelManager.ShowCanvas(CanvasType.GameplayCanvas);
    }
}

