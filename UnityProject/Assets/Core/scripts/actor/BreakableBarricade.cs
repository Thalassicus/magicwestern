using UnityEngine;
using System.Collections;

public class BreakableBarricade : AbstractActor {
	private new Renderer renderer;

	public bool isFullyDestroyable	= false;

	public float coverBlockChance	= 1f;

	protected override void Awake() {
		base.Awake();
		if( renderer == null ) renderer = gameObject.GetComponent<Renderer>();
	}

	public override void Despawn() {
		if( isFullyDestroyable ) {
			Destroy( gameObject );
		} else {
			renderer.material.color = new Color( 0.1f, 0.1f, 0.1f, 1f );
		}
	}

	public override void DoDamage( float percent ) {
		coverBlockChance = percent;
		gameObject.GetComponent<Renderer>().material.color = Color.Lerp( Color.red, Color.white, percent );
	}
}
