using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueId))]
public class SaveTransform : MonoBehaviour, ISavable {

	public bool position = true;
	public bool rotation = true;
	
	public void OnLoad(Dictionary<string, object> data) {
		if (position) transform.position = (Vector3)data["transform@position"];
		if (rotation) transform.rotation = (Quaternion)data["transform@rotation"];
	}

	public void OnSave(ref Dictionary<string, object> data) {
		if (position) data["transform@position"] = transform.position;
		if (rotation) data["transform@rotation"] = transform.rotation;
	}
}
