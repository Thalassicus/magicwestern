using UnityEngine;
using System.Collections;

public abstract class AbstractAbility : MonoBehaviour 
{
	// This is the parent class for each ability.

	public bool doAbility1
	{
		get
		{
			return Input.GetButtonDown ("Joy1ButtonY");
		}
	}

	public bool doAbility2
	{
		get
		{
			return Input.GetButtonDown ("Joy1ButtonB");
		}
	}

	void Update()
	{

	}

	public abstract void AbilityTrigger();
}
