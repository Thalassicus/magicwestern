using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
#endif

public class SpriteRendererMimic : MonoBehaviour {
	public string		sortingLayer	= "Default";
	public int			orderInLayer	= 0;
	public FilterMode	textFilterMode	= FilterMode.Point;
	
	void Start() { ApplySettings(); }
	void Reset() { ApplySettings(); }

	public void SetLayer( string layerName ) {
		sortingLayer = layerName;
		if( gameObject.GetComponent<Renderer>() != null ) {
			gameObject.GetComponent<Renderer>().sortingLayerName = sortingLayer;
			gameObject.GetComponent<Renderer>().sortingOrder = orderInLayer;
		}
	}
	
	public void SetTextMeshFilteringMode(FilterMode mode) {
		TextMesh textMesh = gameObject.GetComponent<TextMesh>();
		if( textMesh != null ) {
			textMesh.font.material.mainTexture.filterMode = mode;
		}
		GUIText guiText = gameObject.GetComponent<GUIText>();
		if( guiText != null ) {
			guiText.font.material.mainTexture.filterMode = mode;
		}
	}

	public void ApplySettings() {
		if( gameObject.GetComponent<Renderer>() != null ) {
			gameObject.GetComponent<Renderer>().sortingLayerName = sortingLayer;
			gameObject.GetComponent<Renderer>().sortingOrder = orderInLayer;
		}
		SetTextMeshFilteringMode( textFilterMode );
	}
}

#if UNITY_EDITOR
[CustomEditor( typeof( SpriteRendererMimic ) )]
[CanEditMultipleObjects]
public class SpriteRendererMimicEditor : Editor {
	private int sortingLayerIndex = 0;

	void Reset() {
		SpriteRendererMimic script	= (SpriteRendererMimic)target;
		
		string[] sortingLayerList = GetSortingLayerNames();

		for( int i = 0; i < sortingLayerList.Length; i++ ) {
			if( sortingLayerList[i].Equals( script.sortingLayer ) ) {
				sortingLayerIndex = i;
			}
		}
	}

	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();

		if( GUILayout.Button( "Apply Settings" ) ) {
			for( int i = 0; i < targets.Length; i++ ) {
				((SpriteRendererMimic) targets[i]).SetTextMeshFilteringMode( ((SpriteRendererMimic) targets[i]).textFilterMode );
			}
		}
		
		string[] sortingLayerList = GetSortingLayerNames();

		sortingLayerIndex = EditorGUILayout.Popup( "Sorting Layer", sortingLayerIndex, sortingLayerList );
		if( GUI.changed ) {
			for( int i = 0; i < targets.Length; i++ ) {
				( (SpriteRendererMimic)targets[i] ).SetLayer( sortingLayerList[sortingLayerIndex] );
				EditorUtility.SetDirty( targets[i] );
			}
		}

		EditorGUILayout.PropertyField( serializedObject.FindProperty( "orderInLayer" ) );
		EditorGUILayout.PropertyField( serializedObject.FindProperty( "textFilterMode" ) );
		
		if( EditorGUI.EndChangeCheck() ) serializedObject.ApplyModifiedProperties();
		if( GUI.changed ) {
			EditorUtility.SetDirty( target );
		}
	}

	// Get the sorting layer names
	public string[] GetSortingLayerNames() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
 
	// Get the unique sorting layer IDs -- tossed this in for good measure
	public int[] GetSortingLayerUniqueIDs() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}
}
#endif
