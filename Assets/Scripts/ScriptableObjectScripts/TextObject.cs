using UnityEngine;

[CreateAssetMenu(menuName = "DialogSystem/Create Text Object")]
public class TextObject : ScriptableObject
{
    [TextArea]
    public string text;

    public float timeBetweenChars;
    public float fadeWaitDuration;
    public float fadeDuration;
}
