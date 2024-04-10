using UnityEngine;

public class DragObject : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public bool isDragable, isMoving;

    private float distanceFromCamera;
    public float moveMultiplier;

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
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
    }

    private void OnMouseDrag()
    {
        if (isDragable == true)
        {
            isMoving = true;

            Vector3 pos = Input.mousePosition;
            pos.z = distanceFromCamera;
            pos = Camera.main.ScreenToWorldPoint(pos);

            rb.velocity = (pos - gameObject.transform.position) * moveMultiplier;
        }
    }

    private void OnMouseUp()
    {
        if (isDragable == true)
        {
            rb.velocity = Vector3.zero;
            isMoving = false;
        }
    }
}