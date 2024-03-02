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
    public float fadeDuration;
    private float elapsedTime = 0f;

    [HideInInspector]
    public string sourceText;
	[HideInInspector]
	public string shownText;

	private int sourceTextLength;
	private int stringIndex;

    public void OnEnable()
	{
		text.color = Color.white;

		stringIndex = 0;
		sourceTextLength = sourceText.Length;

		StartCoroutine(ShowText(sourceText[stringIndex]));
		stringIndex++;
	}

	public void SelectChar()
	{
		if (stringIndex < sourceTextLength)
		{
			StartCoroutine(ShowText(sourceText[stringIndex]));
			stringIndex++;
		}
		else if (stringIndex == sourceTextLength)
		{
			StartCoroutine(FadeText());
		}
	}

	public IEnumerator ShowText(char charToShow)
	{
		yield return new WaitForSeconds(timeBetweenChars);
		shownText = shownText + charToShow;
		text.text = shownText;
		SelectChar();
	}

    public IEnumerator FadeText()
    {
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
        shownText = string.Empty;

        enabled = false;
    }
}
