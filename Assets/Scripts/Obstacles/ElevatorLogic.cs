using System.Collections;
using UnityEngine;

public class ElevatorLogic : MonoBehaviour 
{

	public enum ElevatorType
	{
		Activated,
		Automated
	}
	public ElevatorType type;

	public Transform startPoint;
	public Transform endPoint;

	public float elevatorTime;
	private bool isMoving;

	private void Start()
	{
		isMoving = false;
	}

	private void Update()
	{
		if (type == ElevatorType.Automated)
		{
            if (transform.position == startPoint.position && isMoving == false)
            {
                StartCoroutine(ElevatorLerp(endPoint));
                isMoving = true;
            }
            else if (transform.position == endPoint.position && isMoving == false)
            {
                StartCoroutine(ElevatorLerp(startPoint));
                isMoving = true;
            }
        }
	}

    private void OnMouseDown()
	{
		if (type == ElevatorType.Activated)
		{
			if (transform.position == startPoint.position && isMoving == false)
			{
				StartCoroutine(ElevatorLerp(endPoint));
				isMoving = true;
			}
			else if (transform.position == endPoint.position && isMoving == false)
			{
                StartCoroutine(ElevatorLerp(startPoint));
				isMoving = true;
			}
		}
	}

    private IEnumerator ElevatorLerp(Transform destination)
    {
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = destination.position;

        float elapsedTime = 0f;

        while (elapsedTime < elevatorTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / elevatorTime);
            transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }

        isMoving = false;
        transform.position = targetPosition;
    }

}
