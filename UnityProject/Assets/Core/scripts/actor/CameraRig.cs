using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour {
	public Transform parent;

	void Update() {
		transform.position = parent.position;
	}
}
