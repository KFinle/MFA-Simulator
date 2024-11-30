
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro; // Use this if you're using TextMeshPro InputField

public class InputFieldChecker : MonoBehaviour
{
    public InputField myInputField; // Assign this in the Inspector if you're checking a specific InputField
    public TMP_InputField myTMPInputField; // Use this for TextMeshPro InputField

    void Update()
    {
        // Check if the currently selected GameObject is an InputField
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            // Detect if the selected GameObject is an InputField or TMP_InputField
            InputField currentInputField = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
            TMP_InputField currentTMPInputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

            if (currentInputField != null || currentTMPInputField != null)
            {
                // Check if Enter key is pressed
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    // Simulate pressing the submit button or calling the confirm action
                    Debug.Log("Enter pressed while focused on an InputField!");
                    if (currentInputField != null)
                    {
                        currentInputField.onEndEdit.Invoke("");
                    }
                    else if (currentTMPInputField != null)
                    {
                        currentTMPInputField.onEndEdit.Invoke(""); // Trigger the end edit event for TMP_InputField
                    }
                }
            }
        }
    }
}
