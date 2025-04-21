using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonWrapper : MonoBehaviour
{
    public LevelManager levelManager; // Reference to the LevelManager
    public CanvasType targetCanvasType;     // The canvas type to pass
    public GameObject codeValidation; // MFA input box container

    [Header("Optional Fields")]
    public InputField code;               // Input field for MFA code
    public GameObject mfaInstructions;    // MFA instructions UI element
    private string mfaInstructionsText;   // Cached instructions text
    public CodeOrigin codeOrigin;         // The CodeOrigin to use for generating and validating codes


    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>(); // Find the LevelManager in the scene
    }

    // Show a specific canvas via the LevelManager
    public void ShowCanvas()
    {
        Debug.Log($"Target canvas: {targetCanvasType}");
        levelManager.ShowCanvas(targetCanvasType);
    }

    // Generate a code for the specified CodeOrigin
    public void GenerateCode()
    {
        if (codeOrigin != CodeOrigin.None)
        {
            levelManager.mfaController.GenerateCodeFromOrigin(codeOrigin);
            if (mfaInstructions != null)
            {
                mfaInstructionsText = levelManager.mfaController.instructions; // Retrieve instructions
                SetInstructionText(mfaInstructionsText);
            }
        }
        else
        {
            Debug.LogError("CodeOrigin is not set.");
        }
    }

    // Validate the entered MFA code
    public void ValidateCode()
    {
        if (codeOrigin != CodeOrigin.None)
        {
            levelManager.mfaController.ValidateCode(codeOrigin, code.text);

            if (levelManager.mfaController.codeValid)
            {
                SetInstructionText("SUCCESS! Code is valid.");
                SetInstructionText("");
                code.text = "";
                SetMFAInputBoxOff();
                if (codeOrigin == CodeOrigin.PCLogin) levelManager.pcManager.pcNeedsMFA = false;
                ShowCanvas();
            }
            else
            {
                SetInstructionText("WRONG CODE. DO IT AGAIN. NOW.");
            }
        }
        else
        {
            Debug.LogError("CodeOrigin is not set.");
        }
    }

    // Update the MFA instructions text
    public void SetInstructionText(string newText)
    {
        if (mfaInstructions != null)
        {
            mfaInstructions.GetComponent<TextMeshProUGUI>().text = newText;
            mfaInstructions.SetActive(true);
        }
    }

    // Print a debug message
    public void PrintDebug()
    {
        Debug.Log("Clicked!");
    }

    // Toggle the visibility of the MFA input box
    public void ToggleMFAInputBox()
    {
        if (codeValidation != null)
        {
            codeValidation.SetActive(!codeValidation.activeSelf);
        }
    }

    // Ensure the MFA input box is visible
    public void SetMFAInputBoxOn()
    {
        if (codeValidation != null && !codeValidation.activeSelf)
        {
            codeValidation.SetActive(true);
        }
    }

    // Ensure the MFA input box is hidden
    public void SetMFAInputBoxOff()
    {
        if (codeValidation != null && codeValidation.activeSelf)
        {
            codeValidation.SetActive(false);
        }
    }
    
    public void LaunchGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }
    public void ValidatePhoneMFA()
    {
        if (codeOrigin != CodeOrigin.None)
        {
            levelManager.mfaController.ValidateCode(codeOrigin, code.text);

            if (levelManager.mfaController.codeValid)
            {
                SetInstructionText("SUCCESS! Code is valid.");
                // Optionally, notify the LevelManager of task completion if needed
                code.text = "";
                if (codeOrigin == CodeOrigin.TextApp) FindFirstObjectByType<PhoneManager>().messagesNeedMFA = false;
                if (codeOrigin == CodeOrigin.CallApp) FindFirstObjectByType<PhoneManager>().callNeedsAuthentication = false;
                //SetMFAInputBoxOff();
                ShowCanvas();
            }
            else
            {
                SetInstructionText("WRONG CODE. DO IT AGAIN. NOW.");
            }
        }
        else
        {
            Debug.LogError("CodeOrigin is not set.");
        }
    }
}
