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
    public string[] speakerNames;

    [Header("Regression Dialogue")]

    [TextArea(2, 5)]
    public string[] regressionDialogueLines;
    public string[] regressionSpeakerNames;

    [Header("Therapist Animation")]
    public Animator therapistAnimator;

    [Header("Regression")]
    public SecuenciaRegresion secuenciaRegresion;

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

        ShowLine();
    }

    public void StartRegressionDialogue()
    {
        dialogueLines = regressionDialogueLines;
        speakerNames = regressionSpeakerNames;

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
        // Mostrar quién habla
        speakerName.text = speakerNames[currentLine];

       
        dialogueText.text = dialogueLines[currentLine];

        if (therapistAnimator != null)
        {
            if (speakerNames[currentLine] == "Terapeuta")
            {
                therapistAnimator.SetBool("isTalking", true);
            }
            else
            {
                therapistAnimator.SetBool("isTalking", false);
            }
        }
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