using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{

    public static LevelLogic instance;

    private void Awake()
    {
        instance = this;
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    public SaveSystem saveSystem;

    private void Start()
    {
        if (saveSystem == null)
        {
            Debug.LogWarning("<b>[LevelLogic]</b> LevelLogic script detected no SaveSystem present in the scene, there will be dragons...");
        }
        else
        {
            SaveSystem.instance.Load(() =>
            {
                Debug.Log("<b>[LevelLogic]</b> Data loaded successfully!");
                LoadCollectibles();
            });
        }

        UILogic.instance.AnimationHandler("FadeIn");
    }

    public void LoadCollectibles()
    {
        //Find all gameObjects with the "Collectible" tag
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");

        //Loop through all tagged collectible gameObjects, and get their IDs 
        foreach (GameObject obj in collectibles)
        {
            CollectibleLogic collectible = obj.GetComponent<CollectibleLogic>();
            CollectibleType collectibleProperties = collectible.collectible;

            Debug.Log("<b>[LevelLogic]</b> Found local gameObject: " + obj.name + " of ID: " + collectibleProperties.id);

            //Loop through all saved collectible gameObjects, and get their IDs 
            foreach (CollectibleType savedCollectible in SaveSystem.instance.saveData.collectibles)
            {
                Debug.Log("<b>[LevelLogic]</b> Saved collectible ID: " + savedCollectible.id);

                //check for any matches, and destroy them
                if (savedCollectible.id == collectibleProperties.id)
                {
                    Destroy(obj);
                    Debug.Log("<b>[LevelLogic]</b> Destroyed match: " + obj.name + " of ID: " + collectibleProperties.id);
                }
            }
        }

        if (collectibles.Length == 0)
        {
            Debug.Log("<b>[LevelLogic]</b> No Collectible Objects found in the level");
        }
    }

    public void StartLevelLoad(string levelToLoad)
    {
        UILogic.instance.AnimationHandler("FadeOutLoad");
        UILogic.instance.levelToLoad = levelToLoad;
    }

    public IEnumerator RestartLevel()
    {
        UILogic.instance.AnimationHandler("FadeOut");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
