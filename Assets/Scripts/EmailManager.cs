using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class EmailManager : MonoBehaviour
{
    public GameObject emailPrefab; // Assign your message prefab
    public Transform contentTransform; // Assign the Content object from the ScrollView
    public ScrollRect scrollRect; // Explicitly assign the ScrollRect component

    public LevelManager levelManager;
    private int testInt = 0;

    public bool unreadEmail = false;
    public Canvas inboxPage;
    
    public Image emailNotificationIcon;


    void Start()
    {
        // Scroll to the bottom on start
        ScrollToBottom();
    }
    public void AddMessage(string messageText)
    {
        unreadEmail = true;
        // Instantiate a new message bubble
        GameObject newMessage = Instantiate(emailPrefab, contentTransform);

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
            scrollRect.verticalNormalizedPosition = 1f; // Scrolls to the bottom
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
        
        if (inboxPage.gameObject.activeSelf)
        {
            unreadEmail = false;
        }
        
        if (unreadEmail && emailNotificationIcon != null)
        {
            if (emailNotificationIcon.gameObject.activeSelf == false) emailNotificationIcon.gameObject.SetActive(true);
        }
        if (!unreadEmail && emailNotificationIcon != null )
        {
            if (emailNotificationIcon.gameObject.activeSelf) emailNotificationIcon.gameObject.SetActive(false);

        }

    }
}
