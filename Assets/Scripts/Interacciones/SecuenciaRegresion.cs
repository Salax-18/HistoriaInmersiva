using UnityEngine;

public class SecuenciaRegresion : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource hypnosisAudio;

    public void StartRegression()
    {
        Debug.Log("REGRESIÓN INICIADA");
    }

    public void StartHypnosisAudio()
    {
        if (hypnosisAudio != null)
        {
            hypnosisAudio.Play();
        }
    }
}