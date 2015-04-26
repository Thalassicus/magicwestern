using UnityEngine;
using System.Collections;

public class FiringSpeed : WeaponMod 
{
	public SimpleWeapon weaponScript;

	void Start()
	{
		weaponScript = gameObject.GetComponent<SimpleWeapon>();
	}

	public override void ApplyBonuses()
	{
		weaponScript.attacksPerSecond += 2;
	}
}
