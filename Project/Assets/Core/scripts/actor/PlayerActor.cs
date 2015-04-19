#define lazyInspector	// can be lazy about linking inspector items before building the game
using UnityEngine;
using System.Collections;

public class PlayerActor : AbstractActor {
	private	new			Rigidbody	rigidbody;
	public	Camera		cameraBase;

	public	float		moveForce	= 20;

	public override bool doAttack {
		get {
			return Input.GetButton( "Joy1RightButton" );
		}
	}

	protected override void Awake() {
#if lazyInspector
		if( rigidbody == null ) rigidbody = gameObject.GetComponent<Rigidbody>();
#endif
	}

	public override void Despawn() {
		Respawn();
	}

	public override void Respawn() {
		Spawn();
		transform.position	= Vector3.up * 0.6f;
		rigidbody.velocity	= Vector3.zero;
		HitPoints health	= gameObject.GetComponent<HitPoints>();
		health.current		= health.maximum;
		health.isDead		= false;
		gameObject.GetComponent<Renderer>().material.color = Color.white;
	}
	
	void Update() {
		Vector3 rotationAxis = Vector3.zero;
		rotationAxis += OrientedVector3( cameraBase.transform.forward ) * Input.GetAxisRaw( "Joy1RightStickY" );
		rotationAxis += OrientedVector3( cameraBase.transform.right ) * Input.GetAxisRaw( "Joy1RightStickX" );
		if( rotationAxis.magnitude > 0.1f ) transform.LookAt( transform.position + rotationAxis );
	}

	void FixedUpdate() {
		Vector3 inputAxis	= Vector3.zero;
		inputAxis.x = Input.GetAxisRaw( "Joy1LeftStickX" );
		inputAxis.z = Input.GetAxisRaw( "Joy1LeftStickY" );

		inputAxis = OrientedVector3( cameraBase.transform.forward ) * Input.GetAxis( "Joy1LeftStickY" ) + OrientedVector3( cameraBase.transform.right ) * Input.GetAxis( "Joy1LeftStickX" );
		rigidbody.AddForce( inputAxis * moveForce );

		if( transform.position.y < 0 ){
			Respawn();
		}
	}
	
	private Vector3 OrientedVector3( Vector3 inputVector ) {
		inputVector.y = 0;
		inputVector.Normalize();
		return inputVector;
	}
}
