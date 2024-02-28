using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Transform> camPoints = new List<Transform>();
    [HideInInspector]
    public bool camInPosition;

    public IEnumerator SetCameraPos(int camPoint, float moveTime)
    {
        camInPosition = false;
        Vector3 targetPosition = camPoints[camPoint].position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Debug.Log("Moving");
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / moveTime);
            yield return null;
        }

        //Ez is egy megoldás, de a másik egyelőre smoothabb xdd

        //float elapsedTime = 0f;
        //while (elapsedTime < moveTime)
        //{
        //    elapsedTime += Time.deltaTime;
        //    float t = Mathf.Clamp01(elapsedTime / moveTime);
        //    transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        //    yield return null;
        //}

        transform.position = targetPosition;
        Debug.Log("Camera in position");
        camInPosition = true;
    }
}