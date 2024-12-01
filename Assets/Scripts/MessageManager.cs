
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class MessageManager : MonoBehaviour
{
    public GameObject messagePrefab; // Assign your message prefab
    public Transform contentTransform; // Assign the Content object from the ScrollView
    public ScrollRect scrollRect; // Explicitly assign the ScrollRect component

    public LevelManager levelManager;
    private int testInt = 0;

    public bool unreadText = false;
    public Canvas messageApp;
    
    public Light2D textNotificationLight;


    void Start()
    {
        // Scroll to the bottom on start
        ScrollToBottom();
    }
    public void AddMessage(string messageText)
    {
        unreadText = true;
        // Instantiate a new message bubble
        GameObject newMessage = Instantiate(messagePrefab, contentTransform);

        // Set the message text
        TMP_Text messageTextComponent = newMessage.GetComponentInChildren<TMP_Text>();
        if (messageTextComponent != null)
        {
            messageTextComponent.text = messageText;
        }

        // Force the layout to update
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform.GetComponent<RectTransform>());

        // Scroll to the bottom
        Canvas.ForceUpdateCanvases();
        ScrollToBottom();
    }


    private void ScrollToBottom()
    {
        // Ensure the ScrollRect is properly assigned
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases(); // Forces UI updates
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform.GetComponent<RectTransform>());
            scrollRect.verticalNormalizedPosition = 0f; // Scrolls to the bottom
        }
        else
        {
            Debug.LogWarning("ScrollRect is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMessage("Test #" + testInt);
            testInt++;
        }
        
        if (messageApp.gameObject.activeSelf)
        {
            unreadText = false;
        }
        
        if (unreadText && textNotificationLight != null)
        {
            if (textNotificationLight.gameObject.activeSelf == false) textNotificationLight.gameObject.SetActive(true);
        }
        if (!unreadText && textNotificationLight != null )
        {
            if (textNotificationLight.gameObject.activeSelf) textNotificationLight.gameObject.SetActive(false);

        }

    }
}

