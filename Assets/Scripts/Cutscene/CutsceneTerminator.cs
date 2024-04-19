using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTerminator : MonoBehaviour
{
    public enum TerminatorType 
    { 
        Ingame,
        Standalone
    }
    public TerminatorType type;

    private PauseCheck pauseCheckReference;
    private UILogic UILogicReference;

    public Animator fadeAnimator;
    public string sceneToLoad;

    public void Start()
    {
        if (type == TerminatorType.Ingame)
        {
            pauseCheckReference = FindObjectOfType<PauseCheck>();
            UILogicReference = FindObjectOfType<UILogic>();
        }
    }

    public void TerminateCutscene()
    {
        fadeAnimator.enabled = false;

        switch(type)
        {
            case TerminatorType.Ingame:
                UILogicReference.AnimationHandler("FadeIn");
                pauseCheckReference.isPaused = false;

                SceneManager.UnloadSceneAsync(gameObject.scene);
                break;
            case TerminatorType.Standalone:
                SceneManager.LoadScene(sceneToLoad);
                break;
        }
    }
}
