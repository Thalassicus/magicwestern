using UnityEngine;
using UnityEditor;
using System.Collections;
#pragma warning disable 414,162

public class XtenderUtilities : EditorWindow {
	[MenuItem( "Editor Xtender/Utilities" )]
	static void Init() {
		XtenderUtilities window = (XtenderUtilities)EditorWindow.GetWindow( typeof( XtenderUtilities ) );
		window.title = "Editor Utilities";
		window.Show();
		window.position = new Rect( 20, 80, 300, 200 );
	}
	
	private string[] tabs = new string[] { "Misc", "Font", "Rename", "Set Variable", "Input" };
	private int selectedTab;
	
	void OnGUI() {
		GUILayout.Label( position + "" );
		GUILayout.Space( 3 );
		int oldValue = GUI.skin.window.padding.bottom;
		GUI.skin.window.padding.bottom = -20;
		Rect windowRect = GUILayoutUtility.GetRect( 1, 20*((tabs.Length/3)+1) );
		windowRect.x += 4;
		windowRect.width -= 7;
		selectedTab = GUI.SelectionGrid( windowRect, selectedTab, tabs, 3, "Window" );
		GUI.skin.window.padding.bottom = oldValue;

		switch( selectedTab ) {
			case 1:
				FontGUI();
				break;
			case 2:
				RenameGUI();
				break;
			case 3:
				SetVariableGUI();
				break;
			case 4:
				InputGUI();
				break;
			default:
				MiscGUI();
				break;
		}
	}
	
#region Input Utilities
	private SerializedObject	inputManager;
	private int					numberOfPlayers	= 1;
	
	private void InputGUI() {



		EditorGUILayout.LabelField("UNDER CONSTRUCTION");
		return;

		if( GUILayout.Button( "Apply Settings" ) ){
		}

		if( GUILayout.Button( "Load Defaults" ) ) {
			LoadDefaultInputSettings();
		}

		if( GUILayout.Button( "Load InputManager" ) ) {
			inputManager = LoadSavedInputManager();
		}
		
		numberOfPlayers = EditorGUILayout.Popup( numberOfPlayers, new string[] { "1 Player", "2 Players", "3 Players", "4 Players", "5 Players", "6 Players", "7 Players", "8 Players" } );

	}

	private void LoadDefaultInputSettings() {
		ClearInputManager();
		inputManager	= LoadSavedInputManager();
		
	}

	private SerializedObject LoadSavedInputManager() {
		return new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" ) );
	}
	
	private void ClearInputManager() {
		SerializedObject	serializedObject	= LoadSavedInputManager();
		SerializedProperty	axesProperty		= serializedObject.FindProperty( "m_Axes" );
		axesProperty.ClearArray();
		serializedObject.ApplyModifiedProperties();
	}

	private SerializedProperty GetChildProperty(SerializedProperty parent, string name){
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do{
			if(child.name == name) return child;
		}while(child.Next(false));
		return null;
	}

	private bool AxisDefined( string axisName ) {
		SerializedObject	serializedObject	= new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[0] );
		SerializedProperty	axesProperty		= serializedObject.FindProperty( "m_Axes" );

		axesProperty.Next( true );
		axesProperty.Next( true );
		while( axesProperty.Next(false) ){
			SerializedProperty axis = axesProperty.Copy();
			axis.Next( true );
			if( axis.stringValue == axisName ) return true;
		}
		return false;
	}

	private void AddAxis( InputAxis axis ) {
		if( AxisDefined( axis.m_Name ) ) return;
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();

		SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
		
		GetChildProperty(axisProperty, "m_Name").stringValue = axis.m_Name;
		GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
		GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
		GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
		GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
		GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
		GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
		GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
		GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
		GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
		GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
		GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
		GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
		GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
		GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;
		
		serializedObject.ApplyModifiedProperties();
	}

	private enum AxisType { KeyOrMouseButton, MouseMovement, JoystickAxis };

	private class InputAxis {
		public	string		m_Name					= "";
		public	string		descriptiveName			= "";
		public	string		descriptiveNegativeName	= "";
		public	string		negativeButton			= "";
		public	string		positiveButton			= "";
		public	string		altNegativeButton		= "";
		public	string		altPositiveButton		= "";

		public	float		gravity					= 0;
		public	float		dead					= 0;
		public	float		sensitivity				= 0;

		public	bool		snap	= false;
		public	bool		invert	= false;

		public	AxisType	type	= AxisType.KeyOrMouseButton;

		public	int			axis	= 0;
		public	int			joyNum	= 0;
	}
#endregion

