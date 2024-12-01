using UnityEngine;

public class PCManager : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] ButtonWrapper mfaButtonWrapper;

    public bool pcNeedsMFA = false;
    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }
    void Update()
    {
        if (pcNeedsMFA && levelManager.CanvasIsActive(CanvasType.ComputerHome))
        {
            mfaButtonWrapper.codeOrigin = CodeOrigin.PCLogin;
            mfaButtonWrapper.targetCanvasType = CanvasType.ComputerHome;
            levelManager.ShowCanvas(CanvasType.ComputerLogin);
        }
    }
}

