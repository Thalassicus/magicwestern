using UnityEngine;
using System.Collections;

public class MeshUtilities {
	public static Mesh CreateMesh( float width, float height ) {
		Mesh mesh = new Mesh();
		mesh.name = "ScriptedMesh";
		mesh.vertices = new Vector3[] {
			new Vector3(-width, -height, 0.01f),
			new Vector3(width, -height, 0.01f),
			new Vector3(width, height, 0.01f),
			new Vector3(-width, height, 0.01f)
		};
		mesh.uv = new Vector2[] {
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0)
		};
		mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mesh.RecalculateNormals();
		return mesh;
	}
	
	public static int FindFurthestVertexFrom( Mesh mesh, Vector3 comparePoint ) {
		int vertexIndex			= 0;
		float distance			= Vector3.Distance( comparePoint, mesh.vertices[vertexIndex] );
		float compareDistance	= 0;
		for( int i = 1; i < mesh.vertexCount; i++ ) {
			compareDistance = Vector3.Distance( comparePoint, mesh.vertices[i] );
			if( compareDistance > distance ) {
				distance	= compareDistance + 0;
				vertexIndex = i;
			}
		}
		return vertexIndex;
	}

	public static int FindClosestVertexTo( Mesh mesh, Vector3 comparePoint ) {
		int vertexIndex			= 0;
		float distance			= Vector3.Distance( comparePoint, mesh.vertices[vertexIndex] );
		float compareDistance	= 0;
		for( int i = 1; i < mesh.vertexCount; i++ ) {
			compareDistance = Vector3.Distance( comparePoint, mesh.vertices[vertexIndex] );
			if( compareDistance < distance ) {
				distance = compareDistance;
				vertexIndex = i;
			}
		}
		return vertexIndex;
	}
}
