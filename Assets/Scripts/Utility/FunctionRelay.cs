using UnityEngine;
using System.Collections;

public class FunctionRelay : MonoBehaviour {

	public GameObject target;
	[SerializeField]
	[EnumFlags]
	public Methods methods;

	/*
		I have to go here and add more functions each time I want more.
		No biggie tho ^^
	*/

	void OnFootstep() { ForwardOntoTarget(Methods.OnFootstep); }
	void OnInteractStart(PlayerController source) { ForwardOntoTarget(Methods.OnInteractStart, source); }
	void OnInteractEnd(PlayerController source) { ForwardOntoTarget(Methods.OnInteractEnd, source); }
	void OnInteract(PlayerController source) { ForwardOntoTarget(Methods.OnInteract, source); }
	void OnElectrifyStart(PlayerController source) { ForwardOntoTarget(Methods.OnElectrifyStart, source); }
	void OnElectrifyEnd(PlayerController source) { ForwardOntoTarget(Methods.OnElectrifyEnd, source); }
	void OnElectrify(PlayerController source) { ForwardOntoTarget(Methods.OnElectrify, source); }

	/*
		Just some helper functions to shorter it down
	*/

	void ForwardOntoTarget(Methods method) {
		// Check if method is inside the mask
		if (((int)methods & (int)method) > 0)
			target.SendMessage(method.ToString(), SendMessageOptions.DontRequireReceiver);
	}

	void ForwardOntoTarget(Methods method, object value) {
		// Check if method is inside the mask
		if (((int)methods & (int)method) > 0)
			target.SendMessage(method.ToString(), value, SendMessageOptions.DontRequireReceiver);
	}

	/*
		Methods enum
	*/

	[System.Flags]
	public enum Methods {
		OnFootstep			= 1 << 0,
		OnInteractStart		= 1 << 1,
		OnInteractEnd		= 1 << 2,
		OnInteract			= 1 << 3,
		OnElectrifyStart	= 1 << 4,
		OnElectrifyEnd		= 1 << 5,
		OnElectrify			= 1 << 6,
	}

}
