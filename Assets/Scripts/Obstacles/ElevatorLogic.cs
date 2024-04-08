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

    private void OnCollisionEnter(Collision collision)
    {
        if (type == ElevatorType.EndElevator && collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            StartCoroutine(SetPlayerPos(collision.gameObject));
        }
    }

    private IEnumerator SetPlayerPos(GameObject player)
    {

        yield return new WaitForSeconds(.3f);

        player.transform.SetParent(transform);

        Rigidbody playerRB = player.gameObject.GetComponent<Rigidbody>();
        playerRB.isKinematic = true;

        PlayerController playerController = player.gameObject.GetComponentInChildren<PlayerController>();
        playerController.enabled = false;

        Vector3 platformPos = new Vector3();
        platformPos = transform.position;
        platformPos.y += 1f;

        player.transform.position = platformPos;

        MoveElevator();
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
