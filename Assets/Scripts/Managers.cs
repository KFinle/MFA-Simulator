using UnityEngine;

public class Managers : MonoBehaviour
{
public MFAController mfaController;
public LevelManager levelManager;
public ApplicationManager appManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() 
    {
        if (mfaController==null)
        {
            mfaController = GetComponentInChildren<MFAController>();
        }
        if (levelManager == null)
        {
            levelManager = GetComponentInChildren<LevelManager>();
        }
        if (appManager == null)
        {
            appManager = GetComponentInChildren<ApplicationManager>();

        }
    }
    void Start()
    {
    DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
