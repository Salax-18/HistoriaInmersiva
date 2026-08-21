using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerName;
    public TMP_Text dialogueText;

    [TextArea(2, 5)]
    public string[] dialogueLines;

    private int currentLine = 0;
    public bool dialogueActive = false;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);

        ShowLine();
    }

    public void NextLine()
    {
        if (!dialogueActive)
            return;

        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowLine();
        }
    }

    private void ShowLine()
    {
        speakerName.text = "Terapeuta";
        dialogueText.text = dialogueLines[currentLine];
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}