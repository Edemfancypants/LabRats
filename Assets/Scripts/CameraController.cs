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

    public IEnumerator SetCameraPos(int camPoint, float moveTime)
    {
        Vector3 targetPosition = camPoints[camPoint].position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / moveTime);
            yield return null;
        }

        transform.position = targetPosition;
    }
}