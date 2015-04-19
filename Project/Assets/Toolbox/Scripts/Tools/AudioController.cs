using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AudioType { UNKNOWN, MASTER, MUSIC, GAME, NUM }

public class AudioController : MonoBehaviour {
	public static AudioController local;
	
	public	float[]			volumeControls = new float[(int)AudioType.NUM];

	public	int				currentMusicChannel = 0;
	public	AudioSource		musicChannelOne;
	public	AudioSource		musicChannelTwo;
	
	public	bool			isCrossfading	= false;
	
	public	List<AudioHelper>	helperList	= new List<AudioHelper>();

	void Awake() {
		local = this;
		//InitializeAudioSources();
		musicChannelOne.Play();

		AudioListener.volume = ( volumeControls[(int)AudioType.GAME] );
	}

	public void InitializeAudioSources() {
		if( musicChannelOne == null ) {
			musicChannelOne	= (AudioSource)Camera.main.gameObject.AddComponent<AudioSource>(  );
		}
		musicChannelOne.playOnAwake = false;
		musicChannelOne.ignoreListenerVolume = true;
		musicChannelOne.loop = true;
		musicChannelOne.priority = 0;

		if( musicChannelTwo == null ) {
			musicChannelTwo = (AudioSource)Camera.main.gameObject.AddComponent<AudioSource>(  );
		}
		musicChannelTwo.playOnAwake = false;
		musicChannelTwo.ignoreListenerVolume = true;
		musicChannelTwo.loop = true;
		musicChannelTwo.priority = 0;

		for( int i = 0; i < volumeControls.Length; i++ ) {
			SetVolume( i, volumeControls[i] );
		}
	}

	public void SetVolume( int typeID, float newVolume ) { SetVolume( (AudioType)typeID, newVolume ); }
	public void SetVolume( AudioType type, float newVolume ) {
		volumeControls[(int)type] = newVolume;
		switch( type ) {
			case AudioType.GAME:
				AudioListener.volume = volumeControls[(int)AudioType.GAME] * volumeControls[(int)AudioType.MASTER];
				break;
			case AudioType.MUSIC:
				musicChannelOne.volume = newVolume * volumeControls[(int)AudioType.MASTER];
				musicChannelTwo.volume = newVolume * volumeControls[(int)AudioType.MASTER];
				break;
		}
		UpdateVolume( type );
		/*
		foreach( AudioHelper helper in helperList ) {
			if( (int)helper.data.type == typeID ){
				helper.UpdateVolume();
			}
		}
		//*/
	}
	
	private void UpdateVolume( AudioType type ) {
		if( type != AudioType.MASTER ) {
			foreach( GameObject obj in GameObject.FindGameObjectsWithTag( "SOUND_" + type.ToString() ) ) {
				AudioHelper helper = obj.GetComponent<AudioHelper>();
				if( helper != null ) {
					helper.UpdateVolume();
				}
			}
		} else {
			foreach( AudioHelper helper in helperList ) {
				if( helper.data.type == type ) {
					helper.UpdateVolume();
				}
			}
		}
	}
	
	public static float GetVolume( float defaultVolume, AudioType type = AudioType.UNKNOWN ) {
		return defaultVolume * local.volumeControls[(int)type] * ( type != AudioType.MASTER ? local.volumeControls[(int)AudioType.MASTER] : 1 );
	}
	
	public static void SetMusic( AudioData data, bool doCrossfade = true ) {
		if( local.currentMusicChannel == 0 ) {
			local.SetMusicHelper( data, local.musicChannelOne, local.musicChannelTwo, doCrossfade );
			local.currentMusicChannel = 1;
		} else {
			local.SetMusicHelper( data, local.musicChannelTwo, local.musicChannelOne, doCrossfade );
			local.currentMusicChannel = 0;
		}
	}

	private void SetMusicHelper( AudioData data, AudioSource fromChannel, AudioSource toChannel, bool doCrossfade ) {
		toChannel.clip = data.clip;
		if( doCrossfade ) {
			StartCoroutine( Crossfade( fromChannel, toChannel, 1f, fromChannel.volume + 0, AudioController.GetVolume( data.volume, data.type ) ) );
		} else {
			toChannel.volume	= AudioController.GetVolume( data.volume, data.type );
			toChannel.Play();
			fromChannel.volume	= 0;
			fromChannel.Stop();
		}
	}

	private IEnumerator Crossfade( AudioSource from, AudioSource to, float duration, float aVol, float bVol ) {
		isCrossfading = true;
		//a2.time	= a1.time;
		float	startTime	= Time.time;
		float	endTime		= startTime + duration;
		//to.time = from.time;	// for beat syncronization
		to.Play();
		while( Time.time < endTime ) {
			float i = ( Time.time - startTime ) / duration;
			from.volume = aVol * ( 1 - i );
			to.volume = bVol * i;

			yield return new WaitForSeconds( 0.01f ); ;
		}
		from.volume = 0;
		to.volume = bVol;
		from.Stop();
		//sourceCurrent = to;
		//sourceUnknown = from;
		//sourceUnknownVol = aVol;
		//sourceCurrentVol = bVol;
		isCrossfading = false;
	}
	
