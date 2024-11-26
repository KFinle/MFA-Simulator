
using UnityEngine;
using UnityEngine.UI;

public class ButtonWrapper : MonoBehaviour
{
    public LevelManager levelManager;  // Reference to the LevelManager
    public CanvasType canvasType;  // The canvas type to pass
    public InputField code;

    // This method will be called by the button's OnClick()
    public void ShowCanvas()
    {
        levelManager.ShowCanvas(canvasType);  // Pass the CanvasType parameter
    }

    public void Login()
    {
        levelManager.OnTaskInteraction();
    }
    public void ValidateCode()
    {
        levelManager.mfaController.ValidateCode(code.text);
    }
}
