using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleActor : AbstractActor {
	//[HideInInspector]
	public HitPoints health;

#if UNITY_EDITOR
	public override void Reset() {
		base.Reset();
		if( health == null )	health	= gameObject.GetComponent<HitPoints>();
	}
#endif
}

#if UNITY_EDITOR
[CustomEditor( typeof( SimpleActor ) )]
//[CanEditMultipleObjects]
public class SimpleActorEditor : EditorExtended {
	public override void DrawProperties() {
		DrawDefaultInspector();
		//Rect rect = EditorGUILayout.GetControlRect();
		//EditorGUI.PropertyField( rect, serializedObject.FindProperty( "health" ), new GUIContent("Hit Points") );
		//SimpleActor actor = (SimpleActor)target;
		//if( actor.health != null ) {
			//actor.health.DrawInspector( rect );
		//}
	}
}
#endif
