using UnityEngine;

public class DragObject : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public bool isDragable, isMoving;

    public enum DragObjectType
    {
        Position,
        Rotation
    }
    public DragObjectType type;

    private float distanceFromCamera;
    public float moveMultiplier;

    public float rotationMultiplier;
    public float rotateUpperLimit;
    public float rotateLowerLimit;

    private Vector3 lastMousePosition;

    private void Start()
    {
        rb = gameObject.transform.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        isDragable = true;
    }

    private void OnDisable()
    {
        isDragable = false;
    }

    private void Update()
    {
        distanceFromCamera = Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);

        if (type == DragObjectType.Rotation)
        {
            Vector3 currentRotation = gameObject.transform.localEulerAngles;
            currentRotation.y = Mathf.Clamp(currentRotation.y, rotateLowerLimit, rotateUpperLimit);

            gameObject.transform.localEulerAngles = currentRotation;
        }

    }

    private void OnMouseDown()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (isDragable)
        {
            if (type == DragObjectType.Position)
            {
                isMoving = true;

                Vector3 pos = Input.mousePosition;
                pos.z = distanceFromCamera;
                pos = Camera.main.ScreenToWorldPoint(pos);

                rb.velocity = (pos - gameObject.transform.position) * moveMultiplier;
            }
            else if (type == DragObjectType.Rotation)
            {
                isMoving = true;

                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                Vector3 rotationVector = new Vector3(-mouseDelta.y, -mouseDelta.x, 0f) * rotationMultiplier;

                rb.AddTorque(rotationVector);
            }

            lastMousePosition = Input.mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if (isDragable)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
        }
    }
}
