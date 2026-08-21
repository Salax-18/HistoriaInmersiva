using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerName;
    public TMP_Text dialogueText;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("Therapist Animation")]
    public Animator therapistAnimator;

    private int currentLine = 0;
    public bool dialogueActive = false;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
      
        if (!dialogueActive)
            return;

        // Presionar E para avanzar
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);

        
        if (therapistAnimator != null)
        {
            therapistAnimator.SetBool("isTalking", true);
        }

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

       
        if (therapistAnimator != null)
        {
            therapistAnimator.SetBool("isTalking", false);
        }
    }
}