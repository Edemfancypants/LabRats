using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

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
            gameObject.name = "SaveSystem";
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public enum BuildTargetEnum
    {
        Vita,
        PC,
    }

    [Header("Save data")]
    public SaveData saveData;

    [Header("DataPath settings")]
    public string saveName = "testSave";
    public string saveExt = ".save";

    [Header("Build Settings")]
    public BuildTargetEnum buildTarget;

    private bool dataLoaded;

    private void Start()
    {
        if (buildTarget == BuildTargetEnum.Vita)
        {
            string dataPath = "ux0:data/LabRats";

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
        }
    }

    private string GetSavePath()
    {
        switch (buildTarget)
        {
            case BuildTargetEnum.Vita:
                return "ux0:data/LabRats/" + saveName + saveExt;
            case BuildTargetEnum.PC:
                return Application.persistentDataPath + "/" + saveName + saveExt; ;
            default:
                return string.Empty;
        }
    }

    public void Save()
    {
        Debug.Log("Saving data");

        string dataPath = GetSavePath();

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath, FileMode.Create);

        serializer.Serialize(stream, saveData);
        stream.Close();
    }

    public void Load(Action onDataLoaded)
    {
        if (dataLoaded == false)
        {
            string dataPath = GetSavePath();

            if (File.Exists(dataPath))
            {
                Debug.Log("Loading data");

                var serializer = new XmlSerializer(typeof(SaveData));
                using (var stream = new FileStream(dataPath, FileMode.Open))
                {
                    saveData = serializer.Deserialize(stream) as SaveData;
                }

                dataLoaded = true;

                onDataLoaded?.Invoke();
            }
            else
            {
                Debug.LogWarning("Couldn't find data to load!");
            }
        }
        else
        {
            Debug.Log("Data already loaded, using local values");

            onDataLoaded?.Invoke();
        }
    }

    public void ClearSave()
    {
        Debug.Log("Deleted save file");
        string dataPath = GetSavePath();

        File.Delete(dataPath);

        ResetSaveData();
    }

    public void ResetSaveData()
    {
        saveData.collectibles.Clear();

        saveData.masterFloat = 0f;
        saveData.bgmFloat = 0f;
        saveData.sfxFloat = 0f;

        Save();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SaveSystem))]
public class SaveSystemEditorTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveSystem saveSystem = (SaveSystem)target;

        GUILayout.Space(10);
        GUILayout.Label("Debug Settings", EditorStyles.boldLabel);
        GUILayout.Space(5);

        DrawButton("Save", saveSystem.Save);
        DrawButton("Load", () => { saveSystem.Load(() => { }); });
        DrawButton("Delete Save", () => { saveSystem.ClearSave(); saveSystem.Load(() => { }); });
        DrawButton("Open Save Location", () => { System.Diagnostics.Process.Start(Application.persistentDataPath); });
    }

    private void DrawButton(string label, Action action)
    {
        GUILayout.Label(label, EditorStyles.boldLabel);
        if (GUILayout.Button(label))
        {
            action();
        }
    }
}
#endif