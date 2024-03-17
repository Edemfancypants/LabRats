using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScroll : MonoBehaviour 
{
	[Header("Text Reference")]
	public TMP_Text text;

	[Header("Scroll Settings")]
	public float timeBetweenChars;
	public float fadeWaitDuration;
    public float fadeDuration;
    private float elapsedTime = 0f;

    [HideInInspector]
    public string sourceText;

    public void OnEnable()
	{
		text.color = Color.white;

		StartCoroutine(ShowText());
	}

	public IEnumerator ShowText()
	{
		text.text = string.Empty;
		string textToShow = sourceText;

		for (int i = 0; i < textToShow.Length; i++)
		{
			text.text += textToShow[i];
			yield return new WaitForSecondsRealtime(timeBetweenChars);
		}

		StartCoroutine(FadeText());
	}

    public IEnumerator FadeText()
    {
		yield return new WaitForSeconds(fadeWaitDuration);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            text.color = Color.Lerp(text.color, Color.clear, t);
            yield return null;
        }
        text.color = Color.clear;

		text.text = string.Empty;
        sourceText = string.Empty;

        enabled = false;
    }
}