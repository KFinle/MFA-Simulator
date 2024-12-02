using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Dropdown levelDropdown; // Dropdown for selecting levels (visible only to developers)
    public Canvas introCanvas; // Intro screen canvas
    public Canvas endCanvas; // End screen canvas
    public TextMeshProUGUI introText; // Intro text element
    public TextMeshProUGUI endText; // End text element
    public Text taskListText; // The UI Text where tasks will be displayed
    public GameObject taskListPanel; // The Panel that holds the task list
    public Text notePadText;

    public int levelIndex = 0; // Default to the first level for the player
    public int numLevels;

    public Canvas[] canvases;
    public LevelData[] levels; // Array of levels
    public LevelData currentLevel;
    [HideInInspector] public int currentTaskIndex = 0;

    // Dictionary to map CanvasType to actual canvas
    public Dictionary<CanvasType, Canvas> canvasDictionary = new Dictionary<CanvasType, Canvas>();

    public MFAController mfaController; // Reference to MFAController
    public AuthenticatorApp authenticatorApp; // Reference to AuthenticatorApp

    public MessageManager messageManager;
    public EmailManager emailManager;
    public DialogueController dialogueController;
    public PhoneManager phoneManager;

    public PCManager pcManager;

    private Canvas canvasPointer;
    private PlayerController player;
    private bool levelActive = false;
    public bool endingTriggered = false;
    Coroutine gameOverRoutine = null;
    bool gameOver = false;




    void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Start()
    {
        if (levelDropdown != null)
        {
            // Populate the dropdown only for developers
            PopulateDropdown();
            levelDropdown.onValueChanged.AddListener(OnDropdownValueChanged); // Listen for dropdown changes
            levelDropdown.gameObject.SetActive(false); // Hide the dropdown for players during gameplay
        }

        // Initialize the dictionary and map each canvas type to its corresponding canvas
        canvasDictionary = new Dictionary<CanvasType, Canvas>();

        for (int i = 0; i < canvases.Length; i++)
        {
            CanvasType canvasType;
            if (System.Enum.TryParse(canvases[i].name, out canvasType))
            {
                canvasDictionary[canvasType] = canvases[i];
            }
            else
            {
                Debug.LogWarning($"Canvas name {canvases[i].name} doesn't match any enum type.");
            }
        }
        // Start the game with the first level
        numLevels = levels.Length;
        StartGame();
    }

    void OnDropdownValueChanged(int selectedIndex)
    {
        if (selectedIndex != levelIndex && selectedIndex >= 0 && selectedIndex < levels.Length)
        {
            EndLevel(); // End the current level before switching
            levelIndex = selectedIndex; // Set the new level index
            StartGame(); // Start the newly selected level
        }
    }

    void Update()
    {
        if (gameOver)
        {
            if (gameOverRoutine == null)
            {
                gameOverRoutine = StartCoroutine(GameOverRoutine());
            }
        }
        // Check if an MFA code has been validated
        if (mfaController.codeValid)
        {
            var currentTask = currentLevel.tasks[currentTaskIndex];

            // Check if the current task requires a specific code origin and is present in validated codes
            if (currentTask.codeOrigin == CodeOrigin.None ||
                !mfaController.validatedCodes.ContainsKey(currentTask.codeOrigin))
            {
                mfaController.codeValid = false;
                return;
            }

            // Reset code validation flag after checking
            mfaController.codeValid = false;

            // Complete the task
            CompleteTask();
        }

        // Existing logic for generating codes and managing canvases
        if (levelActive && currentLevel.tasks[currentTaskIndex].codeDestination == CodeDestination.Authenticator)
        {
            //GenerateCodeForTask(currentLevel.tasks[currentTaskIndex]);
        }

        if (!dialogueController.inIntro && CanvasIsActive(CanvasType.IntroCanvas))
        {
            StartLevelTasks();
        }

        if (!endingTriggered && CanvasIsActive(CanvasType.CompletionCanvas))
        {
            ShowCanvas(CanvasType.GameplayCanvas);
        }
        
        if (!levelActive && !dialogueController.inDialogue && !endingTriggered)
        {
            EndLevel();
        }
    }

    void PopulateDropdown()
    {
        levelDropdown.ClearOptions();
        foreach (var level in levels)
        {
            levelDropdown.options.Add(new Dropdown.OptionData(level.levelName));
        }
        levelDropdown.RefreshShownValue();
    }

    public void StartGame()
    {
        endingTriggered = false;
        levelActive = true;
        player.SpawnPlayer();
        currentLevel = levels[levelIndex];
        
        if (currentLevel.startsWithEmail)
        {
            emailManager.AddMessage(currentLevel.introEmailMessage);
        }
        
        if (currentLevel.startsWithText)
        {
            messageManager.AddMessage(currentLevel.introTextMessage);
        }


        InitTaskList();
        if (notePadText != null)
        {
            notePadText.text = currentLevel.notepadText;
        }
        introText.text = currentLevel.introText;
        UpdateTaskList();
        
        pcManager.pcNeedsMFA = currentLevel.pcNeedsMFA;
        
        phoneManager.messagesNeedMFA = currentLevel.messagesNeedMFA;
        phoneManager.callNeedsAuthentication = currentLevel.callsNeedsMFA;

        dialogueController.inIntro = true;
        dialogueController.DisplayNextParagraph(currentLevel.introDialogue);

        ShowCanvas(CanvasType.IntroCanvas);
    }

    public void StartLevelTasks()
    {
        ShowCanvas(CanvasType.GameplayCanvas);
        StartNextTask();
    }

    void StartNextTask()
    {
        if (currentTaskIndex < currentLevel.tasks.Length)
        {
            mfaController.validatedCodes.Clear();
            mfaController.ResetAllCodes();
            var task = currentLevel.tasks[currentTaskIndex];
            if (task.sendsText)
            {
                messageManager.AddMessage(task.textMessage);
            }
            
            if (task.sendsEmail)
            {
                emailManager.AddMessage(task.email);
            }
            if (task.taskTextAppNeedsMFA)
            {
                phoneManager.messagesNeedMFA = true;
            }
            if (task.taskCallAppNeedsMFA)
            {
                phoneManager.generatedPhoneCode = false;
                phoneManager.callNeedsAuthentication = true;
            }

            if (task.preTaskDialogue != null)
            {
                dialogueController.DisplayNextParagraph(task.preTaskDialogue);
            }
            Debug.Log("Starting Task: " + task.taskDescription);

            UpdateTaskList(); // Update task list UI

            if (!task.requiresInteraction)
            {
                GenerateCodeForTask(task);
            }
        }
        else
        {
            levelActive = false;
        }
    }

    public void OnTaskInteraction()
    {
        var task = currentLevel.tasks[currentTaskIndex];
        if (task.requiresInteraction)
        {
            GenerateCodeForTask(task);
        }
    }

    void GenerateCodeForTask(LevelData.Task task)
    {
        // Call the fixed GenerateCodeFromOrigin method
        mfaController.GenerateCodeFromOrigin(task.codeOrigin);

        Debug.Log($"Code generated for task: {task.taskDescription}, sent to {task.codeDestination} from {task.codeOrigin}");
    }

    public void CompleteTask()
    {
        // Reset the MFA code after the task is completed
        mfaController.ResetCode(currentLevel.tasks[currentTaskIndex].codeOrigin);

        if (currentLevel.tasks[currentTaskIndex].postTaskDialogue != null)
        {
            dialogueController.DisplayNextParagraph(currentLevel.tasks[currentTaskIndex].postTaskDialogue);
        }

        currentLevel.tasks[currentTaskIndex].isCompleted = true;
        currentTaskIndex++;
        StartNextTask();
    }

    void EndLevel()
    {

        endingTriggered = true;
        Debug.Log("Level Complete!");
        SelectCanvas(CanvasType.CompletionCanvas);
        endText.text = currentLevel.endText;

        StartCoroutine(FadeOutAndNextLevel());
    }

    public void NextLevel()
    {
        currentTaskIndex = 0;
        levelIndex++;
        if (levelIndex < numLevels)
        {
            StartGame();
        }
        else
        {
            Debug.Log("Out of levels");
            gameOver = true;
            

        }
    }

    void UpdateTaskList()
    {
        taskListText.text = "Tasks:\n";
        for (int i = 0; i < currentLevel.tasks.Length; i++)
        {
            var task = currentLevel.tasks[i];
            string taskStatus = (i == currentTaskIndex) ? "[Current Task] " : "[Completed] ";

            taskListText.text = taskStatus + task.taskDescription + "\n";

            if (i == currentTaskIndex && !task.isCompleted)
            {
                break;
            }
        }
    }

    public void ShowCanvas(CanvasType canvasType)
    {
        SelectCanvas(canvasType);
    }

    void SelectCanvas(CanvasType canvasType)
    {
        foreach (var canvas in canvases)
        {
            canvas.gameObject.SetActive(false);
        }

        if (canvasDictionary.ContainsKey(canvasType))
        {
            canvasDictionary[canvasType].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("CanvasType not found!");
        }
    }

    void InitTaskList()
    {
        foreach (var task in currentLevel.tasks)
        {
            task.isCompleted = false;
        }
    }

    public bool CanvasIsActive(CanvasType canvasType)
    {
        if (canvasDictionary.ContainsKey(canvasType))
        {
            return canvasDictionary[canvasType].gameObject.activeSelf;
        }
        return false;
    }

    public CodeOrigin GetCurrentTaskOrigin()
    {
        if (currentTaskIndex >= 0 && currentTaskIndex < currentLevel.tasks.Length)
        {
            return currentLevel.tasks[currentTaskIndex].codeOrigin;
        }
        Debug.LogError("Invalid task index.");
        return CodeOrigin.None; // Default or fallback value
    }
    



        public Image fadeImage; // The image used for the fade effect
        public float fadeDuration = 2f; // Duration of the fade
        IEnumerator FadeOutAndNextLevel()
    {

        // Start fading in the image
        float elapsedTime = 0f;
        Color originalColor = fadeImage.color;
        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        fadeImage.gameObject.SetActive(true);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully opaque before proceeding
        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        Debug.Log("Called next level");
        // Proceed to the next level after fading
        
        NextLevel();
}


public Image gameOverImage;

IEnumerator GameOverRoutine()
{
    ShowCanvas(CanvasType.GameOverCanvas);
        // Start fading in the image
        float elapsedTime = 0f;
        Color originalColor = gameOverImage.color;
        gameOverImage.color = new Color(0f, 0f, 0f, 1f);
        gameOverImage.gameObject.SetActive(true);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            gameOverImage.color = new Color(alpha, alpha, alpha, 1f);
            yield return null;
        }
        yield return new WaitForSeconds(fadeDuration);

        // Ensure it's fully opaque before proceeding
        gameOverImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        Debug.Log("Ended game");
        // ApplicationManager.QuitGame();
        SceneManager.LoadScene("HomeScene");
        
}

}