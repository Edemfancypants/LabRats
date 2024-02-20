using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{
    public static UILogic instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject blackScreen;
    public Animator blackScreenAnim;

    public void Fade(bool state)
    {
        blackScreen.SetActive(true);

        //Fade to black
        if (state == true)
        {
            blackScreenAnim.Play("FadeOut");
        }
        else //Fade from black
        {
            blackScreenAnim.Play("FadeIn");
        }
    }
}
