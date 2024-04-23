using UnityEngine;

public class POILogic : MonoBehaviour
{
    public CutsceneLogic cutsceneLogic;

    public void ResetPlayerRotationOnAction(PlayerController player)
    {
        Debug.Log("<b>[POILogic]</b> Player exited POI trigger, or played cutscene on GameObject: " + cutsceneLogic.gameObject.name);

        player.lookRotationPoint = null;
        player.lookRotationLock = false;

        player.RotatePlayerModel(player.moveDir);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && cutsceneLogic.cutscenePlayed == false)
        {
            Debug.Log("<b>[POILogic]</b> Player entered POI trigger on GameObject: " + cutsceneLogic.gameObject.name);

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.lookRotationPoint = transform;
            player.lookRotationLock = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ResetPlayerRotationOnAction(collision.gameObject.GetComponent<PlayerController>());
        }
    }
}
