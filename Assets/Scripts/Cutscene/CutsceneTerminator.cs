using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTerminator : MonoBehaviour
{

    private PauseCheck pauseCheckReference;
    private UILogic UILogicReference;

    public Animator fadeAnimator;

    public void Start()
    {
        pauseCheckReference = FindObjectOfType<PauseCheck>();
        UILogicReference = FindObjectOfType<UILogic>();
    }

    public void TerminateCutscene()
    {
        fadeAnimator.enabled = false;
        UILogicReference.AnimationHandler("FadeIn");
        pauseCheckReference.isPaused = false;

        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
