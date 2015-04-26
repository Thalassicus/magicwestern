using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {
	public	LayerMask			touchInputMask	;
	
	private	List<GameObject>	touchList		= new List<GameObject>();
	private	GameObject[]		touchesOld		;
	private	List<Vector2>		touchPositions	= new List<Vector2>();
	private	Vector2[]			touchPositionsOld;

	private	Collider2D[]		hitColliders	;
	private	MonoBehaviour		testObject		;

	public	bool				lockTouchInput	= false;

	public void ProcessTouch( bool hasTouch, Touch touch ) {
		TouchPhase	inputID			= TouchPhase.Canceled;
		Vector2		inputPosition	;

		if( hasTouch ) {
			inputPosition	= Camera.main.ScreenToWorldPoint( touch.position );
			inputID			= touch.phase;
		}else{
			inputPosition	= Camera.main.ScreenToWorldPoint( Input.mousePosition );
			if( Input.GetMouseButtonDown( 0 ) ){
				inputID	= TouchPhase.Began;
			}else if( Input.GetMouseButtonUp( 0 ) ){
				inputID	= TouchPhase.Ended;
			}else if( Input.GetMouseButton( 0 ) ){
				inputID = TouchPhase.Stationary;
			}
		}
		hitColliders = Physics2D.OverlapPointAll( inputPosition );
		if( hitColliders != null && hitColliders.Length > 0 ) {
			GameObject recipient = hitColliders[0].gameObject;
			if( hitColliders.Length > 1 ) {
				foreach( Collider2D obj in hitColliders ) {
					if( obj.isTrigger ) {
						if( !recipient.GetComponent<Collider2D>().isTrigger ) {
							recipient = obj.gameObject;
						} else {
							if( obj.gameObject.layer > recipient.layer ) {
								recipient = obj.gameObject;
							}
						}
					}
				}
			}

			if( !recipient.GetComponent<Collider2D>().isTrigger || ( lockTouchInput && recipient.layer < LayerMask.NameToLayer( "HUDObject" ) ) ) {
				//Debug.Log( "Attempted input on " + recipient.name + " of layer " + LayerMask.LayerToName(recipient.layer)  + " Contained? " + World.clickableArea.Contains( inputPosition ) );
				return;
			}

			touchList.Add( recipient );

			debugLabelText = recipient.name;

			switch( inputID ) {
				case TouchPhase.Began:
					recipient.SendMessage( "OnActionDown", inputPosition, SendMessageOptions.DontRequireReceiver );
					break;
				case TouchPhase.Ended:
					recipient.SendMessage( "OnActionUp", inputPosition, SendMessageOptions.DontRequireReceiver );
					break;
				case TouchPhase.Canceled:
					recipient.SendMessage( "OnActionExit", inputPosition, SendMessageOptions.DontRequireReceiver );
					break;
				case TouchPhase.Stationary:
				case TouchPhase.Moved:
					recipient.SendMessage( "OnActionStay", inputPosition, SendMessageOptions.DontRequireReceiver );
					break;
			}
		}
	}

	void Update() {
		touchesOld = new GameObject[touchList.Count];
		touchList.CopyTo( touchesOld );
		touchList.Clear();

		touchPositionsOld = new Vector2[touchPositions.Count];
		touchPositions.CopyTo( touchPositionsOld );
		touchPositions.Clear();

		if( Input.multiTouchEnabled && Input.touchCount > 0 ) {
			foreach( Touch touch in Input.touches ) {
				touchPositions.Add( touch.position );
				ProcessTouch(true,touch);
			}
		} else {
			touchPositions.Add( Input.mousePosition );
			if( Input.GetMouseButton( 0 ) || Input.GetMouseButtonDown( 0 ) || Input.GetMouseButtonUp( 0 ) ) {
				ProcessTouch( false, new Touch() );
			}
		}

		foreach( GameObject obj in touchesOld ) {
			if( obj != null && !touchList.Contains( obj ) ) {
				obj.SendMessage( "OnMouseExit", SendMessageOptions.DontRequireReceiver );
			}
		}
	}
	
	public bool AnyTouchWithin(Rect rect){
		bool	isTouchingButton	= false;
		foreach(Vector2 pos in touchPositions){
			if( rect.Contains(pos) ){
				isTouchingButton	= true;
			}
		}
		return isTouchingButton;
	}

	void OnApplicationQuit() {
		//Globals.Game.SaveAll();
	}

	public string debugLabelText = "Nothing";
	/*
	public Rect debugLabel = new Rect(10,10,200,400);
	void OnGUI() {
		Color tmpColor	= GUI.skin.label.normal.textColor;
		GUI.skin.label.normal.textColor = Color.black;
		GUI.Label(debugLabel,"Input: " + debugLabelText);
		GUI.skin.label.normal.textColor	= tmpColor;
	}
	//*/
}
