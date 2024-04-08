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

    public enum CameraMovementType
    {
        Basic,
        Advanced
    }

    [System.Serializable]
    public class CamControllerSettings
    {
        //Camera movement variables

        public List<Transform> camPoints = new List<Transform>();

        public CameraMovementType movementType;

        public AnimationCurve animCurve;
        public float moveSpeedMultiplier;

        //Menu camera variables

        public bool menuCamera;

        public Animator menuCamAnimator;
        public Animator doorAnimator;
        public Animator UIAnimator;
        public Animator elevatorAnimator;

        public bool camInPosition;
    }

    public CamControllerSettings settings;

    private void Start()
    {
        if (settings.menuCamera == true)
        {
            StartCameraMove(1, 1f);
        }
    }

    public void AnimationHandler(string logicToRun)
    {
        switch (logicToRun)
        {
            ////Game Start Animations////
            case "DoorAnimationStart":
                AudioLogic.instance.PlaySFXPitched("DoorBeep");
                AudioLogic.instance.PlaySFXPitched("DoorOpen");
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
                AudioLogic.instance.PlaySFXPitched("ElevatorDown");
                settings.elevatorAnimator.Play("Elevator_Down");
                break;
            case "LevelSelectFadeStart":
                settings.UIAnimator.Play("LevelSelect_LevelSelectFadeIn");
                break;
            case "MainFadeStartFromLevelSelect":
                settings.UIAnimator.Play("LevelSelect_MainUIFadeIn");
                break;
            case "ElevatorBackAnimStart":
                AudioLogic.instance.PlaySFXPitched("ElevatorDown");
                settings.elevatorAnimator.Play("Elevator_Up");
                break;
            case "StartFadeAnimation":
                settings.UIAnimator.Play("LevelSelect_FadeStart");
                break;

            default:
                Debug.LogWarning("<b>[CameraController]</b> Couldn't find logic connected to this animationEvent.");
                break;
        }
    }

    public void StartCameraMove(int camPoint, float moveSpeed)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(camPoint, moveSpeed));
    }

    public IEnumerator MoveCamera(int camPoint, float moveSpeed)
    {
        settings.camInPosition = false;
        Vector3 targetPosition = settings.camPoints[camPoint].position;

        Debug.Log("<b>[CameraController]</b> Camera started moving to camPoint: " + settings.camPoints[camPoint].name + " position of: " + targetPosition);

        //Basic CameraMove script using distance
        if (settings.movementType == CameraMovementType.Basic)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / moveSpeed);
                yield return null;
            }
        }

        //Advanced CameraMove script using movement duration with AnimCurve
        if (settings.movementType == CameraMovementType.Advanced)
        {
            Vector3 startPos = transform.position;
            float moveSpeedTotal = moveSpeed * settings.moveSpeedMultiplier;
            float duration = Vector3.Distance(startPos, targetPosition) / moveSpeedTotal;

            float elapsedTime = 0f;
            while (elapsedTime <= duration)
            {
                elapsedTime += Time.deltaTime;
                float percent = Mathf.Clamp01(elapsedTime / duration);
                transform.position = Vector3.Lerp(startPos, targetPosition, settings.animCurve.Evaluate(percent));
                yield return null;
            }
        }

        transform.position = targetPosition;
        Debug.Log("<b>[CameraController]</b> Camera in position");
        settings.camInPosition = true;
    }
}