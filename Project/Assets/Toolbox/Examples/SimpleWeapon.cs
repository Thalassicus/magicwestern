using UnityEngine;
using System.Collections;

public class SimpleWeapon : AbstractWeapon {
	public GameObject	projectilePrefab;
	public Vector3		barrelSpawnPoint;

	public WeaponMod mod1;
	public WeaponMod mod2;
	
	public override void Attack() {
		GameObject obj = (GameObject)Instantiate( projectilePrefab, transform.TransformPoint( barrelSpawnPoint ), linkedInventory.linkedActor.transform.rotation );
		SimpleProjectile proj = obj.GetComponent<SimpleProjectile>();
		proj.Initialize( damageInfo );
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere( transform.TransformPoint( barrelSpawnPoint ), 0.05f );
	}
}
