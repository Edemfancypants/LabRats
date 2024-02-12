using System.Collections;
using UnityEngine;

public class CameraMovementTrigger : MonoBehaviour
{
    public int camIndex;
    public float moveTime;
    public GameObject otherTrigger;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActivateCameraMovement());
        }
    }

    private IEnumerator ActivateCameraMovement()
    {
        yield return StartCoroutine(CameraController.instance.SetCameraPos(camIndex, moveTime));

        while (Vector3.Distance(Camera.main.transform.position, CameraController.instance.camPoints[camIndex].position) > 0.5f)
        {
            yield return null;
        }

        otherTrigger.SetActive(true);
        gameObject.SetActive(false);
    }
}
