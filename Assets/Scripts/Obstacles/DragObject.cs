using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public bool isDragable, isMoving;

    private float distanceFromCamera;
    public float moveMultiplier;

    void Start()
    {
        isDragable = true;

        rb = gameObject.transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        distanceFromCamera = Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);
    }

    void OnMouseDown()
    {
        Debug.Log(rb.gameObject.name);
    }

    void OnMouseDrag()
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

    void OnMouseUp()
    {
        rb.velocity = Vector3.zero;
        isMoving = false;
    }
}