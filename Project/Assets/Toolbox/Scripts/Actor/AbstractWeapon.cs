using UnityEngine;
using System.Collections;

public abstract class AbstractWeapon : AbstractEquipment {
	public	DamageInfo	damageInfo		= new DamageInfo( 5, DamageType.GENERIC );
	public	float	attacksPerSecond	= 4;
	private	float	nextAttack			= 0;

	public	bool		useMagazine		= false;
	public	bool		reloading		= false;

	public	int			magazineCount	= 0;
	public	int			magazineMax		= 3;
	public	float		reloadTime		= 2;

	public bool doWeapon1
	{
		get
		{
			return Input.GetButtonDown ("Joy1TriggerRight");
		}
	}

	public bool doWeapon2
	{
		get
		{
			return Input.GetButtonDown ("Joy1TriggerLeft");
		}
	}

	void Update() {
		/*if( !linkedInventory.linkedActor.doAttack  ) return;	// if the linked actor is not attacking, do nothing
		if( nextAttack >= Time.time ) return;	// if it has been too short a time since the last attack, do nothing
		if( reloading ) { reloading		= false; magazineCount	= 0; }
		if( useMagazine ) {
			magazineCount++;
			if( magazineCount < magazineMax ) reloading = true;
		}
		nextAttack = Time.time + ( 1f / attacksPerSecond ) + ( reloading ? reloadTime : 0 );
		Attack();*/
	}

	public abstract void Attack();

	public virtual void WeaponTrigger()
	{
		if( nextAttack >= Time.time ) return;	// if it has been too short a time since the last attack, do nothing
		if( reloading ) { reloading		= false; magazineCount	= 0; }
		if( useMagazine ) {
			magazineCount++;
			if( magazineCount < magazineMax ) reloading = true;
		}
		nextAttack = Time.time + ( 1f / attacksPerSecond ) + ( reloading ? reloadTime : 0 );
		Attack();
	}
}
