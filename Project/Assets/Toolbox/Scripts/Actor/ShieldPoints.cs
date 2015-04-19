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

public class ShieldPoints : HitPoints {
	public	Animator	shieldAnim;

	public	int		currentShield	= 100;
	public	int		maximumShield	= 100;
	
	public float ChangeShield( DamageInfo info ) {
		if( currentShield == 0 && info.amount < 0 ) { return info.amount; }
		if( currentShield == maximumShield && info.amount > 0 ) { return 0; }
		shieldAnim.SetInteger( "DamageType", (int)info.type );
		float amt = currentShield + info.amount;
		currentShield = (int)Mathf.Clamp( currentShield + info.amount, 0, maximumShield );
		if( currentShield == 0 ) { shieldAnim.SetTrigger( "Kill" ); return amt; }
		shieldAnim.SetTrigger( "ChangeHealth" );
		return 0;
	}
	
	public override void ChangeHealth( DamageInfo info ) {
		if( currentShield > 0 && info.amount < 0 ) {
			float amount = ChangeShield( info );
			if( amount < 0 ){
				DamageInfo dmg = new DamageInfo( info );
				dmg.amount = amount;
				base.ChangeHealth( info );
			}
			return;
		}
		base.ChangeHealth( info );
	}
	
	/* UNITY_EDITOR
#if UNITY_EDITOR
	public override void DrawProperties( out Rect rect ) {
		base.DrawProperties( out rect );
		rect = EditorGUILayout.GetControlRect();
		EditorGUI.LabelField( new Rect( rect.x + ( rect.width - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Shield" ) ).x ) / 2, rect.y, rect.width, rect.height ), "Shield" );
		rect = EditorGUILayout.GetControlRect();
		this.currentShield = Mathf.Clamp( EditorGUI.IntField( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), this.currentShield ), 0, this.maximumShield );
		this.maximumShield = EditorGUI.IntField( new Rect( rect.x + rect.width * 0.8f, rect.y, rect.width * 0.2f, rect.height ), this.maximumShield );
		EditorGUI.ProgressBar( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.6f, rect.height ), this.currentShield / (float)this.maximumShield, "" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width, rect.height ), "Current" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.8f - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Maximum" ) ).x, rect.y, rect.width, rect.height ), "Maximum" );
		
	}
#endif
	//*/
}

#if UNITY_EDITOR
[CustomEditor( typeof( ShieldPoints ) )]
//[CanEditMultipleObjects]
public class ShieldPointsEditor : HitPointsEditor {
	public override void DrawSubProperties( ref Rect rect ) {
		base.DrawSubProperties( ref rect );
		ShieldPoints script = (ShieldPoints)target;
		rect = EditorGUILayout.GetControlRect();
		EditorGUI.LabelField( new Rect( rect.x + ( rect.width - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Shield" ) ).x ) / 2, rect.y, rect.width, rect.height ), "Shield" );
		rect = EditorGUILayout.GetControlRect();
		script.currentShield = Mathf.Clamp( EditorGUI.IntField( new Rect( rect.x, rect.y, rect.width * 0.2f, rect.height ), script.currentShield ), 0, script.maximumShield );
		script.maximumShield = EditorGUI.IntField( new Rect( rect.x + rect.width * 0.8f, rect.y, rect.width * 0.2f, rect.height ), script.maximumShield );
		EditorGUI.ProgressBar( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width * 0.6f, rect.height ), script.currentShield / (float)script.maximumShield, "" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.2f, rect.y, rect.width, rect.height ), "Current" );
		EditorGUI.LabelField( new Rect( rect.x + rect.width * 0.8f - GUI.skin.GetStyle( "label" ).CalcSize( new GUIContent( "Maximum" ) ).x, rect.y, rect.width, rect.height ), "Maximum" );
	}
}
#endif