using System.Collections;
using System.Collections.Generic;
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
		Vector3 targetPosition = destination.position;
		Debug.Log(targetPosition);

		while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
		{
			transform.position =  Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / elevatorTime);
			yield return null;
		}

		isMoving = false;
        transform.position = targetPosition;
    }
}
