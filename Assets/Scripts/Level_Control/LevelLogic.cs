using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour 
{

	public static LevelLogic instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		UILogic.instance.Fade(false);
	}
	
	public IEnumerator RestartLevel()
	{
		UILogic.instance.Fade(true);
		yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
