using UnityEngine;
using System.Collections;

public class SimpleGameObjectSwitch : MonoBehaviour {

	public GameObject targetObject;
	public bool inverted;

	void Start() {
		targetObject.SetActive(inverted);
	}

	void OnElectrifyStart() {
		targetObject.SetActive(!inverted);
	}

	void OnElectrifyEnd() {
		targetObject.SetActive(inverted);
	}

}
