using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour 
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
            GrappleLogic.instance.lineRenderer.enabled = false;
            Destroy(gameObject);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Grappleable")
		{
			Debug.Log("Grappleable");
            GrappleLogic.instance.grappledPoint = transform.position;
            Destroy(gameObject);
        }
		else
		{
			Debug.Log("Lol");
            GrappleLogic.instance.lineRenderer.enabled = false;
            Destroy(gameObject);
		}
    }
}