#region Misc Utilities
	public GameObject	object1;
	public GameObject	object2;
	public float		percent = 0.5f;
	
	public bool showCenterObject	= false;
	
	private void MiscGUI() {
		if( GUILayout.Button( "Break Selected Prefab Connection" ) ) {
			GameObject disconnectingObj = Selection.activeGameObject;
			Selection.activeGameObject = null;
			BreakPrefabConnection( disconnectingObj );
			Selection.activeObject = disconnectingObj;
		}

		showCenterObject = EditorGUILayout.Foldout( showCenterObject, "Center and Orient Object" );

		if( showCenterObject ) {
			EditorGUI.indentLevel++;
			GUILayout.BeginHorizontal();
			GUILayout.Label( "Percent: " );
			percent = Mathf.Clamp( EditorGUILayout.FloatField( percent ), 0f, 1f );
			GUILayout.EndHorizontal();
			percent = GUILayout.HorizontalSlider( percent, 0f, 1f );
			
			GUILayout.BeginHorizontal();
			GUILayout.Label( "Start Object" );
			GUILayout.Label( "Target Object" );
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			object1 = (GameObject)EditorGUILayout.ObjectField( object1, typeof( GameObject ), true );
			object2 = (GameObject)EditorGUILayout.ObjectField( object2, typeof( GameObject ), true );
			GUILayout.EndHorizontal();

			if( GUILayout.Button( "Center Selected" ) ) {
				CenterObject( Selection.activeGameObject );
			}
			EditorGUI.indentLevel--;
		}
	}

	public void BreakPrefabConnection( GameObject obj ) {
		PrefabUtility.DisconnectPrefabInstance( obj );
		Object prefab = PrefabUtility.CreateEmptyPrefab( "Assets/dummy.prefab" );
		PrefabUtility.ReplacePrefab( obj, prefab, ReplacePrefabOptions.ConnectToPrefab );
		PrefabUtility.DisconnectPrefabInstance( obj );
		AssetDatabase.DeleteAsset( "Assets/dummy.prefab" );
	}
	
	public void CenterObject( GameObject obj ) {
		if( object1 == null ) { Debug.LogError( "Object 1 not set." ); return; }
		if( object2 == null ) { Debug.LogError( "Object 2 not set." ); return; }
		
		Vector3 direction = ( object2.transform.position - object1.transform.position );
		obj.transform.position = object1.transform.position + ( direction * 0.5f );
		obj.transform.rotation = Quaternion.LookRotation( Vector3.forward, direction.normalized );
	}
