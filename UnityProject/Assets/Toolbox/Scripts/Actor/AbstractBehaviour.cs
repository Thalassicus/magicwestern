using UnityEngine;
using System.Collections;

public abstract class AbstractBehaviour {
	protected			AbstractActor	actor;
	protected			bool			thisCanDoAction	= false;
	public		virtual	bool			canDoAction		{ get { return thisCanDoAction; } }

	public abstract void DoAction();
}
