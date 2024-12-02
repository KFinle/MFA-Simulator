
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "MFA Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName; // Name of the level
    public string introText; // Introductory text for the level
    public string endText; // Text displayed upon completion
    public float mfaTimer; // Timer for MFA code expiration
    public float levelTimer; // Timer for overall level completion

    public string notepadText; // Text to be displayed on the in-game notepad

    public DialogueContent introDialogue; // Dialogue content for the intro
    public DialogueContent outroDialogue; // Dialogue content for the outro
    public bool messagesNeedMFA = false;
    public bool callsNeedsMFA = false;
    public bool phoneNeedsMFA = false;
    public bool startsWithEmail = false;
    public bool startsWithText = false;
    public string introTextMessage;
    public bool pcNeedsMFA = false;
    public string introEmailMessage;

    [System.Serializable]
    public class Task
    {
        public string taskDescription; // Description of the task
        public CodeDestination codeDestination; // Where to send the MFA code (Email, Phone, Authenticator)
        public CodeOrigin codeOrigin; // Origin of the MFA request

        public bool requiresInteraction; // Whether player interaction is needed to generate the code
        public bool mfaTimerEnabled; // If the MFA timer is enabled for this task
        public bool levelTimerEnabled; // If the level timer is enabled for this task
        public bool isCompleted = false; // Whether the task is completed
        public bool isFinalTask = false; // Marks the task as the final one in the level
        // public bool isFirstTask = false; // Marks the task as the first one in the level


        public DialogueContent preTaskDialogue; // Dialogue before the task starts
        public DialogueContent postTaskDialogue; // Dialogue after the task completes
        public bool sendsText = false;
        public bool sendsEmail = false;
        public string textMessage;
        public string email;
        public bool taskTextAppNeedsMFA = false;

        public bool taskCallAppNeedsMFA = false;
    }

    public Task[] tasks; // Array of tasks for this level
}
