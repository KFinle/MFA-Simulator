
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWrapper : MonoBehaviour
{
    public LevelManager levelManager;  // Reference to the LevelManager
    public CanvasType canvasType;  // The canvas type to pass

    public GameObject codeValidation;
    

    void Start()
    {
        
        levelManager = FindObjectOfType<LevelManager>(); // Find the LevelManager in the scene
    }
    [Header("Optional Fields")]
    public InputField code;
    public GameObject mfaInstructions;
    private string mfaInstructionsText;

    // This method will be called by the button's OnClick()
    public void ShowCanvas()
    {
        levelManager.ShowCanvas(canvasType);  // Pass the CanvasType parameter

    }

    public void Login()
    {
        levelManager.OnTaskInteraction();
        if (mfaInstructions != null)
        {
            mfaInstructionsText = levelManager.mfaController.instructions;
            SetInstructionText(mfaInstructionsText);
        }       
    }
    public void ValidateCode()
    {
        levelManager.mfaController.ValidateCode(code.text);
        if (!levelManager.mfaController.codeValid)
        {
            SetInstructionText("WRONG CODE. DO IT AGAIN. NOW.");
        }
    }

    public void SetInstructionText(string newText)
    {
        mfaInstructions.GetComponent<TextMeshProUGUI>().text = newText;
        mfaInstructions.SetActive(true);
    }
    public void PrintDebug()
    {
        Debug.Log("Clicked!");
    }

    public void ToggleMFAInputBox()
    {
        if(!codeValidation.activeSelf)
        {
            codeValidation.SetActive(true);
        }
        else 
        {
            codeValidation.SetActive(false);
        }
    }

    public void SetMFAInputBoxOn()
    {
        if(!codeValidation.activeSelf)
        {
            codeValidation.SetActive(true);
        }
    }
    
    public void SetMFAInputBoxOff()
    {
        if(codeValidation.activeSelf)
        {
            codeValidation.SetActive(false);
        }
    }
}
