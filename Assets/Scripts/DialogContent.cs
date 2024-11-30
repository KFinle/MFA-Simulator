using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewDialogueScript", menuName = "Dialogue/New Dialogue Content")]

public class DialogueContent : ScriptableObject
{
    public string speakerName;

    [TextArea(5, 10)]
    public string[] paragraphs;
}
