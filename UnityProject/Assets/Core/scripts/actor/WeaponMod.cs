using UnityEngine;
using System.Collections;

public abstract class WeaponMod : MonoBehaviour 
{
	// Mods provide bonuses to their attached weapon.

	public abstract void ApplyBonuses();
}
