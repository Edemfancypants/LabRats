using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TriggerLogic : MonoBehaviour
{
    public enum TriggerType
    {
        Restart,
        End,
        AnimationTrigger,
        TextTrigger,
        CameraMovementTrigger
    }

    [System.Serializable]
    public class TriggerSettings
    {
        [Header("Trigger Type Settings")]
        public TriggerType type;

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
        public float moveTime;
        public GameObject otherTrigger;
        public bool canMove;
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
                    LevelLogic.instance.StartCoroutine(LevelLogic.instance.RestartLevel());
                    break;
                case TriggerType.End:
                    LevelLogic.instance.StartCoroutine(LevelLogic.instance.LoadLevel(settings.levelName));
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
                        StartCoroutine(ActivateCameraMovement());
                    }
                    break;
            }

            if (settings.type != TriggerType.CameraMovementTrigger)
            {
                gameObject.SetActive(false);
            }

            Debug.Log("Trigger entered: " + gameObject.transform.parent.name);
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

    private IEnumerator ActivateCameraMovement()
    {
        StartCoroutine(CameraController.instance.SetCameraPos(settings.camIndex, settings.moveTime));

        settings.canMove = false;

        while (CameraController.instance.camInPosition != true)
        {
            yield return null;
        }

        settings.otherTrigger.SetActive(true);
        gameObject.SetActive(false);

        settings.canMove = true;
    }
}