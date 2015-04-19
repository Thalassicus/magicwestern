using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ArrayHelpers {
	public static int ConvertTo1DIndex( int x, int y, int width ) {
		return y * width + x;
	}
	public static Vector2 ConvertTo2DIndex( int index, int width ) {
		return new Vector2( index % width, index / width );
	}
	public static T GetItemAtIndex<T>( List<T> list, int x, int y, int width ) {
		return list[ConvertTo1DIndex( x, y, width )];
	}
}
