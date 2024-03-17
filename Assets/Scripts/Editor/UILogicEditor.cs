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

                EditorGUILayout.LabelField("AudioMixer reference", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.audioMixer"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Slider references", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.masterSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.BGMSlider"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.SFXSlider"));
                break;

            case UILogic.UIType.InGame:

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("In Game UI Settings", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.blackScreen"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.blackScreenAnim"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}