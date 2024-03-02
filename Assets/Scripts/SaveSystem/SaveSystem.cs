using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Xml.Serialization;

using UnityEngine;

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

    public event Action onLoadEvent;

    private void Start()
    {
        Load();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearSave();
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

    public void Load()
    {
        string dataPath = GetSavePath();

        if (File.Exists(dataPath))
        {
            Debug.Log("Loading data");

            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath, FileMode.Open);
            saveData = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            onLoadEvent.Invoke();
        }
        else
        {
            Debug.LogWarning("Couldn't find data to load!");
        }
    }

    public void ClearSave()
    {
        Debug.Log("Deleted save file");
        string dataPath = GetSavePath();

        File.Delete(dataPath);
    }
}
