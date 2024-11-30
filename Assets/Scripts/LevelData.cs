using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "MFA Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName; // Name of the level
    public string introText; // Introductory text for the level
    public string endText; // Text displayed upon completion
    public float mfaTimer;
    public float levelTimer;

    public string notepadText;

    public DialogueContent introDialogue;
    public DialogueContent outroDialogue;

    [System.Serializable]
    public class Task
    {
        public string taskDescription; // Description of the task
        public CodeDestination codeDestination; // Where to send the MFA code
        public CodeOrigin codeOrigin;


        public bool requiresInteraction; // Whether interaction is required to generate the code
        public bool mfaTimerEnabled;
        public bool levelTimerEnabled;
        public bool isCompleted = false;
        public bool isFinalTask = false;
        public bool isFirstTask = false;
    public DialogueContent preTaskDialogue;
    public DialogueContent postTaskDialogue;
    }

    public Task[] tasks; // Array of tasks for this level
}