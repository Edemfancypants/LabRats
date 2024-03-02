using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogic : MonoBehaviour {

	public enum TriggerType
	{
		Restart,
		End,
		AnimationTrigger,
		TextTrigger
	}
	[Header("Trigger Type Settings")]
	public TriggerType type;

	[Header("Level Load Settings")]
	public string levelName;

	[Header("Animation Settings")]
	public Animator animator;
	public string animationName;

	[Header("Text Settings")]
	[TextArea]
	public string textToDisplay;
	public float timeBetweenChars;
	public float fadeDuration;
	
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			switch (type)
			{
				case TriggerType.Restart:
					LevelLogic.instance.StartCoroutine(LevelLogic.instance.RestartLevel());
					break;
				case TriggerType.End:
                    LevelLogic.instance.StartCoroutine(LevelLogic.instance.LoadLevel(levelName));
                    break;
                case TriggerType.AnimationTrigger:
					animator.SetTrigger(animationName);
					break;
				case TriggerType.TextTrigger:
					TextScroll textScroll = collision.GetComponentInChildren<TextScroll>();

					textScroll.timeBetweenChars = timeBetweenChars;
					textScroll.fadeDuration = fadeDuration;
					textScroll.sourceText = textToDisplay;
					textScroll.enabled = true;

					gameObject.SetActive(false);
					break;
            }
		}
	}		
	
}
