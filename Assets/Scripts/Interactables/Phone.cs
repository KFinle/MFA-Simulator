
using UnityEngine;

public class Phone : MonoBehaviour, IInteractable
{
    public void Interact(LevelManager levelManager)
    {
        // Trigger the LevelManager to display the Phone canvas
        levelManager.ShowCanvas(CanvasType.PhoneHome);
        Debug.Log("You interacted with the phone.");
    }
    public void CancelInteraction(LevelManager levelManager)
    {
        Debug.Log("Interaction with ComputerHome ended");
        levelManager.ShowCanvas(CanvasType.GameplayCanvas);
    }   
}
