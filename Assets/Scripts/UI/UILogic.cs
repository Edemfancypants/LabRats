using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    public static UILogic instance;

    private void Awake()
    {
        instance = this;
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    public enum UIType
    {
        Menu,
        Pause,
        InGame
    }

    [System.Serializable]
    public class UISettings
    {
        [Header("UI Type")]
        public UIType type;

        /// <summary>
        /// In Game UI variables
        /// </summary>

        //Fade variables
        public GameObject blackScreen;
        public Animator blackScreenAnim;

        /// <summary>
        /// Menu type UI variables
        /// </summary>

        //Menu GameObject variables
        public GameObject mainPanel;
        public GameObject optionsPanel;

        //Audio variables
        public AudioMixer audioMixer;
        public Slider masterSlider;
        public Slider BGMSlider;
        public Slider SFXSlider;
    }

    [Header("UI Settings")]
    public UISettings settings;

    public SaveSystem saveSystem;
    private bool volumeSet;

    public void Start()
    {
        volumeSet = false;

        if (saveSystem == null)
        {
            Debug.LogWarning("No SaveSystem present in scene, there will be dragons...");
        }
        else
        {
            saveSystem.onLoadEvent += SetVolumeFromSave;
        }
    }

    public void StartGameAnimation()
    {
        settings.mainPanel.SetActive(false);
        CameraController.instance.menuCamAnimator.enabled = true;
    }

    public void LoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SetVolumeSliders()
    {
        if (volumeSet == true)
        {
            float masterVolume = settings.masterSlider.value;
            settings.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, masterVolume));

            float BGMVolume = settings.BGMSlider.value;
            settings.audioMixer.SetFloat("BGMVolume", Mathf.Lerp(-80, 0, BGMVolume));

            float SFXVolume = settings.SFXSlider.value;
            settings.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 0, SFXVolume));

            SaveSystem.instance.saveData.masterFloat = masterVolume;
            SaveSystem.instance.saveData.bgmFloat = BGMVolume;
            SaveSystem.instance.saveData.sfxFloat = SFXVolume;
        }
    }

    public void SetVolumeFromSave()
    {
        Debug.Log("Data loaded successfully from save containing: " + "masterFloat: " + saveSystem.saveData.masterFloat + " " +
                                                                    "BGMFloat: " + saveSystem.saveData.bgmFloat + " " +
                                                                    "SFXFloat: " + saveSystem.saveData.sfxFloat);

        float masterVolume = saveSystem.saveData.masterFloat;
        settings.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, masterVolume));
        settings.masterSlider.value = masterVolume;

        float BGMVolume = saveSystem.saveData.bgmFloat;
        settings.audioMixer.SetFloat("BGMVolume", Mathf.Lerp(-80, 0, BGMVolume));
        settings.BGMSlider.value = BGMVolume;

        float SFXVolume = saveSystem.saveData.sfxFloat;
        settings.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 0, SFXVolume));
        settings.SFXSlider.value = SFXVolume;

        volumeSet = true;
    }

    public void QuitApplication()
    { 
        Application.Quit();
    }

    public void Fade(bool state)
    {
        settings.blackScreen.SetActive(true);

        //Fade to black
        if (state == true)
        {
            settings.blackScreenAnim.Play("FadeOut");
        }
        else //Fade from black
        {
            settings.blackScreenAnim.Play("FadeIn");
        }
    }
}
