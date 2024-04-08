using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour {

    public SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    private void Start()
    {
        if (saveSystem == null)
        {
            Debug.LogWarning("<b>[CollectibleManager]</b> CollectibleManager script detected no SaveSystem present in the scene, there will be dragons...");
        }
        else
        {
            SaveSystem.instance.Load(() => {
                ActivateCollectibles();
            });
        }
    }

    public void ActivateCollectibles()
    {
        //Find all gameObjects with the "Collectible" tag
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");

        //Loop through all tagged collectible gameObjects, and get their IDs 
        foreach (GameObject obj in collectibles)
        {
            obj.SetActive(false);

            CollectibleLogic collectible = obj.GetComponent<CollectibleLogic>();
            CollectibleType collectibleProperties = collectible.collectible;

            //Loop through all saved collectible gameObjects, and get their IDs 
            foreach (CollectibleType savedCollectible in SaveSystem.instance.saveData.collectibles)
            {
                Debug.Log("<b>[CollectibleManager]</b> Saved collectible ID: " + savedCollectible.id);

                //check for any matches, and activate them
                if (savedCollectible.id == collectibleProperties.id)
                {
                    obj.gameObject.SetActive(true);
                    Debug.Log("<b>[CollectibleManager]</b> Activated collectible in the scene: " + obj.name + " of ID: " + collectibleProperties.id);
                }
            }
        }
    }
}
