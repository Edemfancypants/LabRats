using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSnap : MonoBehaviour
{

    public List<Transform> snapPoints;

    private DragObject obstacle;
    public Transform currentPosition;
    public float lerpSpeed = 5f; 

    private void Start()
    {
        obstacle = GetComponent<DragObject>();
    }

    private void SnapToPoint(int position)
    {
        if (!obstacle.isMoving)
        {
            currentPosition = snapPoints[position];
            StartCoroutine(LerpToPosition(currentPosition.position));
        }
    }

    private IEnumerator LerpToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;
        obstacle.isMoving = true;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        obstacle.isMoving = false;
        transform.position = targetPosition;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "SnapTrigger" && obstacle.isMoving == false)
        {
            int position = collision.GetComponent<PlatformSnapCheck>().ReturnPosition();
            SnapToPoint(position);
        }
    }
}