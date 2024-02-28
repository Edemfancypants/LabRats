using UnityEngine;

public class CollectibleLogic : MonoBehaviour 
{

	public CollectibleType collectible;

    public void AddCollectible()
    {
        bool collectibleExists = false;

        foreach (CollectibleType existingCollectible in SaveSystem.instance.saveData.collectibles)
        {
            if (existingCollectible.id == collectible.id)
            {
                collectibleExists = true;
                break;
            }
        }

        if (!collectibleExists)
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
