using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DamageType { GENERIC, HEAL, NUM_DAMAGE_TYPES }

[Serializable]
public class DamageInfo {
	public	float			amt			= 0;
	public float amount { get { return ( type == DamageType.HEAL ? 1 : -1 ) * amt; } set { amt = Mathf.Abs( value ); } }
	public	DamageType		type		= DamageType.GENERIC;
	public	Vector3			hitPoint	= Vector3.zero;
	public	GameObject		source;

	public DamageInfo() { }

	public DamageInfo( DamageInfo info ) : this( info.amount, info.type, info.hitPoint, info.source ) { } // clones the damageinfo
	public DamageInfo( int _amount, DamageType _type = DamageType.GENERIC ) : this( (float)_amount, _type, Vector3.zero ) { }
	public DamageInfo( float _amount, DamageType _type = DamageType.GENERIC ) : this( _amount, _type, Vector3.zero ) { }
	public DamageInfo( float _amount, DamageType _type, Vector3 position, GameObject _source = null ) {
		amount = _amount;
		type = _type;
		hitPoint = position;
		source = _source;
	}

	public static bool operator ==( DamageInfo a, float b ) { return a.amount == b; }
	public static bool operator ==( float a, DamageInfo b ) { return a == b.amount; }
	public static bool operator !=( DamageInfo a, float b ) { return a.amount != b; }
	public static bool operator !=( float a, DamageInfo b ) { return a != b.amount; }
	public static bool operator <=( DamageInfo a, float b ) { return a.amount <= b; }
	public static bool operator <=( float a, DamageInfo b ) { return a <= b.amount; }
	public static bool operator <( DamageInfo a, float b ) { return a.amount < b; }
	public static bool operator <( float a, DamageInfo b ) { return a < b.amount; }
	public static bool operator >=( DamageInfo a, float b ) { return a.amount >= b; }
	public static bool operator >=( float a, DamageInfo b ) { return a >= b.amount; }
	public static bool operator >( DamageInfo a, float b ) { return a.amount > b; }
	public static bool operator >( float a, DamageInfo b ) { return a > b.amount; }

	public static bool operator ==( DamageInfo a, int b ) { return a.amount == b; }
	public static bool operator ==( int a, DamageInfo b ) { return a == b.amount; }
	public static bool operator !=( DamageInfo a, int b ) { return a.amount != b; }
	public static bool operator !=( int a, DamageInfo b ) { return a != b.amount; }
	public static bool operator <=( DamageInfo a, int b ) { return a.amount <= b; }
	public static bool operator <=( int a, DamageInfo b ) { return a <= b.amount; }
	public static bool operator <( DamageInfo a, int b ) { return a.amount < b; }
	public static bool operator <( int a, DamageInfo b ) { return a < b.amount; }
	public static bool operator >=( DamageInfo a, int b ) { return a.amount >= b; }
	public static bool operator >=( int a, DamageInfo b ) { return a >= b.amount; }
	public static bool operator >( DamageInfo a, int b ) { return a.amount > b; }
	public static bool operator >( int a, DamageInfo b ) { return a > b.amount; }

	public static bool operator !=( DamageInfo a, DamageInfo b ) { return ( a.amount != b.amount ) || ( a.type != b.type ); }
	public static bool operator ==( DamageInfo a, DamageInfo b ) { return ( a.amount == b.amount ) && ( a.type == b.type ); }
	public static bool operator <=( DamageInfo a, DamageInfo b ) { return ( a.amount <= b.amount ); }
	public static bool operator <( DamageInfo a, DamageInfo b ) { return ( a.amount < b.amount ); }
	public static bool operator >=( DamageInfo a, DamageInfo b ) { return ( a.amount >= b.amount ); }
	public static bool operator >( DamageInfo a, DamageInfo b ) { return ( a.amount > b.amount ); }

	public override bool Equals( System.Object obj ) {
		if( obj == null ) { return false; }
		DamageInfo b = (DamageInfo)obj;
		if( (System.Object)b == null ) { return false; }
		return ( amount == b.amount ) && ( type == b.type );
	}

	public bool Equals( DamageInfo b ) {
		if( (object)b == null ) { return false; }
		return ( amount == b.amount ) && ( type == b.type );
	}

	public override int GetHashCode() {
		return (int)Mathf.Pow( amount, Convert.ToInt32( type ) );
	}

	public override string ToString() {
		return "" + amount;
	}
}

#if !UNITY_EDITOR
[CustomPropertyDrawer( typeof( DamageInfo ) )]
public class DamageInfoDrawer : PropertyDrawerExtended {
	protected override void DrawProperties() {
		DrawLabeledProperty( "Amount: ", "amount", 0.22f, 0.16f );
		DrawLabeledProperty( "", "type", 0f, 0.31f );
		DrawLabeledProperty( "", "source", 0f, 0.31f );
	}
}
#endif