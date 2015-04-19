using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DataPackageTypes { GENERIC, EXAMPLE, NUM }
[System.Serializable]
public abstract class DataPackage {
	//protected virtual int dataIdentifierNumber { get{ return 0; } }

	public abstract void ApplyPackage();
}

/*
#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( DataPackage ) )]
public class DataPackageDrawer : PropertyDrawerExtended {
	//protected	virtual		string	packageName		{ get { return "Data Package"; } }
	protected override int numberOfLines { get { return 1; } }
	
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
		BeginProperty( position, property, label );
		property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, label );
		if( property.isExpanded ){
			NextRow();
			EditorGUI.indentLevel++;
			DrawProperties();
			EditorGUI.indentLevel--;
		}
		EndProperty();
	}

	protected override void DrawProperties() {
	}
}
#endif
//*/
