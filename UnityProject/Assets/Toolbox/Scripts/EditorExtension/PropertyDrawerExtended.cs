#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public abstract class PropertyDrawerExtended : PropertyDrawer {
	protected virtual int			numberOfLines	{ get{ return 1; } }
	protected int					startingIndentLevel;
	protected FieldSetupData		data;

	public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
		return base.GetPropertyHeight( property, label ) * numberOfLines;
	}

	protected void BeginProperty( Rect position, SerializedProperty property, GUIContent label ) {
		startingIndentLevel = EditorGUI.indentLevel;
		label = EditorGUI.BeginProperty( position, label, property );
		data = new FieldSetupData();
		data.property = property;
		data.contentPosition = EditorGUI.PrefixLabel( position, label );
		data.contentPosition.height /= (float)numberOfLines;
		data.defaultContentPosition = data.contentPosition;
		data.style = GUIStyle.none;
		data.style.alignment = TextAnchor.MiddleRight;
		EditorGUI.indentLevel = 0;
	}

	protected void BeginProperty( Rect position, SerializedProperty property ) {
		startingIndentLevel = EditorGUI.indentLevel;
		EditorGUI.BeginProperty( position, new GUIContent(""), property );
		data = new FieldSetupData();
		data.property = property;
		data.contentPosition = position;
		data.contentPosition.height /= (float)numberOfLines;
		data.defaultContentPosition = data.contentPosition;
		data.style = GUIStyle.none;
		data.style.alignment = TextAnchor.MiddleRight;
		//EditorGUI.indentLevel = 0;
	}

	public void EndProperty() {
		EditorGUI.EndProperty();
		EditorGUI.indentLevel = startingIndentLevel;
	}
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
		BeginProperty( position, property, label );
		DrawProperties();
		EndProperty();
	}

	protected abstract void DrawProperties();
	
	protected void DrawLabeledProperty( string displayName, string internalName, float displayNameScale, float internalNameScale ) {
		data.Set( displayName, internalName, displayNameScale, internalNameScale );

		data.contentPosition.width = data.defaultContentPosition.width * data.displayScaler;
		EditorGUI.LabelField( data.contentPosition, data.displayName, data.style );
		data.contentPosition.x += data.contentPosition.width;

		data.contentPosition.width = data.defaultContentPosition.width * data.internalScaler;
		EditorGUI.PropertyField( data.contentPosition, data.property.FindPropertyRelative( data.internalName ), GUIContent.none );
		data.contentPosition.x += data.contentPosition.width;
		data.contentPosition.width = data.defaultContentPosition.width;
	}

	protected void DrawFoldoutProperty( string internalName, bool shouldDrawLabel ) { DrawFoldoutProperty( internalName, GUIContent.none , shouldDrawLabel ); }
	protected void DrawFoldoutProperty( string internalName, GUIContent content ) { DrawFoldoutProperty( internalName, content, true ); }
	protected void DrawFoldoutProperty( string internalName, string content = "" ) { DrawFoldoutProperty( internalName, new GUIContent(content), true ); }
	protected void DrawFoldoutProperty( string internalName, GUIContent content, bool shouldDrawLabel ) {
		SerializedProperty prop = data.property.FindPropertyRelative( internalName );
		float heightStorage			= data.contentPosition.height;
		data.contentPosition.height = EditorGUI.GetPropertyHeight( prop );
		EditorGUI.PropertyField( data.contentPosition, prop, ( !shouldDrawLabel ? GUIContent.none : ( content.text.Equals( "" ) ? new GUIContent( prop.name ) : content ) ) );
		data.contentPosition.height = heightStorage;
		NextRow( EditorGUI.GetPropertyHeight( prop ) );
	}

	protected void DrawSplitProperty( string internalName, string displayName, int count, int index ) {
		SerializedProperty prop		= data.property.FindPropertyRelative( internalName );
		data.contentPosition.width	= data.defaultContentPosition.width / (float)count;
		data.contentPosition.x		+= data.contentPosition.width * index;
		EditorGUI.PropertyField( data.contentPosition, prop, new GUIContent( displayName ) );
		data.contentPosition.x		= data.defaultContentPosition.x;
		data.contentPosition.width	= data.defaultContentPosition.width;
	}
	
	protected void NextRow( float customHeight = 0f ) {
		data.contentPosition.x = data.defaultContentPosition.x;
		data.contentPosition.y += (customHeight == 0f ? data.contentPosition.height : customHeight);
	}

	public float GetTotalPropertyHeight( SerializedProperty prop ) {
		float height = 0f;
		SerializedProperty property = prop;
		while( !SerializedProperty.EqualContents( property, prop.GetEndProperty() ) ) {
			height += EditorGUI.GetPropertyHeight( property );
			property.NextVisible( true );
		}
		return height;
	}

	protected class FieldSetupData {
		public string	displayName;
		public string	internalName;
		public float	displayScaler;
		public float	internalScaler;
		public Rect		contentPosition;
		public Rect		defaultContentPosition;

		public SerializedProperty property;

		public GUIStyle	style;

		public FieldSetupData() { }

		public void Set( string thisdisplayName, string thisinternalName, float thisdisplayScaler, float thisinternalScaler ) {
			displayName = thisdisplayName;
			internalName = thisinternalName;
			displayScaler = thisdisplayScaler;
			internalScaler = thisinternalScaler;
		}
	}
}
#endif