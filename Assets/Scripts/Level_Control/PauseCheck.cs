using UnityEngine;

public class PauseCheck : MonoBehaviour
{
    public static PauseCheck instance;

    public bool isPaused;

    private void Awake()
    {
        SetupInstance();
    }

    public void SetupInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.name = "PauseCheck";
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        isPaused = false;
    }
}
