using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Bed : MonoBehaviour, IInteractable
{
    public void Interact(LevelManager levelManager)
    {
        if (levelManager.currentLevel.tasks[levelManager.currentTaskIndex].isFinalTask == true)
        {
            levelManager.CompleteTask();
        }
        else 
        {

            FindFirstObjectByType<PlayerController>().isInteracting = false;
            FindFirstObjectByType<PlayerController>().interactableObject = null;
            StartCoroutine(FlashText("It's not time for bed yet!", levelManager.taskListText.text, levelManager.taskListText, 2));
        }

    }
    public void CancelInteraction(LevelManager levelManager)
    {

    }

    IEnumerator FlashText(string flashText, string previousText, Text textBox, float seconds)
    {
        textBox.text = flashText;
        yield return new WaitForSeconds(seconds);
        textBox.text = previousText;
    }
}