using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	[Tooltip("The object to follow")]
	public Transform targetObj;
	
	public Vector3 offset;
	private Vector3 lastPos;
	
	void LateUpdate () {
	
		transform.position = targetObj.position + offset;
		
	}
}
