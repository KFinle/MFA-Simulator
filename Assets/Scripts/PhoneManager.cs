using TMPro;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] ButtonWrapper mfaButtonWrapper;

    public bool messagesNeedMFA = false;
    public bool callNeedsAuthentication = false;
    public TextMeshProUGUI instructions;
    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }
    void Update()
    {
        if (messagesNeedMFA && levelManager.CanvasIsActive(CanvasType.PhoneMessagingApp))
        {
            instructions.text = "Messaging app has updated. Get a code from MFA app.";
            mfaButtonWrapper.codeOrigin = CodeOrigin.TextApp;
            mfaButtonWrapper.targetCanvasType = CanvasType.PhoneMessagingApp;
            levelManager.ShowCanvas(CanvasType.PhoneMFACheck);
        }
        if (callNeedsAuthentication && levelManager.CanvasIsActive(CanvasType.PhoneCallApp))
        {
            instructions.text = "Check your email. It's your only option.";
            mfaButtonWrapper.codeOrigin = CodeOrigin.CallApp;
            mfaButtonWrapper.targetCanvasType = CanvasType.PhoneCallApp;
            mfaButtonWrapper.GenerateCode();
            levelManager.ShowCanvas(CanvasType.PhoneMFACheck);
        }

    }
}

