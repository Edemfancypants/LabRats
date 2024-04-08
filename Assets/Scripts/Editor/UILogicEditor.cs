using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UILogic))]
public class UILogicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        UILogic uiLogic = (UILogic)target;
        UILogic.UISettings settings = uiLogic.settings;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.type"));

        switch (settings.type)
        {
            case UILogic.UIType.Menu:

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Menu UI Settings", EditorStyles.boldLabel);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Menu GameObject references", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.mainPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.optionsPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.collectiblesPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.levelSelectPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.loadingPanel"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("AudioMixer reference", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.audioMixer"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Slider references", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.masterSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.BGMSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.SFXSlider"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Animation references", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.UIAnimator"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("TextScroll references", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.textScroll"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Loading Screen references", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.loadingBarFill"));
                break;

            case UILogic.UIType.InGame:

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("In Game UI Settings", EditorStyles.boldLabel);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Fade Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.blackScreen"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Pause UI Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.pauseUI"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.pauseAnimator"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Loading Screen Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.loadingPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.loadingBarFill"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}