#endregion
#region Font Utilities
	public Color effectColor = Color.white;
	public float offsetDistance = 0.02f;

	public void FontGUI() {
		if( GUILayout.Button( "Delete Font Effects" ) ) {
			DeleteChildrenWithName( "Outline" );
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Offset Distance" );
		offsetDistance = EditorGUILayout.FloatField( offsetDistance );
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label( "Color" );
		effectColor = EditorGUILayout.ColorField( "", effectColor );
		GUILayout.EndHorizontal();
		if( GUILayout.Button( "Create Outline" ) ) {
			CreateOutline();
		}
		if( GUILayout.Button( "Create Drop Shadow" ) ) {
			CreateDropShadow();
		}
	}

	public void CreateOutline() {
		GameObject selected = BeginEffect();
		if( selected == null ) { return; }

		Transform[] transforms = new Transform[8];
		transforms[0] = CreateDuplicateAt( selected, new Vector3( offsetDistance, offsetDistance, 0 ), effectColor );
		transforms[1] = CreateDuplicateAt( selected, new Vector3( -offsetDistance, offsetDistance, 0 ), effectColor );
		transforms[2] = CreateDuplicateAt( selected, new Vector3( -offsetDistance, -offsetDistance, 0 ), effectColor );
		transforms[3] = CreateDuplicateAt( selected, new Vector3( offsetDistance, -offsetDistance, 0 ), effectColor );
		transforms[4] = CreateDuplicateAt( selected, new Vector3( 0, offsetDistance, 0 ), effectColor );
		transforms[5] = CreateDuplicateAt( selected, new Vector3( 0, -offsetDistance, 0 ), effectColor );
		transforms[6] = CreateDuplicateAt( selected, new Vector3( offsetDistance, 0, 0 ), effectColor );
		transforms[7] = CreateDuplicateAt( selected, new Vector3( -offsetDistance, 0, 0 ), effectColor );

		EndEffect( transforms, selected );
	}

	public void CreateDropShadow() {
		GameObject selected = BeginEffect();
		if( selected == null ) { return; }

		Transform[] transforms = new Transform[1];
		transforms[0] = CreateDuplicateAt( selected, new Vector3( -offsetDistance, -offsetDistance, 0 ), effectColor );

		EndEffect( transforms, selected );
	}

	public GameObject BeginEffect() {
		if( Selection.activeGameObject.GetComponent<TextMesh>() == null ) {
			Debug.LogError( "Attempted to give outline to non-text object!" );
			return null;
		}

		DeleteChildrenWithName( "Outline" );
		return Selection.activeGameObject;
	}

	public void EndEffect( Transform[] transforms, GameObject selected ) {
		Vector3 position;
		foreach( Transform outline in transforms ) {
			position = outline.transform.localPosition;
			outline.transform.parent = selected.transform;
			outline.transform.localPosition = position;
		}
	}

	public void DeleteChildrenWithName( string name ) {
		Transform transform = Selection.activeGameObject.transform;
		if( transform.childCount > 0 ) {
			for( int i = transform.childCount - 1; i >= 0; i-- ) {
				if( transform.GetChild( i ).name.Contains( name ) ) {
					DestroyImmediate( transform.GetChild( i ).gameObject );
				}
			}
		}
	}

	public Transform CreateDuplicateAt( GameObject rootObject, Vector3 position ) { return CreateDuplicateAt( rootObject, position, Color.white ); }
	public Transform CreateDuplicateAt( GameObject rootObject, Vector3 position, Color color ) {
		GameObject outline				= (GameObject)Object.Instantiate( rootObject );
		outline.name = rootObject.name + "Outline";
		outline.transform.localPosition = position;

		SpriteRendererMimic mimic = outline.GetComponent<SpriteRendererMimic>();
		mimic.sortingLayer = rootObject.GetComponent<Renderer>().sortingLayerName;
		mimic.orderInLayer = rootObject.GetComponent<Renderer>().sortingOrder - 1;
		outline.GetComponent<Renderer>().sortingLayerName = rootObject.GetComponent<Renderer>().sortingLayerName;
		outline.GetComponent<Renderer>().sortingOrder = rootObject.GetComponent<Renderer>().sortingOrder - 1;
		TextMesh mesh						= outline.GetComponent<TextMesh>();
		if( mesh != null ) {
			mesh.color = color;
			mesh.font.material.mainTexture.filterMode = rootObject.GetComponent<TextMesh>().font.material.mainTexture.filterMode;
		}
		return outline.transform;
	}
#endregion
#region Rename Utilities
	public string	searchName			= "Name";
	public string	replaceName			= "Names";
	public bool		searchInChildren	= false;

	private void RenameGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Search For: " );
		searchName = EditorGUILayout.TextField( searchName );
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label( "Replace With: " );
		replaceName = EditorGUILayout.TextField( replaceName );
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label( "Replace In Children: " );
		searchInChildren = EditorGUILayout.Toggle( searchInChildren );
		GUILayout.EndHorizontal();

		if( GUILayout.Button( "Rename Selection" ) ) {
			foreach( GameObject obj in Selection.gameObjects ) {
				if( searchInChildren ) {
					RecursiveRename( obj, searchName, replaceName );
				} else {
					RenameObject( obj, searchName, replaceName );
				}
			}
		}
	}

	public void RenameObject( GameObject obj, string _searchName, string _replaceName ) {
		if( obj.name.Contains( _searchName ) ) {
			Debug.Log( "Checking: " + obj.name + " for: " + _searchName + " to replace with: " + _replaceName );
			obj.name = obj.name.Replace( _searchName, _replaceName );
		}
	}

	public void RecursiveRename( GameObject obj, string _searchName, string _replaceName ) {
		RenameObject( obj, _searchName, _replaceName );
		if( obj.transform.childCount > 0 ){
			foreach( Transform child in obj.transform ) {
				RecursiveRename( child.gameObject, _searchName, _replaceName );
			}
		}
	}

#endregion
#region Set Variable
	public string	targetClass		= "ActionPhase";
	public string	targetVariable	= "speedMult";
	public string	targetValue		= "0";
	public Vector2	targetBounds	= new Vector2( -6, 5 );
	
	private void SetVariableGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Class: " + targetClass );
		//targetClass = EditorGUILayout.TextField( targetClass );
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label( "Variable: " + targetVariable );
		//targetVariable = EditorGUILayout.TextField( targetVariable );
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		GUILayout.Label("New Value:");
		targetBounds = EditorGUILayout.Vector2Field("Click Bounds", targetBounds);
		//targetValue = EditorGUILayout.FloatField( float.Parse( targetValue ) ).ToString();
		GUILayout.EndHorizontal();
		
		if( GUILayout.Button( "Set Target Variable" ) ) {
			//foreach( ActionPhase phase in (ActionPhase[])Resources.FindObjectsOfTypeAll( typeof( ActionPhase ) ) ) {
				//phase.speedMult = float.Parse( targetValue );
			//	phase.clickBounds = targetBounds;
			//}
			/*
			try {
				System.Type type = System.Type.GetType( targetClass );
				Debug.Log("Type:" + type + "Target Class: " + targetClass);
				foreach( UnityEngine.Object classDef in GameObject.FindObjectsOfType( type ) ) {
					FieldInfo field = type.GetField( targetVariable, BindingFlags.Instance );
					field.SetValue( classDef, float.Parse( targetValue ) );
				}
			} catch( System.Exception ex ) {

			}
			//*/
		}
	}
#endregion
}

#pragma warning restore 414,162