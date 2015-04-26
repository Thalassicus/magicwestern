using UnityEngine;
using System.Collections;

public class SimpleInventory : AbstractInventory 
{
	public SimpleWeapon weapon1;
	public SimpleWeapon weapon2;

	void Update()
	{
		if( !linkedActor.doAttack  ) return;

		if(!weapon1.doWeapon1) return;
		weapon1.WeaponTrigger ();
		if(!weapon2.doWeapon2) return;
		weapon2.WeaponTrigger ();
	}
}
