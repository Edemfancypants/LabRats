using UnityEngine;

public class SecretCollectibleUnlock : MonoBehaviour
{
    private KeyCode[] konamiCode = {KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow,
                                     KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
                                     KeyCode.B, KeyCode.A};
    private int currentInputIndex = 0;

    public CollectibleManager collectibleManager;
    public SaveSystem saveSystem;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[currentInputIndex]))
            {
                currentInputIndex++;

                if (currentInputIndex == konamiCode.Length)
                {
                    Debug.Log("Konami code entered!");
                    ActivateSecretCollectible();
                }
            }
            else
            {
                currentInputIndex = 0;
            }
        }
    }

    private void ActivateSecretCollectible()
    {
        CollectibleType collectible_KeyBoard = new CollectibleType();
        collectible_KeyBoard.id = 9;
        collectible_KeyBoard.name = "Keyboard";

        if (!saveSystem.saveData.collectibles.Contains(collectible_KeyBoard))
        {
            AudioLogic.instance.PlaySFX("KonamiJingle");

            saveSystem.saveData.collectibles.Add(collectible_KeyBoard);

            saveSystem.Save();
            saveSystem.dataLoaded = false;

            saveSystem.Load(() =>
            {
                collectibleManager.ActivateCollectibles();
            });
        }        

        this.enabled = false;
    }
}
