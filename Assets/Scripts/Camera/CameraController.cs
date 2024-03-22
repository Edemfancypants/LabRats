using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public bool menuCamera;

    public Animator menuCamAnimator;
    public Animator doorAnimator;
    public Animator UIAnimator;

    //public AnimationCurve animCurve;

    public List<Transform> camPoints = new List<Transform>();
    [HideInInspector]
    public bool camInPosition;

    private void Start()
    {
        camInPosition = true;

        if (menuCamera == true)
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
                doorAnimator.Play("GameStart_DoorOpenAnim");
                break;

            ////Options Animations////
            case "OptionsFadeStart":
                UIAnimator.Play("Options_OptionsUIFadeIn");
                break;
            case "MainFadeStart":
                UIAnimator.Play("Options_MainUIFadeIn");
                break;

            ////Collectibles Animations////
            case "CollectibleFadeStart":
                UIAnimator.Play("Collectibles_CollectibleUIFadeIn");
                break;
            case "MainFadeStartFromCollectibles":
                UIAnimator.Play("Collectibles_MainUIFadeIn");
                break;

            default:
                Debug.LogWarning("Couldn't find logic connected to this animationEvent.");
                break;
        }
    }

    public void StartLevelLoad(string levelName)
    {
        UILogic.instance.LoadLevel(levelName);
    }

    public IEnumerator SetCameraPos(int camPoint, float moveTime)
    {
        camInPosition = false;
        Vector3 targetPosition = camPoints[camPoint].position;

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
        camInPosition = true;
    }
}

//Debug camera controller
#if UNITY_EDITOR
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditorTest : Editor
{
    private int camPos = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraController cameraController = (CameraController)target;

        DrawDebugHeader();

        GUILayout.Label("Camera Forward", EditorStyles.boldLabel);
        if (GUILayout.Button("Camera Forward"))
        {
            CameraForwardDebug(cameraController);
        }

        GUILayout.Label("Camera Backward", EditorStyles.boldLabel);
        if (GUILayout.Button("Camera Back"))
        {
            CameraBackwardDebug(cameraController);
        }
    }

    private void DrawDebugHeader()
    {
        GUILayout.Space(10);
        GUILayout.Label("Debug Settings", EditorStyles.boldLabel);
        GUILayout.Space(5);
    }

    private void CameraForwardDebug(CameraController _cameraController)
    {
        if (_cameraController.camPoints.Count > 0 && camPos < _cameraController.camPoints.Count - 1 && _cameraController.camInPosition)
        {
            camPos++;
            _cameraController.StartCoroutine(_cameraController.SetCameraPos(camPos, .5f));
            Debug.Log("Current camera position index is: " + camPos);
        }
        else
        {
            Debug.LogWarning("Cannot move camera forward: Index out of range or camera not in position.");
        }
    }

    private void CameraBackwardDebug(CameraController _cameraController)
    {
        if (camPos > 0 && _cameraController.camInPosition)
        {
            camPos--;
            _cameraController.StartCoroutine(_cameraController.SetCameraPos(camPos, .5f));
            Debug.Log("Current camera position index is: " + camPos);
        }
        else
        {
            Debug.LogWarning("Cannot move camera back: Index out of range or camera not in position.");
        }
    }
}
#endif