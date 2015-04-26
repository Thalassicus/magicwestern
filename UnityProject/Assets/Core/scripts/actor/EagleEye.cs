using UnityEngine;
using System.Collections;

public class EagleEye : AbstractAbility 
{
	// This will reveal an area on the screen or reveal enemies on screen.

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void AbilityTrigger()
	{
		Debug.Log ("Used Eagle Eye");
	}
}
