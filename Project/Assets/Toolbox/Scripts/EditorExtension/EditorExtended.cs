#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

public class EditorExtended : Editor {
	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();

		DrawProperties();

		if( EditorGUI.EndChangeCheck() ) serializedObject.ApplyModifiedProperties();
		if( GUI.changed ) {
			EditorUtility.SetDirty( target );
		}
	}

	public virtual void AttemptAutofill() {
		/*
		foreach( Object tar in targets ) {
			( (SimpleActor)tar ).Autofill();
		}
		//*/
	}

	public virtual void DrawProperties() {
	}

	public void DrawAutofill() {
		if( GUILayout.Button( "Attempt Autofill" ) ) {
			AttemptAutofill();
		}
	}

	public bool DrawFoldoutProperty(bool showFoldout, string internalName) {
		SerializedProperty property = serializedObject.FindProperty( internalName );
		showFoldout = EditorGUILayout.Foldout( showFoldout, property.name + " (" + property.type + ")" );
		if( showFoldout ) {
			EditorGUILayout.PropertyField(property);
		}
		return showFoldout;
	}

	public void CustomArray( SerializedProperty list, bool showArraySize = true ) { CustomArray( list, new string[0], showArraySize ); }
	public void CustomArray( SerializedProperty list, string[] names, bool showArraySize = true ) {
		//EditorGUILayout.PropertyField( list );
		list.isExpanded = EditorGUILayout.Foldout( list.isExpanded, new GUIContent( list.name ) );
		if( list.isExpanded ) {
			EditorGUI.indentLevel++;
			if( showArraySize ) {
				EditorGUILayout.PropertyField( list.FindPropertyRelative( "Array.size" ) );
			}
			for( int i = 0; i < list.arraySize; i++ ) {
				if( names.Length > 0 && names.Length > i ) {
					EditorGUILayout.PropertyField( list.GetArrayElementAtIndex( i ), new GUIContent( names[i] ) );
				} else {
					EditorGUILayout.PropertyField( list.GetArrayElementAtIndex( i ) );
				}
			}
			EditorGUI.indentLevel--;
		}
	}

	public void DrawOutlinedBox( Rect rect, Color outline, Color background ){
		Rect tmpRect	= new Rect( rect );
		if( background == Color.clear || background.a < 1 ){
			tmpRect.Set( rect.x, rect.y, 1, rect.height );
			EditorGUI.DrawRect( tmpRect, outline );

			tmpRect.Set( rect.x + rect.width - 1, rect.y, 1, rect.height );
			EditorGUI.DrawRect( tmpRect, outline );

			tmpRect.Set( rect.x, rect.y, rect.width, 1 );
			EditorGUI.DrawRect( tmpRect, outline );

			tmpRect.Set( rect.x, rect.y + rect.height, rect.width, 1 );
			EditorGUI.DrawRect( tmpRect, outline );

			tmpRect.Set( rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2 );
			EditorGUI.DrawRect( tmpRect, background );
		} else {
			EditorGUI.DrawRect( rect, outline );
			tmpRect.Set( rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2 );
			EditorGUI.DrawRect( tmpRect, background );
		}
	}

	public Rect firstRect;
	public Rect BeginPropertiesBox( Rect rect, string label ) {
		firstRect		= new Rect( rect );
		return rect;
	}
	public void EndPropertiesBox( Rect lastRect ) {
		Rect rect = firstRect;
		rect.height = lastRect.y - rect.y + lastRect.height;
		//rect.y += firstRect.height;
		rect.x -= 6;
		rect.width += 8;
		rect.height += 2;
		EditorGUI.DrawRect( rect, new Color( 0f, 0f, 0f, 0.05f ) );


		Rect tmpRect = new Rect();

		tmpRect.Set( rect.x, rect.y, 1, rect.height );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x + rect.width - 1, rect.y, 1, rect.height );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x, rect.y, rect.width, 1 );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		tmpRect.Set( rect.x, rect.y + rect.height, rect.width, 1 );
		EditorGUI.DrawRect( tmpRect, Color.grey );

		EditorGUILayout.Space();
	}
}
#endif