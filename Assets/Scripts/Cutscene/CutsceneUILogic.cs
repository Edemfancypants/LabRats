using UnityEngine;
using UnityEngine.Playables;

public class CutsceneUILogic : MonoBehaviour
{

    public PlayableDirector timeline;

    private bool isPlaying = false;

    private void Start()
    {
        if (timeline != null && timeline.state == PlayState.Playing)
        {
            isPlaying = true;
        }
    }

    public void PauseResumeTimeline()
    {
        if (timeline != null)
        {
            if (isPlaying)
            {
                timeline.Pause();
                isPlaying = false;
            }
            else
            {
                timeline.Play();
                isPlaying = true;
            }
        }
    }

    public void RestartTimeline()
    {
        if (timeline != null)
        {
            timeline.time = 0f;
            timeline.Play();
            isPlaying = true;
        }
    }
}