	//public static AudioSource PlayClipAt( AudioData data ) { return PlayClipAt( data, Camera.main.transform, Vector3.zero ); }
	public static AudioSource PlayClipAt( AudioData data, Vector3 position ) { return PlayClipAt( data, null, position ); }
	public static AudioSource PlayClipAt( AudioData data, Transform parent ) { return PlayClipAt( data, parent, Vector3.zero ); }
	public static AudioSource PlayClipAt( AudioData data, Transform parent, Vector3 position ) {
		GameObject tmpAudio = new GameObject( "Temp Audio" );
		if( parent != null ) {
			tmpAudio.transform.parent			= parent;
			tmpAudio.transform.localPosition	= position;
		}else{
			tmpAudio.transform.position	= position;
		}
		AudioSource audioSrc	= tmpAudio.AddComponent<AudioSource>();
		AudioHelper helper		= tmpAudio.AddComponent<AudioHelper>();
		helper.SetData( data );
		audioSrc.Play();
		Destroy( tmpAudio, data.clip.length );
		return audioSrc;
	}
}

[System.Serializable]
public class AudioData {
	public	AudioClip	clip;
	public	float		volume			= 1f;
	public	AudioType	type			= AudioType.UNKNOWN;
	public	bool		isLooping		= false;

	public AudioData() {}
	public AudioData( AudioType _type ) {
		type = _type;
		if(type == AudioType.MUSIC){
			isLooping = true;
		}
	}
	public AudioData( AudioClip _clip, AudioType _type, float _volume, bool _isLooping = false ) {
		clip		= _clip;
		type		= _type;
		volume		= _volume;
		isLooping	= _isLooping;
	}

	public void ApplyTo( ref AudioSource audioSrc ) {
		audioSrc.clip	= clip;
		audioSrc.volume	= AudioController.GetVolume( volume, type );
	}
	
	public static void ApplyTo( AudioData data, ref AudioSource audioSrc ) {
		audioSrc.clip	= data.clip;
		audioSrc.volume	= AudioController.GetVolume( data.volume, data.type );
	}

	public void UpdateVolume( ref AudioSource audioSrc ){
		audioSrc.volume = AudioController.GetVolume( volume, type );
	}

	public static void UpdateVolume( AudioData data, ref AudioSource audioSrc ) {
		audioSrc.volume = AudioController.GetVolume( data.volume, data.type );
	}
}

//*/
#if UNITY_EDITOR
[CustomEditor( typeof( AudioController ) )]
[CanEditMultipleObjects]
public class AudioControllerEditor : EditorExtended {
	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();
		if( GUILayout.Button( "Apply Settings" ) ) {
			ApplySettings();
		}
		DrawVolumeArray( serializedObject.FindProperty( "volumeControls" ), System.Enum.GetNames( typeof( AudioType ) ) );
		DrawChannelsArray( serializedObject.FindProperty( "currentMusicChannel" ) );
		//DrawDefaultInspector();
		if( EditorGUI.EndChangeCheck() ) {
			ApplySettings();
			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();
		}
	}

	public void ApplySettings() {
		foreach( Object tar in targets ) {
			AudioController controller = (AudioController)tar;
			controller.InitializeAudioSources();
			for( int i = 0; i < (int)AudioType.NUM; i++ ) {
				controller.SetVolume( i, controller.volumeControls[i] );
			}
		}
	}
	
	public void DrawVolumeArray( SerializedProperty list, string[] names ) {
		EditorGUILayout.PropertyField( list );
		if( list.isExpanded ) {
			EditorGUI.indentLevel++;
			for( int i = 0; i < list.arraySize; i++ ) {
				EditorGUILayout.Slider( list.GetArrayElementAtIndex( i ), 0, 1, new GUIContent( names[i] ) );
			}
			EditorGUI.indentLevel--;
		}
	}

	public void DrawChannelsArray( SerializedProperty list ) {
		list.isExpanded = EditorGUILayout.Foldout( list.isExpanded, new GUIContent( "Music Channels" ) );
		if( list.isExpanded ) {
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "currentMusicChannel" ), new GUIContent( "Current" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "musicChannelOne" ), new GUIContent( "Channel 0" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "musicChannelTwo" ), new GUIContent( "Channel 1" ) );
			EditorGUI.indentLevel--;
		}
	}
}


[CustomPropertyDrawer( typeof( AudioData ) )]
public class AudioDataDrawer : PropertyDrawerExtended {
	protected override int numberOfLines { get { return 4; } }

	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
		BeginProperty( position, property );
		SerializedProperty prop = property.FindPropertyRelative( "clip" );
		if( prop != null ){	// For some reason this function can start running before property is fully initialized when the object is on the director
			EditorGUI.PropertyField( data.contentPosition, prop , label );
			NextRow();
			DrawProperties();
		}
		EndProperty();
	}
	protected override void DrawProperties() {
		EditorGUI.indentLevel++;
		EditorGUI.Slider( data.contentPosition, data.property.FindPropertyRelative( "volume" ), 0f, 1f );
		NextRow();
		EditorGUI.PropertyField( data.contentPosition, data.property.FindPropertyRelative( "isLooping" ) );
		NextRow();
		EditorGUI.PropertyField( data.contentPosition, data.property.FindPropertyRelative( "type" ) );
		EditorGUI.indentLevel--;
	}
}
#endif
//*/