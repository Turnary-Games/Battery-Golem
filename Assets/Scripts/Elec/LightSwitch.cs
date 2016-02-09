using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour {

	public GameObject lightObject;

	void OnElectrify() {
		
	}

	void OnElectrifyStart() {
		print ("start");
		lightObject.SetActive (true);
	}

	void OnElectrifyEnd() {
		print ("end");
		lightObject.SetActive (false);
	}

}
