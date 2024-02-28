using System.Collections;
using UnityEngine;

public class ElevatorLogic : MonoBehaviour 
{

	public enum ElevatorType
	{
		Activated,
		Automated,
        EndElevator
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
			MoveElevator();
		}
	}

	public void MoveElevator()
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

    private void OnCollisionEnter(Collision collision)
    {
        //Rá kéne jönnöm miért nem hívódik a collision ha az elevator "Platform" layeren van
        if (type == ElevatorType.EndElevator && collision.gameObject.tag == "Player")
        {
            //Jó tehát ez egy rendkívül fos megoldás, de ez jutott hirtelen eszembe...
            collision.transform.SetParent(transform);
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
            playerRB.isKinematic = true;

            MoveElevator();
        }
    }
}
