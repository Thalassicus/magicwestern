using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour 
{
	// Reference to PlayerActor script attached to player
	public PlayerActor linkedActor;

	// Ability script references
	public AbstractAbility ability1;
	public AbstractAbility ability2;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!ability1.doAbility1) return;
		Debug.Log ("Ability 1");
		ability1.AbilityTrigger();
		if(!ability2.doAbility2) return;
		Debug.Log ("Ability 2");
		ability2.AbilityTrigger();
	}
}
