using System.Collections.Generic;
using UnityEngine;

public class AudioLogic : MonoBehaviour
{
    public static AudioLogic instance;

    private void Awake()
    {
        instance = this;

        foreach (AudioSource audioSource in soundEffects)
        {
            soundEffectsDict.Add(audioSource.name, audioSource);
        }
    }

    public Dictionary<string, AudioSource> soundEffectsDict = new Dictionary<string, AudioSource>();

    public AudioSource[] soundEffects;
    public AudioSource BGM;

    public void PlaySFX(string sfxName)
    {
        if (soundEffectsDict.ContainsKey(sfxName))
        {
            AudioSource audioSource = soundEffectsDict[sfxName];
            audioSource.Stop();
            audioSource.Play();

            Debug.Log("<b>[AudioLogic]</b> Started playing sound effect: " + sfxName);
        }
        else
        {
            Debug.LogWarning("<b>[AudioLogic]</b> Sound effect with name '" + sfxName + "' not found.");
        }
    }

    public void PlaySFXPitched(string sfxName)
    {
        if (soundEffectsDict.ContainsKey(sfxName))
        {
            AudioSource audioSource = soundEffectsDict[sfxName];
            audioSource.pitch = Random.Range(.8f, 1.2f);

            PlaySFX(sfxName);
        }
        else
        {
            Debug.LogWarning("<b>[AudioLogic]</b> Sound effect with name '" + sfxName + "' not found.");
        }
    }

    public void PlayBGM(bool Paused)
    {
        if (Paused == false)
        {
            BGM.Pause();
        }
        else
        {
            BGM.UnPause();
        }
    }
}
