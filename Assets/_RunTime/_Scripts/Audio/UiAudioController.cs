using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiAudioController : MonoBehaviour
{

    private AudioSource audioSource;

    public AudioSource AudioSource => audioSource != null ? audioSource : audioSource = GetComponent<AudioSource>();


    public void PlaySFX(AudioClip audioClip)
    { 
        AudioSource.clip = audioClip;

        audioSource.Play();

    }

}
