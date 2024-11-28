
using UnityEngine;
using UnityEngine.UI;

public class ButtonWrapper : MonoBehaviour
{
    public LevelManager levelManager;  // Reference to the LevelManager
    public CanvasType canvasType;  // The canvas type to pass
    

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
            mfaInstructions.GetComponent<Text>().text = mfaInstructionsText;
            mfaInstructions.SetActive(true);
        }       
    }
    public void ValidateCode()
    {
        levelManager.mfaController.ValidateCode(code.text);
    }

    
}
