using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioLogic : MonoBehaviour
{
    public enum AudioLogicType
    {
        InGame,
        Cutscene
    }
    public AudioLogicType type;

    public static AudioLogic instance;

    private void Awake()
    {
        if (type == AudioLogicType.InGame)
        {
            instance = this;
        }

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

    public void PauseSFX(string sfxname, bool state)
    {
        if (soundEffectsDict.ContainsKey(sfxname))
        {
            AudioSource audioSource = soundEffectsDict[sfxname];

            if (state == true) //Pause SFX
            {
                audioSource.Pause();
                Debug.Log("<b>[AudioLogic]</b> Paused sound effect with name: " + sfxname);
            }
            else if (state == false) //UnPause SFX
            {
                if (audioSource.isPlaying == false)
                {
                    audioSource.UnPause();
                    Debug.Log("<b>[AudioLogic]</b> UnPaused sound effect with name: " + sfxname);
                }
                else
                {
                    Debug.LogWarning("<b>[AudioLogic]</b> Sound effect with name '" + sfxname + "' is not paused.");
                }
            }
        }
        else
        {
            Debug.LogWarning("<b>[AudioLogic]</b> Sound effect with name '" + sfxname + "' not found.");
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

    public IEnumerator FadeAudio(string sfxToFade, float targetVolume, float fadeDuration)
    {
        if (soundEffectsDict.ContainsKey(sfxToFade))
        {
            AudioSource audioSource = soundEffectsDict[sfxToFade];

            float startVolume = audioSource.volume;
            float volumeChangePerFrame = (targetVolume - startVolume) / (fadeDuration / Time.deltaTime);

            while (Mathf.Abs(audioSource.volume - targetVolume) > 0.01f)
            {
                audioSource.volume += volumeChangePerFrame;
                audioSource.volume = Mathf.Clamp01(audioSource.volume);

                Debug.Log(audioSource.volume);

                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("<b>[AudioLogic]</b> Sound effect with name '" + sfxToFade + "' not found.");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioLogic))]
public class AudioLogicInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioLogic audioLogic = (AudioLogic)target;

        if (GUILayout.Button("Debug Dictionary"))
        {
            Debug.Log("Sound Effects Dictionary Contents:");

            foreach (var kvp in audioLogic.soundEffectsDict)
            {
                Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
            }
        }
    }
}
#endif
