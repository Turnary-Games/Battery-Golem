using UnityEngine;
using System.Collections;

public class FollowLerp : MonoBehaviour {

	[Tooltip("The object to follow")]
	public Transform targetObj;
	[Tooltip("The speed of which the object follows")]
	public float speed;
	[Tooltip("What action should the script take if the target object is invalid, " +
	         "for example if it's not assigned, or if it gets destroyed while the script is active, etc.\n\n" +
	         "<doNothing> This script will do nothing while the target is not valid and this action is selected.\n\n" +
	         "<destroyThisGameObject> Will destroy the GameObject this script is assigned to.\n\n" +
	         "<destroyThisComponent> Will remove this instance of the script from its GameObject, but will not remove the GameObject itself.\n\n" +
	         "<disableThisGameObject> Will disable the GameObject this script is assigned to.\n\n" +
	         "<disableThisComponent> Will disable this instance of the script, but will not disable the GameObject it is assigned to.")]
	public ActionIfInvalid actionIfInvalid;

	[HideInInspector]
	public Vector3 offset;
	private bool validTarget = false;

	// Use this for initialization
	void Start () {
		DoActionIfInvalid ();

		if (validTarget)
			UpdateOffset ();
	}

	// Update is called once per frame
	void Update () {
		DoActionIfInvalid ();

		if (validTarget) {
			Vector3 targetPos = targetObj.position + offset;
			transform.position = Vector3.Lerp (transform.position, targetPos, speed * Time.deltaTime);
		}
	}

	public void UpdateOffset() {
		offset = transform.position - targetObj.position;
	}

	public bool IsInvalid() {
		return targetObj == null;
	}

	void DoActionIfInvalid() {
		if (IsInvalid ()) {
			validTarget = false;

			switch (actionIfInvalid) {
			case ActionIfInvalid.doNothing:
				break;

			case ActionIfInvalid.destroyThisGameObject:
				Destroy (gameObject);
				break;

			case ActionIfInvalid.destroyThisComponent:
				Destroy (this);
				break;

			case ActionIfInvalid.disableThisGameObject:
				gameObject.SetActive (false);
				break;

			case ActionIfInvalid.disableThisComponent:
				enabled = false;
				break;
			}
		} else {
			validTarget = true;
		}
	}

	public enum ActionIfInvalid {
		doNothing,
		destroyThisGameObject,
		destroyThisComponent,
		disableThisGameObject,
		disableThisComponent
	};
}
