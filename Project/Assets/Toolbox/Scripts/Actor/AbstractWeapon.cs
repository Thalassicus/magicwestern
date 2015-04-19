using UnityEngine;
using System.Collections;

public abstract class AbstractWeapon : AbstractEquipment {
	public	DamageInfo	damageInfo		= new DamageInfo( 5, DamageType.GENERIC );
	public	float	attacksPerSecond	= 4;
	private	float	nextAttack			= 0;

	void Update() {
		if( !linkedInventory.linkedActor.doAttack  ) return;	// if the linked actor is not attacking, do nothing
		if( nextAttack >= Time.time ) return;	// if it has been too short a time since the last attack, do nothing
		nextAttack = Time.time + ( 1f / attacksPerSecond );
		Attack();
	}

	public abstract void Attack();
}
