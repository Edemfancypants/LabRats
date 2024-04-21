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
        }
    }

    public void PlayCutscene()
    {
        if (PauseCheck.instance != null)
        {
            PauseCheck.instance.isPaused = true;
        }

        SceneManager.LoadSceneAsync(cutsceneToLoad, LoadSceneMode.Additive);

        ResetPlayerRotationOnAction(PlayerController.instance);
        cutscenePlayed = true;
    }

    public void ResetPlayerRotationOnAction(PlayerController player)
    {
        Debug.Log("<b>[CutsceneLogic]</b> Player exited POI trigger, or played cutscene on GameObject: " + gameObject.name);

        player.lookRotationPoint = null;
        player.lookRotationLock = false;

        player.RotatePlayerModel(player.moveDir);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && cutscenePlayed == false)
        {
            Debug.Log("<b>[CutsceneLogic]</b> Player entered POI trigger on GameObject: " + gameObject.name);

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.lookRotationPoint = transform;
            player.lookRotationLock = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ResetPlayerRotationOnAction(collision.gameObject.GetComponent<PlayerController>());
        }
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
