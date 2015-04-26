using UnityEngine;
using System.Collections;

public class SimpleWeapon : AbstractWeapon {
	public GameObject	projectilePrefab;
	public Vector3		barrelSpawnPoint;

	public	Transform	fakeBullet;
	public	bool		isScalingBullet;
	public	Vector3		defaultScale;
	public	Vector3		currentScale;

	public RaycastHit[] hits;
	public int validHits;

	public void Awake() {
		defaultScale = fakeBullet.localScale;
		currentScale = fakeBullet.localScale;
		currentScale.x = 1f;
	}

	protected override void Update() {
		base.Update();	// Execute default weapon behaviour (in this case, attacking)
		if( !isScalingBullet ) return;
		if( (fakeBullet.localScale - defaultScale).magnitude > 0.001f ) {
			fakeBullet.localScale = Vector3.Lerp( fakeBullet.localScale, defaultScale, Time.deltaTime*6 );
		} else {
			fakeBullet.localScale = defaultScale;
			isScalingBullet = false;
		}
	}

	public override void Attack() {
		//GameObject obj = (GameObject)Instantiate( projectilePrefab, transform.TransformPoint( barrelSpawnPoint ), linkedInventory.linkedActor.transform.rotation );
		//SimpleProjectile proj = obj.GetComponent<SimpleProjectile>();
		//proj.Initialize( damageInfo );
		fakeBullet.localScale = currentScale;
		isScalingBullet = true;
		hits = Physics.RaycastAll(transform.TransformPoint( barrelSpawnPoint ), linkedInventory.linkedActor.transform.forward, 100 );
		int i = 0;
		validHits = 0;
		bool hasPassed = true;
		while( i < 4 && hasPassed ) {
			BreakableBarricade barricade = hits[i].collider.gameObject.GetComponent<BreakableBarricade>();
			hasPassed = barricade != null && ControlledRandom.CompareValue( 1-barricade.coverBlockChance );
			hits[i].collider.gameObject.SendMessage( "ChangeHealth", damageInfo, SendMessageOptions.DontRequireReceiver );
			validHits++;
			i++;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere( transform.TransformPoint( barrelSpawnPoint ), 0.05f );
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.black;
		for( int i = 0; i < validHits; i++ ) {
			Gizmos.DrawWireSphere( hits[i].point, 0.5f );
		}
	}
}
