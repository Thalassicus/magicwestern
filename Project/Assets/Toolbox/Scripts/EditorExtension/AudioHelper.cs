using UnityEngine;
using System.Collections;

[RequireComponent( typeof( AudioSource ) )]
public class AudioHelper : MonoBehaviour {
	private	AudioSource	audioSrc;
	public	AudioData	data;

	public void Awake() {
		audioSrc = gameObject.GetComponent<AudioSource>();
		//AudioController.RegisterHelper( this );
	}
	public void SetData( AudioData _data ) {
		data = _data;
		data.ApplyTo( ref audioSrc );
		gameObject.tag = "SOUND_" + data.type.ToString();
	}
	
	public void UpdateVolume() {
		data.UpdateVolume( ref audioSrc );
	}

	void OnDestroy() {
		//AudioController.UnRegisterHelper( this );
	}
}