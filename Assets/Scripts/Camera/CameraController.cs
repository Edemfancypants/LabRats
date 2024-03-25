using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class CamControllerSettings
    {
        public List<Transform> camPoints = new List<Transform>();

        public bool menuCamera;

        public Animator menuCamAnimator;
        public Animator doorAnimator;
        public Animator UIAnimator;
        public Animator elevatorAnimator;

        //public AnimationCurve animCurve;

        public bool camInPosition;
    }

    public CamControllerSettings settings;

    private void Start()
    {
        settings.camInPosition = true;

        if (settings.menuCamera == true)
        {
            StartCoroutine(SetCameraPos(1, 10f));
        }
    }

    public void AnimationHandler(string logicToRun)
    {
        switch (logicToRun)
        {
            ////Game Start Animations////
            case "DoorAnimationStart":
                settings.doorAnimator.Play("GameStart_DoorOpenAnim");
                break;
            case "StartGame":
                UILogic.instance.LoadLevel("Factory_1");
                break;

            ////Options Animations////
            case "OptionsFadeStart":
                settings.UIAnimator.Play("Options_OptionsUIFadeIn");
                break;
            case "MainFadeStart":
                settings.UIAnimator.Play("Options_MainUIFadeIn");
                break;

            ////Collectibles Animations////
            case "CollectibleFadeStart":
                settings.UIAnimator.Play("Collectibles_CollectibleUIFadeIn");
                break;
            case "MainFadeStartFromCollectibles":
                settings.UIAnimator.Play("Collectibles_MainUIFadeIn");
                break;

            ////Level Select Animations////
            case "ElevatorAnimStart":
                settings.elevatorAnimator.Play("Elevator_Down");
                break;
            case "LevelSelectFadeStart":
                settings.UIAnimator.Play("LevelSelect_LevelSelectFadeIn");
                break;
            case "MainFadeStartFromLevelSelect":
                settings.UIAnimator.Play("LevelSelect_MainUIFadeIn");
                break;
            case "ElevatorBackAnimStart":
                settings.elevatorAnimator.Play("Elevator_Up");
                break;
            case "StartFadeAnimation":
                settings.UIAnimator.Play("LevelSelect_FadeStart");
                break;

            default:
                Debug.LogWarning("Couldn't find logic connected to this animationEvent.");
                break;
        }
    }

    public IEnumerator SetCameraPos(int camPoint, float moveTime)
    {
        settings.camInPosition = false;
        Vector3 targetPosition = settings.camPoints[camPoint].position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Debug.Log("Moving");
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / moveTime);
            yield return null;
        }

        //Ez is egy megoldás, de a másik egyelőre smoothabb xdd

        //float elapsedTime = 0f;
        //while (elapsedTime < moveTime)
        //{
        //    elapsedTime += Time.deltaTime;
        //    float t = Mathf.Clamp01(elapsedTime / moveTime);
        //    transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        //    yield return null;
        //}

        transform.position = targetPosition;
        Debug.Log("Camera in position");
        settings.camInPosition = true;
    }
}