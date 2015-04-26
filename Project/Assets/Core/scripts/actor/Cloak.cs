using UnityEngine;
using System.Collections;

public class Cloak : AbstractAbility 
{
	// Turns the player invisible for a short time.

	private PlayerActor player;
	private PlayerAbilities playAbilities;

	public bool cloaked;

	// Use this for initialization
	void Start () 
	{
		player = gameObject.GetComponent<PlayerActor>();
		playAbilities = gameObject.GetComponent<PlayerAbilities>();

		cloaked = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if(!playAbilities.ability1.doAbility1) return;
		cloaked = !cloaked;
		Debug.Log ("cloaked: " + cloaked);*/
	}

	public override void AbilityTrigger()
	{
		cloaked = !cloaked;
		Debug.Log ("cloaked: " + cloaked);
	}
}
