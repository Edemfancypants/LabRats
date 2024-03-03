using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerLogic))]
public class TriggerSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TriggerLogic triggerLogic = (TriggerLogic)target;
        TriggerLogic.TriggerSettings settings = triggerLogic.settings;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.type"));

        switch (settings.type)
        {
            //Level End trigger 
            case TriggerLogic.TriggerType.End:

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("End Trigger Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.levelName"));
                break;

            //Animation trigger 
            case TriggerLogic.TriggerType.AnimationTrigger:

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Animation Trigger Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.animator"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.animationName"));
                break;

            //Text trigger trigger
            case TriggerLogic.TriggerType.TextTrigger:

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Text Trigger Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.dialogObject"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.useWorldCanvas"));

                if (settings.useWorldCanvas)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.textCanvas"));
                }
                else
                {
                    settings.textCanvas = null;
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
