using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogic : MonoBehaviour {

	public enum TriggerType
	{
		Restart,
		End
	}
	public TriggerType type;

	public string levelName;
	
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			switch (type)
			{
				case TriggerType.Restart:
					LevelLogic.instance.StartCoroutine(LevelLogic.instance.RestartLevel());
					break;
				case TriggerType.End:
                    LevelLogic.instance.StartCoroutine(LevelLogic.instance.LoadLevel(levelName));
					break;
            }
		}
	}		
	
}
