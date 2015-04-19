using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ControlledRandom {
	private	float	escalatingPercent	= 0f;
	public	float	escalatingAmount	= 0.1f;
	public	float	flatPercent			= 0.25f;

	private	float	rand;

	public bool CheckForTrigger() {
		if( flatPercent != 0f ) {
			rand = Random.value;
			if( ( rand < escalatingPercent || rand < flatPercent ) ) {
				escalatingPercent = 0;
				return true;
			}
			escalatingPercent += escalatingAmount;
		}
		return false;
	}
	
	public static float Range( MinMax minmax ) {
		return ( Random.value * ( minmax.max - minmax.min ) ) + minmax.min;
	}

	public static float Range( MinMaxf minmax ) {
		return ( Random.value * ( minmax.max - minmax.min ) ) + minmax.min;
	}

	public static float Range( int min, int max ) {
		return ( Random.value * ( max - min ) ) + min;
	}

	public static float Range( float min, float max ) {
		return ( Random.value * ( max - min ) ) + min;
	}

	// Picks a random thing from a list of possible things based on the weights of each of yo' things
	public static int PickRandomWeighted( int[] list ) {
		int totalWeight	= 0;
		for( int i = 0; i < list.Length; i++ ) {
			totalWeight += list[i];
		}
		return PickRandomWeighted( list, totalWeight );
	}

	// Sameish as above, but with the total pre-calculated
	public static int PickRandomWeighted( int[] list, int totalWeight ) {
		int randomNumber	= Random.Range( 0, totalWeight );
		int selectedIndex	= 0;
		for( int i = 0; i < list.Length; i++ ) {
			if( randomNumber < list[i] ) {
				selectedIndex = i;
				break;
			}
			randomNumber -= list[i];
		}
		return selectedIndex;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( ControlledRandom ) )]
public class ControlledRandomDrawer : PropertyDrawerExtended {
	protected override void DrawProperties() {
		DrawLabeledProperty( "Inc%: ", "escalatingAmount", 0.2f, 0.25f );
		DrawLabeledProperty( "Flat%: ", "flatPercent", 0.3f, 0.25f );
	}
}
#endif