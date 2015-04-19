using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AbstractActor : MonoBehaviour {
	public	Animator	anim;

	protected			bool	thisCanDespawn;
	protected			bool	thisCanUpdate;
	public virtual bool canDespawn { get { return thisCanDespawn; } }
	public virtual bool canUpdate { get { return thisCanUpdate && canDespawn; } }

	public virtual bool doAttack { get { return true; } }

	public virtual void DoDamage( float percent ) {
		// Temporary thing
		gameObject.GetComponent<Renderer>().material.color = Color.Lerp( Color.red, Color.white, percent );
	}

	protected virtual void Awake() {
		if( anim == null ) { anim = gameObject.GetComponent<Animator>(); }
		Spawn();
	}

	// What to do when the actor is created
	public virtual void Spawn() {
		thisCanDespawn = true;
	}

	// What to do when the actor is destroyed
	public virtual void Despawn() {
		if( !canDespawn ) return;	// Do nothing if the actor is already despawned

		thisCanDespawn = false;
	}

	// What to do when the actor is recreated
	public virtual void Respawn() {
	}

	public virtual void Autofill() {
		anim = gameObject.GetComponent<Animator>();
		if( anim == null ) {
			Debug.LogError( "Autofill ERROR: Missing Animator" );
		}
	}

#if UNITY_EDITOR
	public virtual void Reset() {
		if( anim == null ) anim = gameObject.GetComponent<Animator>();
	}
#endif
}

#if UNITY_EDITOR
[CustomEditor( typeof( AbstractActor ) )]
[CanEditMultipleObjects]
public class AbstractActorEditor : EditorExtended {
	public override void AttemptAutofill() {
		foreach( Object tar in targets ) {
			( (AbstractActor)tar ).Autofill();
		}
	}
}
#endif
