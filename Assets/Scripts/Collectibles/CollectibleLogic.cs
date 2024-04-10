using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    public enum CollectibleTypeEnum
    {
        InGame,
        Showcase
    }
    public CollectibleTypeEnum type;

    [Header("Collectible settings")]
    public CollectibleType collectible;

    [Header("TextScroll reference")]
    public TextScroll textScroll;

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

    private void OnMouseDown()
    {
        if (type == CollectibleTypeEnum.Showcase)
        {
            ConfigureTextScroll();
        }
    }

    public void ConfigureTextScroll()
    {
        textScroll.timeBetweenChars = collectible.dialogObject.timeBetweenChars;
        textScroll.fadeWaitDuration = collectible.dialogObject.fadeWaitDuration;
        textScroll.fadeDuration = collectible.dialogObject.fadeDuration;
        textScroll.sourceText = collectible.dialogObject.text;
        textScroll.enabled = true;
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
    [Header("Collectible identifiers")]
    public int id;
    public string name;

    [Header("DialogObject reference")]
    public TextObject dialogObject;
}
