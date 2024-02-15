using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLogic : MonoBehaviour {

    public enum PlatformType
    {
        Moveable,
        Stationary,
        Elevator,
    }
    public PlatformType type;

    private DragObject obstacle;

    private void Start()
    {
        if (type != PlatformType.Elevator)
        {
            obstacle = GetComponent<DragObject>();
        }
    }

    public void PlatformStand(GameObject thisObject, bool state)
    {
        if (state == true) //on platform
        {
            thisObject.transform.parent = gameObject.transform;
            thisObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else //off platform
        {
            thisObject.transform.parent = null;
            thisObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (type == PlatformType.Stationary)
        {
            if (collision.transform.tag == "Player")
            {
                obstacle.isDragable = false;
                obstacle.rb.velocity = Vector3.zero;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (type == PlatformType.Stationary)
        {
            if (collision.transform.tag == "Player")
            {
                obstacle.isDragable = true;
            }
        }
    }
}
