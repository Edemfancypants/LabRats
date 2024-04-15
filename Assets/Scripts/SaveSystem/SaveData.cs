using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Header("Collectibles data")]
    public List<CollectibleType> collectibles;

    [Header("Level data")]
    public List<string> unlockedLevels;

    [Header("Audio settings data")]
    public float masterFloat;
    public float bgmFloat;
    public float sfxFloat;
}
