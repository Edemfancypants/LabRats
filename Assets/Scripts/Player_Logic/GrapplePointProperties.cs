using UnityEngine;

public class GrapplePointProperties : MonoBehaviour 
{

	[System.Serializable]
	public class GrappleProperties
	{
		public float range;
		public float spring;
		public float damping;
	}
	public GrappleProperties properties;
}
