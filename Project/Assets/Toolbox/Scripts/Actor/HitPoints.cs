using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * Required Animator variables:
 *	Integer: DamageType
 *	Trigger: ChangeHealth
 *	Trigger: Kill
 */

public class HitPoints : MonoBehaviour {
	public	AbstractActor	actor;

	public	int			current	= 100;
	public	int			maximum	= 100;

	public	bool		isDead	= false;

	protected virtual void Awake() {
		if( actor == null ) { Autofill(); }
	}

	public virtual void ChangeHealth( DamageInfo info ) {
		if( isDead ) { return; }
		if( info.amount == 0 ) { return; }
		current = (int)Mathf.Clamp( current + info.amount, 0, maximum );
		if(actor.anim != null) actor.anim.SetInteger( "DamageType", (int)info.type );
		if( current == 0 ) {
			Kill( info );
		} else {
			actor.DoDamage( current/(float)maximum );
			if( actor.anim != null ) actor.anim.SetTrigger( "ChangeHealth" );
		}
		//return true;
	}

	public void Kill( DamageInfo info ) {
		if( !isDead ) {
			isDead = true;
			if( actor.anim != null ) actor.anim.SetTrigger( "Kill" );
			actor.Despawn();
			//return true;
		}
		//return false;
	}

	public virtual void Autofill() {
		actor = gameObject.GetComponent<AbstractActor>();
		if( actor == null ) {
			Debug.LogError( "Autofill ERROR: Object [" + gameObject.name + "] Missing SimpleActor" );
		}
	}

#if UNITY_EDITOR
	public void Reset() {
		if( actor == null ){
			actor = gameObject.GetComponent<AbstractActor>();
			actor.Reset();
		}
	}
#endif

	/* UNITY_EDITOR
#if UNITY_EDITOR
	public void DrawInspector( Rect startRect ) {
		//string propBoxLabel = "Health";
		//Rect startRect = EditorGUILayout.GetControlRect();
		//EditorGUI.LabelField( startRect, propBoxLabel );
		Rect rect = new Rect();

		DrawProperties( out rect );
		rect = EditorGUILayout.GetControlRect();
		this.isDead = EditorGUI.Toggle( rect, "Is Dead?", this.isDead );
		EndPropertyBox( startRect, rect );
	}

	public virtual void DrawProperties( out Rect rect ) {
		rect = EditorGUILayout.GetControlRect();
		EditorGUI.LabelField( new Rect( rect.x + ( rect.width - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Health" ) ).x)/2, rect.y, rect.width, rect.height ), "Health" );
		rect = EditorGUILayout.GetControlRect();
		this.current = Mathf.Clamp( EditorGUI.IntField( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), this.current ), 0, this.maximum );
		this.maximum = EditorGUI.IntField( new Rect( rect.x + rect.width * 0.8f, rect.y, rect.width * 0.2f, rect.height ), this.maximum );
		EditorGUI.ProgressBar( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.6f, rect.height ), this.current / (float)this.maximum, "" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width, rect.height ), "Current" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.8f - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Maximum" ) ).x, rect.y, rect.width, rect.height ), "Maximum" );
	}

	public void EndPropertyBox( Rect firstRect, Rect lastRect ) {
		Rect rect = firstRect;
		rect.height = lastRect.y - rect.y + lastRect.height;
		rect.y += firstRect.height / 2;
		rect.x -= 6;
		rect.width += 8;
		rect.height += 2 - ( firstRect.height / 2 );
		Color backgroundColor = new Color( 0f, 0f, 0f, 0.05f );
		EditorGUI.DrawRect( new Rect( rect.x, rect.y, 7, firstRect.height / 2 ), backgroundColor );
		EditorGUI.DrawRect( new Rect( rect.x + rect.width - 20, rect.y, 20, firstRect.height / 2 ), backgroundColor );
		EditorGUI.DrawRect( new Rect( rect.x, rect.y + firstRect.height / 2, rect.width, rect.height - firstRect.height / 2 ), backgroundColor );


		Rect tmpRect = new Rect();

		tmpRect.Set( rect.x, rect.y, 1, rect.height );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x + rect.width - 1, rect.y, 1, rect.height );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x, rect.y, 7, 1 );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x + rect.width - 3, rect.y, 3, 1 );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x, rect.y + rect.height, rect.width, 1 );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		EditorGUILayout.Space();
	}
#endif
	//*/
}

#if UNITY_EDITOR
[CustomEditor( typeof( HitPoints ) )]
//[CanEditMultipleObjects]
public class HitPointsEditor : EditorExtended {
	public override void DrawProperties() {
		Rect rect = EditorGUILayout.GetControlRect();
		EditorGUI.PropertyField( rect, serializedObject.FindProperty( "actor" ) );
		
		EditorGUILayout.Space();

		rect = BeginPropertiesBox( EditorGUILayout.GetControlRect(), "Health" );
		DrawSubProperties( ref rect );

		HitPoints script = (HitPoints)target;
		rect = EditorGUILayout.GetControlRect();
		script.isDead = EditorGUI.Toggle( rect, "Is Dead?", script.isDead );

		EndPropertiesBox( rect );
	}
	
	public virtual void DrawSubProperties( ref Rect rect ) {
		HitPoints script = (HitPoints)target;
		//rect = EditorGUILayout.GetControlRect();
		EditorGUI.LabelField( new Rect( rect.x + ( rect.width - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Health" ) ).x ) / 2, rect.y, rect.width, rect.height ), "Health" );
		rect = EditorGUILayout.GetControlRect();
		script.current = Mathf.Clamp( EditorGUI.IntField( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), script.current ), 0, script.maximum );
		script.maximum = EditorGUI.IntField( new Rect( rect.x + rect.width * 0.8f, rect.y, rect.width * 0.2f, rect.height ), script.maximum );
		EditorGUI.ProgressBar( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.6f, rect.height ), script.current / (float)script.maximum, "" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width, rect.height ), "Current" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.8f - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Maximum" ) ).x, rect.y, rect.width, rect.height ), "Maximum" );
	}
}
#endif