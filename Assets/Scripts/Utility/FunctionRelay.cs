using UnityEngine;
using System.Collections;

public class FunctionRelay : MonoBehaviour {

	public GameObject target;

	/*
		I have to go here and add more functions each time I want more.
		No biggie tho ^^
	*/

	void OnFootstep() { ForwardOntoTarget("OnFootstep"); }

	/*
		Just some helper functions to shorter it down
	*/

	void ForwardOntoTarget(string methodName) {
		target.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
	}

	void ForwardOntoTarget(string methodName, object value) {
		target.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
	}

}
