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
        public GameObject collectiblesPanel;

        //Audio variables
        public AudioMixer audioMixer;
        public Slider masterSlider;
        public Slider BGMSlider;
        public Slider SFXSlider;

        //Animation variables
        public Animator UIAnimator;
    }

    public UISettings settings;

    public SaveSystem saveSystem;
    private bool volumeSet;

    public void Start()
    {
        if (saveSystem == null)
        {
            Debug.LogWarning("UILogic script detected no SaveSystem present in the scene, there will be dragons...");
        }
        else
        {
            if (settings.type == UIType.Menu)
            {
                SaveSystem.instance.Load(() => {
                    Debug.Log("Data loaded successfully!");
                    SetVolumeFromSave();
                });
            }
        }

        if (settings.type == UIType.Menu)
        {
            volumeSet = false;
        }
        else
        {
            volumeSet = true;
        }
    }

    public void AnimationHandler(string logicToRun)
    {
        switch (logicToRun)
        {
            ////Game Start Animations////
            case "FadeMainUI":
                settings.UIAnimator.enabled = true;
                settings.UIAnimator.Play("GameStart_UIFade");
                break;
            case "PlayStartGameAnimation":
                settings.mainPanel.SetActive(false);
                CameraController.instance.menuCamAnimator.enabled = true;
                CameraController.instance.menuCamAnimator.Play("GameStart_CamRotation");
                break;

            ////Options Animations
            case "FadeMainUIOptions":
                settings.UIAnimator.enabled = true;
                settings.UIAnimator.Play("Options_MainUIFadeOut");
                break;
            case "PlayOptionsAnimation":
                CameraController.instance.menuCamAnimator.enabled = true;
                CameraController.instance.menuCamAnimator.Play("Options_CamRotation");
                break;
            case "SwitchToOptions":
                settings.mainPanel.SetActive(false);
                settings.optionsPanel.SetActive(true);
                break;
            case "SwitchToMainFromOptions":
                settings.mainPanel.SetActive(true);
                settings.optionsPanel.SetActive(false);
                break;
            case "FadeOptionsUI":
                settings.UIAnimator.Play("Options_OptionsUIFadeOut");
                break;
            case "PlayOptionsMoveBack":
                CameraController.instance.menuCamAnimator.SetTrigger("Options");
                break;

            ////Collectibles Animations////
            case "FadeMainUICollectibles":
                settings.UIAnimator.enabled = true;
                settings.UIAnimator.Play("Collectibles_MainUIFadeOut");
                break;
            case "PlayCollectibleAnimation":
                CameraController.instance.menuCamAnimator.enabled = true;
                CameraController.instance.menuCamAnimator.Play("Collectibles_CamCloseUp");
                break;
            case "SwitchToCollectibles":
                settings.mainPanel.SetActive(false);
                settings.collectiblesPanel.SetActive(true);
                break;
            case "SwitchToMainFromCollectibles":
                settings.mainPanel.SetActive(true);
                settings.collectiblesPanel.SetActive(false);
                break;
            case "FadeCollectiblesUI":
                settings.UIAnimator.Play("Collectibles_CollectibleUIFadeOut");
                break;
            case "PlayCollectiblesMoveBack":
                CameraController.instance.menuCamAnimator.SetTrigger("Collectible");
                break;

            default:
                Debug.LogWarning("Couldn't find logic connected to this animationEvent.");
                break;
        }
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

    public void ResetVolume()
    {
        volumeSet = false;

        SaveSystem.instance.Load(() => {
            Debug.Log("Data loaded successfully!");
            SetVolumeFromSave();
        });
    }

    public void QuitApplication()
    { 
        Application.Quit();
    }

    public void Fade(bool state)
    {
        if (settings.type == UIType.InGame)
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
}
