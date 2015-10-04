using UnityEngine;
using System.Collections;

public class GameObjectSwitch : MonoBehaviour {

	public GameObject targetObject;
	public bool inverted;

	void OnElectrify() {
		
	}

	void OnElectrifyStart() {
		TargetSetActive(true);
	}

	void OnElectrifyEnd() {
		TargetSetActive(false);
	}

	void TargetSetActive(bool state) {
		targetObject.SetActive(inverted ? !state : state);
	}

}
