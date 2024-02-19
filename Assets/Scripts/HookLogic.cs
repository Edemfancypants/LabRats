using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLogic : MonoBehaviour 
{
	[Header("Lifetime Settings")]
	public float lifeTime;
	private float lifeTimeCounter;

	public void Start()
	{
		lifeTimeCounter = lifeTime;
	}

	public void Update()
	{
		lifeTimeCounter -= Time.deltaTime;

		if (lifeTimeCounter <= 0)
		{
            GrappleGunLogic.instance.lineRenderer.enabled = false;
            Destroy(gameObject);
		}
	}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Grappleable")
        {
            GameObject grapplePoint = new GameObject();
            grapplePoint.name = "GrappledPoint";

            grapplePoint.transform.parent = collision.gameObject.transform;
            grapplePoint.transform.position = transform.position;

            GrappleGunLogic.instance.grappledPoint = grapplePoint.transform;
            GrappleGunLogic.instance.isGrappled = true;
            GrappleGunLogic.instance.GrappleAttach();

            Destroy(gameObject);
        }
        else
        {
            GrappleGunLogic.instance.lineRenderer.enabled = false;
            Destroy(gameObject);
        }
    }
}
