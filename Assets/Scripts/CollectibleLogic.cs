using UnityEngine;

public class CollectibleLogic : MonoBehaviour 
{

	public CollectibleType collectible;
	public int collectibleId;

	private void Start()
	{
		collectibleId = collectible.id;
	}

	public void AddCollectible()
	{
		if (!SaveSystem.instance.saveData.collectibles.Contains(collectible))
		{
			SaveSystem.instance.saveData.collectibles.Add(collectible);
			SaveSystem.instance.Save();
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			AddCollectible();
			Destroy(gameObject);
		}
	}
}

[System.Serializable]
public class CollectibleType
{
	public int id;
	public string name;
}
