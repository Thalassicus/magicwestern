using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class MinMaxf {
	public	float	min;
	public float Min { get { return min; } set { min = value; } }
	public	float	max;
	public float Max { get { return max; } set { max = value; } }

	public MinMaxf( float low, float high ) {
		min = low;
		max = high;
		if( min > max ) {
			Debug.LogError( "Min(" + low + ") greater then Max(" + high + ")" );
			max = min + 1;
		}
	}
}

[System.Serializable]
public class MinMax {
	public	int	min;
	public int Min { get { return min; } set { min = value; } }
	public	int	max;
	public int Max { get { return max; } set { max = value; } }

	public MinMax( int low, int high ) {
		min = low;
		max = high;
		if( min > max ) {
			Debug.LogError( "Min(" + low + ") greater then Max(" + high + ")" );
			max = min + 1;
		}
	}
}
//*
#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( MinMaxf ) )]
public class MinMaxfDrawer : PropertyDrawerExtended {
	protected override void DrawProperties() {
		DrawLabeledProperty( "Min: ", "min", 0f, 0.35f );
		DrawLabeledProperty( "Max: ", "max", 0.3f, 0.35f );
	}
}

[CustomPropertyDrawer( typeof( MinMax ) )]
public class MinMaxDrawer : PropertyDrawerExtended {
	protected override void DrawProperties() {
		DrawLabeledProperty( "Min: ", "min", 0f, 0.35f );
		DrawLabeledProperty( "Max: ", "max", 0.3f, 0.35f );
	}
}
#endif
//*/