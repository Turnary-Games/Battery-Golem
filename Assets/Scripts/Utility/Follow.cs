using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

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
	[Tooltip("Following mode\n\n" +
			 "<lerp> The script will move based on the unity lerp.\n\n" +
			 "<direct> The script will move instantly to the targets position + the offset. The speed variable does nothing.\n\n" + 
			 "<parent> Same as direct, but will move the delta position instead.")]
	public FollowMode mode;
	[Tooltip("If inverse is true, it will move the target after this one, instead of the other way around.")]
	public bool inverse = false;

	[HideInInspector]
	public Vector3 offset;
	private Vector3 lastPos;
	private bool validTarget = false;

	// Use this for initialization
	void Start () {
		DoActionIfInvalid ();

		if (validTarget)
			UpdateOffset ();
	}

	// Update is called once per frame
	void LateUpdate () {
		DoActionIfInvalid ();

		if (validTarget) {
			Transform refObj = targetObj;
			Transform movingObj = transform;
			Vector3 objOffset = offset;

			if (inverse) {
				refObj = transform;
				movingObj = targetObj;
				objOffset = -objOffset;
			}

			switch (mode) {
				case FollowMode.lerp:
					Vector3 pos = refObj.position + objOffset;
					movingObj.position = Vector3.Lerp(movingObj.position, pos, speed * Time.deltaTime);
					break;

				case FollowMode.direct:
					movingObj.position = refObj.position + objOffset;
					break;

				case FollowMode.parent:
					Vector3 delta = refObj.position - lastPos;
					movingObj.position += delta;
					lastPos = refObj.position;
					break;
			}
		}
	}

	public void UpdateOffset() {
		offset = transform.position - targetObj.position;

		lastPos = inverse ? transform.position : targetObj.position;
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
					Destroy(gameObject);
					break;

				case ActionIfInvalid.destroyThisComponent:
					Destroy(this);
					break;

				case ActionIfInvalid.disableThisGameObject:
					gameObject.SetActive(false);
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

	public enum FollowMode {
		lerp,
		direct,
		parent,
	}
}
