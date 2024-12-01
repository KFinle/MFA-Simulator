using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI speakerNameText;
    [SerializeField] public TextMeshProUGUI dialogueText;
    public bool dialogueEnded;

    private Queue<string> paragraphs = new Queue<string>();
    private string p;

    public DialogueContent currentDialogue;
    private Coroutine typeDialogueRoutine;
    private bool isTyping;
    public float typeSpeed = 10;
    private const string HTML_ALPHA = "<color=#00000000>";
    private const float maxTypeTime = 0.2f;
    public bool inDialogue = false;
    public bool inIntro = false;
    public bool inOutro = false;
    [SerializeField] GameObject dialogueBox;

    void Update()
    {
        if (inDialogue)
        {
            
            if (Input.GetMouseButtonDown(0) && currentDialogue != null)
            {
                DisplayNextParagraph(currentDialogue);
            }

            // if (Input.GetKeyDown(KeyCode.X))
            // {
            //     EndDialogue();
            // }
        }
    }
    public void DisplayNextParagraph(DialogueContent content)
    {

        if (paragraphs.Count == 0)
        {
            if (!dialogueEnded)
            {
                StartDialogue(content);
            }
            else if (dialogueEnded && !isTyping)
            {
                // end dialogue
                EndDialogue();
                return;
            }
        }


        if (!isTyping)
        {

        p = paragraphs.Dequeue();
        dialogueText.text = p;
            typeDialogueRoutine = StartCoroutine(TypeDialogueText(p));
        }
        else 
        {
            FinishParagraphEarly();
        }
        
        if (paragraphs.Count == 0)
        {
            dialogueEnded = true;
        }
    }

    void StartDialogue(DialogueContent content)
    {
        inDialogue = true;
        currentDialogue = content;
        dialogueBox.SetActive(true);
        speakerNameText.text = content.speakerName;
        
        for (int i = 0; i < content.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(content.paragraphs[i]);
        }
    }
    public void EndDialogue()
    {
        inIntro = false;
        inOutro = false;
        inDialogue = false;
        dialogueText.text = "";
        speakerNameText.text = "";
        currentDialogue = null;
        dialogueEnded = false;
        if (dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
        }
    }

    IEnumerator TypeDialogueText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        string originalText = text;
        string displayedText;
        int alphaIndex = 0;

        foreach (char c in text.ToCharArray())
        {
            alphaIndex++;
            dialogueText.text = originalText;
            displayedText = dialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            dialogueText.text = displayedText;
            yield return new WaitForSeconds(maxTypeTime / typeSpeed);
        }


        isTyping = false;

    }
    
    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogueRoutine);
        dialogueText.text = p;
        isTyping = false;
    }


}
