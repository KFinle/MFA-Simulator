using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Dropdown levelDropdown; // Dropdown for selecting levels (visible only to developers)
    public Canvas introCanvas; // Intro screen canvas
    public Canvas endCanvas; // End screen canvas
    public Text introText; // Intro text element
    public Text endText; // End text element
    public Text taskListText; // The UI Text where tasks will be displayed
    public GameObject taskListPanel; // The Panel that holds the task list
    
    
    int levelIndex = 0; // Default to the first level for the player
    int numLevels;

    public Canvas[] canvases;
    public LevelData[] levels; // Array of levels
    private LevelData currentLevel;
    private int currentTaskIndex = 0;

    
    // Dictionary to map CanvasType to actual canvas
    public Dictionary<CanvasType, Canvas> canvasDictionary = new Dictionary<CanvasType, Canvas>();

    public MFAController mfaController; // Reference to MFAController
    public AuthenticatorApp authenticatorApp; // Reference to AuthenticatorApp

    void Start()
    {
        if (levelDropdown != null)
        {
            // Populate the dropdown only for developers
            PopulateDropdown();
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

    void Update()
    {
        if (mfaController.codeValid)
        {
            mfaController.codeValid = false;
            CompleteTask();
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
        currentLevel = levels[levelIndex];

        // Display the intro screen
        introCanvas.gameObject.SetActive(true);
        introText.text = currentLevel.introText;
        endCanvas.gameObject.SetActive(false);

        // Show the task list panel
        taskListPanel.SetActive(true);
        UpdateTaskList();
    }

    public void StartLevelTasks()
    {
        introCanvas.gameObject.SetActive(false);
        StartNextTask();
    }

    void StartNextTask()
    {
        if (currentTaskIndex < currentLevel.tasks.Length)
        {
            var task = currentLevel.tasks[currentTaskIndex];
            Debug.Log("Starting Task: " + task.taskDescription);

            UpdateTaskList(); // Update task list UI

            if (!task.requiresInteraction)
            {
                GenerateCodeForTask(task);
            }
        }
        else
        {
            EndLevel();
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
        switch (task.codeDestination)
        {
            case CodeDestination.Authenticator:
                mfaController.GenerateCode(true); // Use authenticator app
                break;
            case CodeDestination.Email:
                mfaController.GenerateCode(false, CodeDestination.Email);
                break;
            case CodeDestination.Phone:
                mfaController.GenerateCode(false, CodeDestination.Phone); // Standard code generation
                break;
            default:
                Debug.LogError("Unknown code destination: " + task.codeDestination);
                break;
        }

        Debug.Log("Code sent to: " + task.codeDestination);
    }

    public void CompleteTask()
    {

        currentTaskIndex++;
        StartNextTask();
 
    }

    void EndLevel()
    {
        Debug.Log("Level Complete!");
        endCanvas.gameObject.SetActive(true);
        endText.text = currentLevel.endText;

        // Hide task list panel when level ends
        taskListPanel.SetActive(false);
    }

    public void NextLevel()
    {
        currentTaskIndex = 0;
        levelIndex++;
        if (levelIndex != numLevels)
        {

        StartGame();
        }
        else {
            Debug.Log("Out of levels");
        }
    }

    void UpdateTaskList()
    {
        taskListText.text = "Tasks:\n";
        for (int i = 0; i < currentLevel.tasks.Length; i++)
        {
            var task = currentLevel.tasks[i];
            string taskStatus = (i == currentTaskIndex) ? "[Current Task] " : "[Completed] ";
            taskListText.text += taskStatus + task.taskDescription + "\n";
        }
    }
    public void ShowCanvas(CanvasType canvasType)
    {
        SelectCanvas(canvasType);
    }
    void SelectCanvas(CanvasType canvasType)
    {
        // Deactivate all canvases
        foreach (var canvas in canvases)
        {
            canvas.gameObject.SetActive(false);
        }

        // Activate the selected canvas
        if (canvasDictionary.ContainsKey(canvasType))
        {
            canvasDictionary[canvasType].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("CanvasType not found!");
        }
    }
}