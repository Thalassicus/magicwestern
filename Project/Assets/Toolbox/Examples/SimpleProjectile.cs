using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Rigidbody ) )]
public class SimpleProjectile : MonoBehaviour {
	public		DamageInfo	dmgInfo;
	public new	Rigidbody	rigidbody;
	public		float		bulletSpeed	= 16f;
	private		float		bulletLifetime;

	public void Initialize( DamageInfo inputDMG ) {
		dmgInfo = inputDMG;
		rigidbody.velocity = transform.forward * bulletSpeed;
		bulletLifetime = Time.time + 4f;
	}

	void Update() {
		if( bulletLifetime < Time.time ) Destroy( gameObject );
	}

	void OnCollisionEnter( Collision other ) {
		other.gameObject.SendMessage( "ChangeHealth", dmgInfo, SendMessageOptions.DontRequireReceiver );
		Destroy( gameObject );
	}
}
