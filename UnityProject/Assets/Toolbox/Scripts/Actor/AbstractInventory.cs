using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractInventory : MonoBehaviour {
	public	AbstractActor		linkedActor;
	public	AbstractEquipment[] equipment;
	//public List<AbstractEquipment> equipment	= new List<AbstractEquipment>();

	protected virtual void Awake() {
		if( linkedActor == null ) linkedActor = gameObject.GetComponent<AbstractActor>();
	}
}
