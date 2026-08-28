using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerName;
    public TMP_Text dialogueText;

    [Header("Dialogue 1")]
    [TextArea(2, 5)]
    public string[] dialogueLines;
    public string[] speakerNames;

    [Header("Dialogue 2 - Regression")]
    [TextArea(2, 5)]
    public string[] regressionDialogueLines;
    public string[] regressionSpeakerNames;

    [Header("Therapist Animation")]
    public Animator therapistAnimator;

    [Header("Regression")]
    public SecuenciaRegresion secuenciaRegresion;

    private int currentLine = 0;
    public bool dialogueActive = false;

    [Header("Hypnosis Audio")]
    public AudioSource hypnosisAudio;
    public float hypnosisFadeInDuration = 4f;

    // Indica si actualmente estamos en el diálogo 2
    private bool isRegressionDialogue = false;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        // SPACE para avanzar el diálogo
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }

    // DIÁLOGO 1


    public void StartDialogue()
    {
        isRegressionDialogue = false;

       

        dialogueActive = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);

        ShowLine();
    }


    // DIÁLOGO 2
   

    public void StartRegressionDialogue()
    {
        isRegressionDialogue = true;

        dialogueActive = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);

        ShowRegressionLine();
    }


    public void NextLine()
    {
        if (!dialogueActive)
            return;

        currentLine++;

        if (isRegressionDialogue)
        {
            if (currentLine >= regressionDialogueLines.Length)
            {
                EndDialogue();
            }
            else
            {
                ShowRegressionLine();
            }
        }
        else
        {
            if (currentLine >= dialogueLines.Length)
            {
                EndDialogue();
            }
            else
            {
                ShowLine();
            }
        }
    }


    // MOSTRAR DIÁLOGO 1
 

    private void ShowLine()
    {
        speakerName.text = speakerNames[currentLine];
        dialogueText.text = dialogueLines[currentLine];

        UpdateTherapistAnimation();
    }


    // MOSTRAR DIÁLOGO 2


    private void ShowRegressionLine()
    {
        speakerName.text = regressionSpeakerNames[currentLine];
        dialogueText.text = regressionDialogueLines[currentLine];

        UpdateTherapistAnimation();

        // La hipnosis comienza cuando aparece la línea 2
        if (currentLine == 2)
        {
            if (hypnosisAudio != null)
            {
                StartCoroutine(FadeInHypnosis());
            }
        }
    }
    //corrutina para hacer fade in del audio de hipnosis
    private System.Collections.IEnumerator FadeInHypnosis()
    {
        hypnosisAudio.volume = 0f;
        hypnosisAudio.Play();

        float elapsed = 0f;

        while (elapsed < hypnosisFadeInDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / hypnosisFadeInDuration;

            hypnosisAudio.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        hypnosisAudio.volume = 1f;
    }


    // ANIMACIÓN DEL TERAPEUTA


    private void UpdateTherapistAnimation()
    {
        if (therapistAnimator != null)
        {
            if (speakerName.text == "Terapeuta")
            {
                therapistAnimator.SetBool("isTalking", true);
            }
            else
            {
                therapistAnimator.SetBool("isTalking", false);
            }
        }
    }

   
    // TERMINAR DIÁLOGO
   

    private void EndDialogue()
    {
        dialogueActive = false;

        dialoguePanel.SetActive(false);

        if (therapistAnimator != null)
        {
            therapistAnimator.SetBool("isTalking", false);
        }

   
       
        if (isRegressionDialogue)
        {
            if (secuenciaRegresion != null)
            {
                secuenciaRegresion.StartRegression();
            }
        }
    }
}