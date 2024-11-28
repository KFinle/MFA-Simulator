
using UnityEngine;
using UnityEngine.UI;

public class MFAController : MonoBehaviour
{
    [Header("UI Elements")]
    public Text feedbackText; // Feedback for validation
    public Text phoneText;
    public Text emailText;
    public bool codeValid = false;

    private string currentGeneratedCode; // Active code generated
    private bool codeGenerated = false;

    public AuthenticatorApp authenticatorApp; // Reference to the authenticator app
    public CodeDestination codeDestination;

    public MessageManager messageManager;
public string instructions = "";
    public void GenerateCode(bool useAuthenticatorApp, CodeDestination destination = CodeDestination.Authenticator)
    {
        if (useAuthenticatorApp)
        {
            // Use the current code from the authenticator app
            currentGeneratedCode = authenticatorApp.GetCurrentCode();
        }
        else
        {
            // Generate a standard random code
            currentGeneratedCode = Random.Range(100000, 999999).ToString();
            
            if (destination == CodeDestination.Email)
            {
                emailText.text = currentGeneratedCode;
                instructions = "Please check the code WE sent to your email.";
            }
            else 
            {
                phoneText.text = currentGeneratedCode;
                instructions = "Please check the code WE sent to your phone.";
                messageManager.AddMessage("Your code: " + currentGeneratedCode);
            }
        }

        codeGenerated = true;
        Debug.Log("Generated MFA Code: " + currentGeneratedCode);
    }

    public void ValidateCode(string input)
    {
        if (!codeGenerated)
        {
            // feedbackText.text = "Error: No code requested!";
            Debug.Log("Validation failed: No code generated.");
            return;
        }

        if (input == currentGeneratedCode)
        {
            // feedbackText.text = "Access Granted!";
            Debug.Log("Validation success.");
            codeGenerated = false; // Reset after successful validation
            codeValid = true;
        }
        else
        {
            // feedbackText.text = "Error: Invalid Code!";
            Debug.Log("Validation failed: Wrong code.");
        }
    }

    public void ResetCode()
    {
        currentGeneratedCode = null;
        codeGenerated = false;
    }
}
