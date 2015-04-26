using UnityEngine;
using System.Collections;

public class ShowColliders : MonoBehaviour {
	void OnDrawGizmos() {
		Matrix4x4 mat	= Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS( transform.position, transform.rotation, transform.lossyScale );

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube( Vector3.zero, Vector3.one );

		Gizmos.matrix = mat;
	}
}
