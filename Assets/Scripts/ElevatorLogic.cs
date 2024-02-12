using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLogic : MonoBehaviour 
{

	public enum ElevatorType
	{
		Activated,
		Automated
	}
	public ElevatorType type;

	public Animator elevatorAnim;
	private bool elevatorState;
    private bool isPlaying;

	private void Start()
	{
		switch (type)
		{
			case ElevatorType.Automated:
				elevatorAnim.SetBool("Automated", true);
				break;
		}
	}

    private void OnMouseDown()
	{
		if (type == ElevatorType.Activated)
		{
                if (elevatorState == false)
                {
                    elevatorAnim.Play("ElevatorUp");
                    elevatorState = true;
                }
                else
                {
                    elevatorAnim.Play("ElevatorDown");
                    elevatorState = false;
                }
        }
	}
}
