using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CutsceneLogic : MonoBehaviour
{
    [Header("Script References")]
    public UILogic uiLogic;
    public DragObject dragObject;
    public GameObject elevatorBlock;
    public GameObject grappleGun;
    public POILogic poiLogic;

    [Header("Cutscene Settings")]
    public string cutsceneToLoad;
    public bool cutscenePlayed = false;

    [Header("Audio Settings")]
    public List<string> soundEffectsToPause = new List<string>();

    private void Start()
    {
        if (uiLogic == null)
        {
            uiLogic = FindObjectOfType<UILogic>();
        }
    }

    private void OnEnable()
    {
        uiLogic.PlayCutsceneEvent += PlayCutscene;
    }

    private void OnDisable()
    {
        uiLogic.PlayCutsceneEvent -= PlayCutscene;
    }

    private void OnMouseDown()
    {
        if (cutscenePlayed == false)
        {
            if (soundEffectsToPause.Count > 0)
            {
                UILogic.instance.sfxToUnpause = soundEffectsToPause;

                for (int i = 0; i < soundEffectsToPause.Count; i++)
                {
                    AudioLogic.instance.PauseSFX(soundEffectsToPause[i], true);
                }
            }

            uiLogic.AnimationHandler("FadeOutCutscene");

            if (dragObject != null)
            {
                dragObject.enabled = true;
            }

            if (elevatorBlock != null)
            {
                elevatorBlock.SetActive(false);
            }

            if (grappleGun != null)
            {
                grappleGun.SetActive(true);
            }
        }
    }

    public void PlayCutscene()
    {
        if (PauseCheck.instance != null)
        {
            PauseCheck.instance.isPaused = true;
        }

        SceneManager.LoadSceneAsync(cutsceneToLoad, LoadSceneMode.Additive);

        poiLogic.ResetPlayerRotationOnAction(PlayerController.instance);
        cutscenePlayed = true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CutsceneLogic))]
public class GameEventInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Play Cutscene"))
        {
            (target as CutsceneLogic).PlayCutscene();
        }
    }
}
#endif
