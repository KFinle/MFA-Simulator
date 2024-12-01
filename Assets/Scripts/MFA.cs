using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MFAController : MonoBehaviour
{
    [Header("UI Elements")]
    public Text feedbackText; // Feedback for validation
    public Text phoneText;
    public Text emailText;
    public bool codeValid = false;

    public AuthenticatorApp authenticatorApp; // Reference to the authenticator app
    public MessageManager messageManager;
    public EmailManager emailManager;

    public Dictionary<CodeOrigin, string> activeCodes = new Dictionary<CodeOrigin, string>();
    // public Dictionary<CodeOrigin, CodeDestination> codePairings = new Dictionary<CodeOrigin, CodeDestination>();
    public Dictionary<CodeOrigin, string> validatedCodes = new Dictionary<CodeOrigin, string>(); 

    public Dictionary<CodeOrigin, CodeDestination> codePairings = new Dictionary<CodeOrigin, CodeDestination>
    {
        {CodeOrigin.StudentPortal, CodeDestination.Phone},
        {CodeOrigin.Email, CodeDestination.Phone},
        {CodeOrigin.TextApp, CodeDestination.Authenticator},
        {CodeOrigin.CallApp, CodeDestination.Email},
        {CodeOrigin.PCLogin, CodeDestination.Phone}
    };
    public string instructions = "";


public void GenerateCode(CodeOrigin origin, bool useAuthenticatorApp = false, CodeDestination destination = CodeDestination.Authenticator)
{
    if (destination == CodeDestination.Authenticator)
    {
        // No static code generation; rely on AuthenticatorApp
        Debug.Log($"Authenticator app linked to {origin}. Use the app's current code.");
        instructions = "Use the code displayed in the Authenticator app.";
    }
    else
    {
        string generatedCode = Random.Range(100000, 999999).ToString();
        activeCodes[origin] = generatedCode;

        if (destination == CodeDestination.Email)
        {
            emailText.text = $"Code from {origin}: {generatedCode}";
            instructions = "Please check the code WE sent to your email.";
            emailManager.AddMessage(emailText.text);
        }
        else if (destination == CodeDestination.Phone)
        {
            phoneText.text = $"Code from {origin}: {generatedCode}";
            instructions = "Please check the code WE sent to your phone.";
            messageManager.AddMessage(phoneText.text);
        }

        Debug.Log($"Generated MFA Code for {origin}: {generatedCode}");
    }

    PrintAllCodes();
}

public void ValidateCode(CodeOrigin origin, string input)
{
    if (codePairings.TryGetValue(origin, out CodeDestination destination) && destination == CodeDestination.Authenticator)
    {
        // Check the authenticator app's current code
        string currentAuthCode = authenticatorApp.GetCurrentCode();
        if (currentAuthCode == input)
        {
            Debug.Log($"Validation success for {origin} using Authenticator App.");
            validatedCodes[origin] = currentAuthCode;
            codeValid = true;
        }
        else
        {
            Debug.Log($"Validation failed: Wrong code for {origin} using Authenticator App.");
        }
    }
    else if (activeCodes.ContainsKey(origin))
    {
        if (activeCodes[origin] == input)
        {
            Debug.Log($"Validation success for {origin}.");
            validatedCodes[origin] = activeCodes[origin];
            activeCodes.Remove(origin);
            codeValid = true;
        }
        else
        {
            Debug.Log($"Validation failed: Wrong code for {origin}.");
        }
    }
    else
    {
        Debug.Log($"Validation failed: No code generated for {origin}.");
    }
    instructions = "";
}
    // Reset a specific origin's code
    public void ResetCode(CodeOrigin origin)
    {
        if (activeCodes.ContainsKey(origin))
        {
            activeCodes.Remove(origin);
            Debug.Log($"Code for {origin} has been reset.");
        }
    }

    // Reset all codes
    public void ResetAllCodes()
    {
        activeCodes.Clear();
        Debug.Log("All codes have been reset.");
    }
    

public void GenerateCodeFromOrigin(CodeOrigin origin)
{
    if (codePairings.TryGetValue(origin, out CodeDestination destination))
    {
        bool useAuthenticatorApp = destination == CodeDestination.Authenticator;
        GenerateCode(origin, useAuthenticatorApp, destination);
    }
    else
    {
        Debug.LogError("No pairing found for the specified origin: " + origin);
    }
}


public void PrintAllCodes()
{
    foreach (var pairing in activeCodes)
    {
        Debug.Log($"CodeOrigin: {pairing.Key}, CodeDestination: {codePairings[pairing.Key]}, Code: {pairing.Value}");
    }
}
    public void ClearValidatedCodes()
    {
        validatedCodes.Clear();
    }
}

