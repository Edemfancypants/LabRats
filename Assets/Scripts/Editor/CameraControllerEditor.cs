using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    private int camPos = 0;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CameraController cameraController = (CameraController)target;
        CameraController.CamControllerSettings settings = cameraController.settings;

        ////In Game Camera Settings////

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Camera Points", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.camPoints"), true);

        ////Menu Camera Settings////

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Menu Cam Settings", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.menuCamera"));

        if (settings.menuCamera)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.menuCamAnimator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.doorAnimator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.UIAnimator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.elevatorAnimator"));
        }

        ////Debug Buttons////

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

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

        serializedObject.ApplyModifiedProperties();
    }

    private void CameraForwardDebug(CameraController _cameraController)
    {
        if (_cameraController.settings.camPoints.Count > 0 && camPos < _cameraController.settings.camPoints.Count - 1 && _cameraController.settings.camInPosition)
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
        if (camPos > 0 && _cameraController.settings.camInPosition)
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
