#define lazyInspector	// can be lazy about linking inspector items before building the game
using UnityEngine;
using System.Collections;

public class EnemyActor : AbstractActor {
	private	PlayerActor	player;
	private	new			Rigidbody	rigidbody;
	public	Camera		cameraBase;
	public	RaycastHit	rayHit;

	public override bool doAttack {
		get {
			Physics.Raycast( new Ray( transform.position + ( Vector3.up * 0.367f ), player.transform.position - transform.position ), out rayHit, 20, LayerMask.NameToLayer( "Projectile" ) | LayerMask.NameToLayer( "Wall" ) );
			return rayHit.collider.gameObject.GetComponent<PlayerActor>() != null;
		}
	}
	
	protected override void Awake() {
#if lazyInspector
		if( rigidbody == null ) rigidbody = gameObject.GetComponent<Rigidbody>();
		player = FindObjectOfType<PlayerActor>();
#endif
	}
	
	public override void Respawn() {
		Spawn();
		transform.position = Vector3.up * 0.6f;
		rigidbody.velocity = Vector3.zero;
	}

	public override void Despawn() {
		Destroy( gameObject );
	}

	void Update() {
		transform.LookAt( player.transform.position );
	}

	private Vector3 OrientedVector3( Vector3 inputVector ) {
		inputVector.y = 0;
		inputVector.Normalize();
		return inputVector;
	}

	void OnDrawGizmos() {
		if( rayHit.point != Vector3.zero ) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere( rayHit.point, 0.25f );
		}
	}
}
