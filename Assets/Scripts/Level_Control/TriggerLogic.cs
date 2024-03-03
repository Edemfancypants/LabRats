using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    public enum TriggerType
    {
        Restart,
        End,
        AnimationTrigger,
        TextTrigger
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
    }

    [Header("Trigger Settings")]
    public TriggerSettings settings;

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
            }

            gameObject.SetActive(false);
        }
    }

    private void SendTextToPlayer(GameObject player)
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

    private void ConfigureTextScroll(TextScroll textScroll)
    {
        textScroll.timeBetweenChars = settings.dialogObject.timeBetweenChars;
        textScroll.fadeWaitDuration = settings.dialogObject.fadeWaitDuration;
        textScroll.fadeDuration = settings.dialogObject.fadeDuration;
        textScroll.sourceText = settings.dialogObject.text;
        textScroll.enabled = true;
    }
}
