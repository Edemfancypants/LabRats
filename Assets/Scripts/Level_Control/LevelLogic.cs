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

        if (saveSystem == null)
        {
            Debug.LogWarning("No SaveSystem present in scene, there will be dragons...");
        }
        else
        {
            saveSystem.onLoadEvent += LoadCollectibles;
        }
    }

    public SaveSystem saveSystem;

	private void Start()
	{
        //UILogic.instance.Fade(false);
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

			Debug.Log("Found local gameObject: " + obj.name + " of ID: " + collectibleProperties.id);

            //Loop through all saved collectible gameObjects, and get their IDs 
            foreach (CollectibleType savedCollectible in SaveSystem.instance.saveData.collectibles)
			{
				Debug.Log("Saved collectible ID: " + savedCollectible.id);

				//check for any matches, and destroy them
				if (savedCollectible.id == collectibleProperties.id)
				{
					Destroy(obj);
                    Debug.Log("Destroyed match: " + obj.name + " of ID: " + collectibleProperties.id);
                }
            }
		}

		if (collectibles.Length == 0)
		{
			Debug.Log("No Collectible Objects found in the level");
		}
	}

	public IEnumerator LoadLevel(string levelToLoad)
	{
        UILogic.instance.Fade(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(levelToLoad);
    }

	public IEnumerator RestartLevel()
	{
		UILogic.instance.Fade(true);
		yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
