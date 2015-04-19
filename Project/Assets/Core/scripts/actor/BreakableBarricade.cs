using UnityEngine;
using System.Collections;

public class BreakableBarricade : AbstractActor {
	public override void Despawn() {
		Destroy( gameObject );
	}
}
