using UnityEngine;
using System.Collections;

public abstract class AbstractWeapon : AbstractEquipment {
	public	DamageInfo	damageInfo		= new DamageInfo( 5, DamageType.GENERIC );
	public	float	attacksPerSecond	= 4;
	private	float	nextAttack			= 0;

	// Default weapon behavior - use base.Update() in child classes to access
	protected virtual void Update() {
		if( !linkedInventory.linkedActor.doAttack ) return;		// if the linked actor is not attacking, do nothing
		if( nextAttack >= Time.time ) return;					// if it has been too short a time since the last attack, do nothing
		nextAttack = Time.time + ( 1f / attacksPerSecond );		// Sets up the next point in time that the gun can fire
		Attack();												// Fires the gun
	}

	// Custom attack function for child classes that handles the actual use of the weapon
	public abstract void Attack();
}
