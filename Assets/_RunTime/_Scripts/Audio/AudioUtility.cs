using UnityEngine;

public static class AudioUtility
{

    public static void PlaySFX(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();

    }


}
