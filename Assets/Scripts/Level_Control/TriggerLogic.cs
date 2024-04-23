using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
#endif

public class TriggerLogic : MonoBehaviour
{
    public enum TriggerType
    {
        Restart,
        End,
        AnimationTrigger,
        TextTrigger,
        CameraMovementTrigger,
        SFXTrigger
    }

    [System.Serializable]
    public class TriggerSettings
    {
        [Header("Trigger Type Settings")]
        public TriggerType type;

        //Restart Trigger variables
        public string restartAudioToPlay;

        //End trigger variables
        public string levelName;

        //Animation trigger variables
        public Animator animator;
        public string animationName;

        //Text trigger variables
        public TextObject dialogObject;
        public bool useWorldCanvas;
        public TextScroll textCanvas;

        //Camera Movement Trigger variables
        public int camIndex;
        public float moveSpeed;
        public GameObject otherTrigger;
        public bool canMove;

        //SFX Trigger variables
        public string audioToPlay;
    }

    [Header("Trigger Settings")]
    public TriggerSettings settings;

    private void OnEnable()
    {
        if (settings.type == TriggerType.CameraMovementTrigger)
        {
            settings.canMove = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (settings.type)
            {
                case TriggerType.Restart:
                    LevelLogic.instance.StartCoroutine(LevelLogic.instance.RestartLevel(settings.restartAudioToPlay));
                    break;
                case TriggerType.End:
                    AddLevelToSave();
                    LevelLogic.instance.StartLevelLoad(settings.levelName);
                    break;
                case TriggerType.AnimationTrigger:
                    settings.animator.SetTrigger(settings.animationName);
                    break;
                case TriggerType.TextTrigger:
                    SendTextToPlayer(collision.gameObject);
                    break;
                case TriggerType.CameraMovementTrigger:
                    if (settings.canMove == true)
                    {
                        CameraController.instance.StartCameraMove(settings.camIndex, settings.moveSpeed);
                        StartCoroutine(SetOtherTrigger());
                    }
                    break;
                case TriggerType.SFXTrigger:
                    AudioLogic.instance.PlaySFX(settings.audioToPlay);
                    break;

            }

            if (settings.type != TriggerType.CameraMovementTrigger)
            {
                gameObject.SetActive(false);
            }

            if (gameObject.transform.parent != null)
            {
                Debug.Log("<b>[TriggerLogic]</b> Trigger entered: " + gameObject.transform.parent.name);
            }
            else
            {
                Debug.Log("<b>[TriggerLogic]</b> Trigger entered: " + gameObject.transform.name);
            }
        }
    }

    public void SendTextToPlayer(GameObject player)
    {
        if (settings.textCanvas != null)
        {
            ConfigureTextScroll(settings.textCanvas);
        }
        else
        {
            TextScroll textScroll = player.GetComponentInChildren<TextScroll>();
            ConfigureTextScroll(textScroll);
        }
    }

    public void ConfigureTextScroll(TextScroll textScroll)
    {
        textScroll.timeBetweenChars = settings.dialogObject.timeBetweenChars;
        textScroll.fadeWaitDuration = settings.dialogObject.fadeWaitDuration;
        textScroll.fadeDuration = settings.dialogObject.fadeDuration;
        textScroll.sourceText = settings.dialogObject.text;
        textScroll.enabled = true;
    }

    public void AddLevelToSave()
    {
        if (!SaveSystem.instance.saveData.unlockedLevels.Contains(settings.levelName))
        {
            SaveSystem.instance.saveData.unlockedLevels.Add(settings.levelName);
            SaveSystem.instance.Save();
        }
    }

    private IEnumerator SetOtherTrigger()
    {
        settings.canMove = false;

        while (CameraController.instance.settings.camInPosition != true)
        {
            yield return null;
        }

        settings.otherTrigger.SetActive(true);
        gameObject.SetActive(false);

        settings.canMove = true;
    }
}