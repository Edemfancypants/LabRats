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

        //Pause UI variables
        public GameObject pauseUI;
        public Animator pauseAnimator;

        /// <summary>
        /// Menu type UI variables
        /// </summary>

        //Menu GameObject variables
        public GameObject mainPanel;
        public GameObject optionsPanel;
        public GameObject collectiblesPanel;
        public GameObject levelSelectPanel;

        //Audio variables
        public AudioMixer audioMixer;
        public Slider masterSlider;
        public Slider BGMSlider;
        public Slider SFXSlider;

        //Animation variables
        public Animator UIAnimator;

        //TextScroll variables
        public TextScroll textScroll;
    }

    public UISettings settings;

    public SaveSystem saveSystem;

    public string levelToLoad;

    private bool volumeSet;
    private bool canPause;

    private PlayerController player;

    private void Start()
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
            Time.timeScale = 1.0f;
        }
        else
        {
            volumeSet = true;

            canPause = true;

            player = PlayerController.instance;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause == true && settings.pauseUI.activeInHierarchy == false || Input.GetButtonDown("Start") && canPause == true && settings.pauseUI.activeInHierarchy == false)
        {
            canPause = false;
            player.isPaused = true;
            if (player.grappleGun != null)
                player.grappleGun.isPaused = true;
            settings.pauseAnimator.Play("Pause_FadeIn");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && canPause == true && settings.pauseUI.activeInHierarchy == true || Input.GetButtonDown("Start") && canPause == true && settings.pauseUI.activeInHierarchy == true)
        {
            ResumeGame();
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
                CameraController.instance.settings.menuCamAnimator.enabled = true;
                CameraController.instance.settings.menuCamAnimator.Play("GameStart_CamRotation");
                break;

            ////Options Animations
            case "FadeMainUIOptions":
                settings.UIAnimator.enabled = true;
                settings.UIAnimator.Play("Options_MainUIFadeOut");
                break;
            case "PlayOptionsAnimation":
                CameraController.instance.settings.menuCamAnimator.enabled = true;
                CameraController.instance.settings.menuCamAnimator.Play("Options_CamRotation");
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
                CameraController.instance.settings.menuCamAnimator.SetTrigger("Options");
                break;

            ////Collectibles Animations////
            case "FadeMainUICollectibles":
                settings.UIAnimator.enabled = true;
                settings.UIAnimator.Play("Collectibles_MainUIFadeOut");
                break;
            case "PlayCollectibleAnimation":
                CameraController.instance.settings.menuCamAnimator.enabled = true;
                CameraController.instance.settings.menuCamAnimator.Play("Collectibles_CamCloseUp");
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
                CameraController.instance.settings.menuCamAnimator.SetTrigger("Collectible");
                break;
            case "DisableTextScroll":
                settings.textScroll.StartCoroutine(settings.textScroll.DestroyText());
                break;

            ////Level Select Animations////
            case "FadeMainUILevelSelect":
                settings.UIAnimator.enabled = true;
                settings.UIAnimator.Play("LevelSelect_MainUIFadeOut");
                break;
            case "PlayLevelSelectAnimation":
                CameraController.instance.settings.menuCamAnimator.enabled = true;
                CameraController.instance.settings.menuCamAnimator.Play("LevelSelect_CamCloseUp");
                break;
            case "SwitchToLevelSelect":
                settings.mainPanel.SetActive(false);
                settings.levelSelectPanel.SetActive(true);

                SetButtonInteractability();
                break;
            case "FadeLevelSelectUI":
                settings.UIAnimator.Play("LevelSelect_LevelSelectFadeOut");
                break;
            case "PlayLevelSelectMoveBack":
                CameraController.instance.settings.menuCamAnimator.SetTrigger("LevelSelect");
                break;
            case "SwitchToMainFromLevelSelect":
                settings.mainPanel.SetActive(true);
                settings.levelSelectPanel.SetActive(false);
                break;
            case "FadeLevelSelectUIPlay":
                settings.UIAnimator.Play("LevelSelect_LevelSelectFadeOutPlay");
                break;
            case "PlayElevatorStartAnimation":
                CameraController.instance.settings.elevatorAnimator.Play("Elevator_Start");
                CameraController.instance.settings.menuCamAnimator.Play("LevelSelect_ElevatorMoveUp");
                break;
            case "DisableLevelSelectPanel":
                settings.levelSelectPanel.SetActive(false);
                break;
            case "LoadLevel":
                LoadLevel(levelToLoad);
                break;

            ////Pause UI Animations////
            case "SwitchToPause":
                settings.pauseUI.SetActive(true);
                break;
            case "PauseTimescale":
                Time.timeScale = 0f;
                canPause = true;
                break;
            case "DisablePause":
                settings.pauseUI.SetActive(false);
                canPause = true;
                break;
            case "PlayLoadMenuAnimation":
                Time.timeScale = 1f;
                settings.pauseAnimator.Play("Pause_FadeOutToMenu");
                break;
            case "LoadMenu":
                SceneManager.LoadScene("Menu");
                break;
            case "PlayQuitGameAnimation":
                Time.timeScale = 1f;
                settings.pauseAnimator.Play("Pause_FadeOutToQuit");
                break;
            case "QuitGame":
                QuitApplication();
                break;

            default:
                Debug.LogWarning("Couldn't find logic connected to this animationEvent.");
                break;
        }
    }

    public void ResumeGame()
    {
        canPause = false;
        Time.timeScale = 1f;
        player.isPaused = false;
        if (player.grappleGun != null)
            player.grappleGun.isPaused = false;
        settings.pauseAnimator.Play("Pause_FadeOut");
    }

    public void LoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SetLevelString(string levelString)
    {
        levelToLoad = levelString;
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

    public void SetButtonInteractability()
    {
        GameObject[] menuButtons = GameObject.FindGameObjectsWithTag("MenuButton");

        foreach (GameObject obj in menuButtons)
        {
            LevelButtonId ButtonId = obj.GetComponent<LevelButtonId>();
            string id = ButtonId.id;

            foreach (string savedId in SaveSystem.instance.saveData.unlockedLevels)
            {
                if (savedId == id)
                {
                    obj.GetComponent<Button>().interactable = true;
                }
            }
        }
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
        Debug.Log("Application successfully terminated. Except if you're seeing this.");
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
