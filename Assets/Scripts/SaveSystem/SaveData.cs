using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<CollectibleType> collectibles;

    public List<string> unlockedLevels;

    public float masterFloat;
    public float bgmFloat;
    public float sfxFloat;
}
