using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class PlayerHUD : PlayerSubClass {

	[Header("HUD selection fields")]

	// Offset of the center relative to the player
	public Vector3 worldOffset;
	public float angleOffset;
	public float minDist = .5f;
	public float maxDist = 2f;
	public bool clockwise = true;
	public int intOffset;

	[Header("HUD Animated leaves")]
	public Transform leavesParent;
	public GameObject leavesPrefab;

	private int hoverMouse;
	private int hoverKey; // hotkeys

	private Vector3 mouseDelta = Vector3.zero;
	
	[HideInInspector]
	public bool isOpen;
	[HideInInspector] // serialize it, but don't show it in the inspector
	public Animator[] leaves = new Animator[4];

	/// <summary>
	/// Screen point of the center.
	/// </summary>
	public Vector3 centerScreenPoint {
		get {
			Vector3 point = Camera.main.WorldToScreenPoint(PlayerController.instance.transform.position + worldOffset);
			return new Vector3(point.x, point.y, 1);
		}
	}

	/// <summary>
	/// World point of the center relative to the world.
	/// </summary>
	public Vector3 centerWorldPoint {
		get { return Camera.main.ScreenToWorldPoint(centerScreenPoint); }
	}

	/// <summary>
	/// World point of the center relative to the camera.
	/// </summary>
	public Vector3 centerLocalWorldPoint {
		get { return Camera.main.transform.InverseTransformPoint(centerWorldPoint); }
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		if (!Application.isPlaying || !isOpen) return;

		Vector3 center = centerWorldPoint;

		UnityEditor.Handles.color = new Color(1, 1, 1, 0.1f);
		UnityEditor.Handles.DrawSolidDisc(center, Camera.main.transform.forward, minDist);
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.DrawWireDisc(center, Camera.main.transform.forward, minDist);
		UnityEditor.Handles.DrawWireDisc(center, Camera.main.transform.forward, maxDist);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(center, Camera.main.transform.TransformVector(VectorHelper.FromDegrees(angleOffset)));
		Gizmos.DrawRay(center, Camera.main.transform.TransformVector(VectorHelper.FromDegrees(90+angleOffset)));
		Gizmos.DrawRay(center, Camera.main.transform.TransformVector(VectorHelper.FromDegrees(180+angleOffset)));
		Gizmos.DrawRay(center, Camera.main.transform.TransformVector(VectorHelper.FromDegrees(270+angleOffset)));

		Gizmos.color = Color.white;
		Gizmos.DrawLine(center, Camera.main.transform.TransformPoint(centerLocalWorldPoint + mouseDelta));

	}
#endif

	void Start() {
		SetHUDVisable(false);
	}

	void Update() {
		// Read input
		if (Input.GetButtonDown("Inventory")) {
			SetHUDVisable(true);
		}

		if (isOpen) {
			for (int slot = 1; slot <= 4; slot++) {
				if (hoverKey == slot - 1 && Input.GetButtonUp("Slot " + slot)) {
					SetHUDVisable(false);
					inventory.Equip(hoverKey);
					return;
				}
			}

			// Hotkeys
			bool allUp = true;
			for (int slot = 1; slot <= 4; slot++) {
				if (Input.GetButtonDown("Slot " + slot)) {
					hoverKey = slot - 1;
				}
				if (Input.GetButton("Slot "+ slot)) {
					allUp = false;
				}
			}
			if (allUp) hoverKey = -1;

			// Mouse
			if (hoverKey == -1) {
				mouseDelta = CalcMouseDelta();
				hoverMouse = CalcSelected(mouseDelta.ToVector2());

				if (hoverMouse != -1 && Input.GetMouseButtonUp(0)) {
					SetHUDVisable(false);
					inventory.Equip(hoverMouse);
					return;
				}
			}

			SetHover(hoverKey == -1 ? hoverMouse : hoverKey);

			// Close inventory
			if (Input.GetButtonUp("Inventory")) {
				SetHUDVisable(false);
				if (hoverKey != -1)
					inventory.Equip(hoverKey);
            }
		}
	}

	Vector2 CalcMouseDelta() {
		// Get world points.
		Vector3 mouseHome = centerWorldPoint;
		Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
		Vector3 mouseDelta = Camera.main.transform.TransformVector(mousePoint - mouseHome);

		return new Vector2(mouseDelta.x, mouseDelta.z);
	}

	int CalcSelected(Vector2 delta) {
		float dist = delta.magnitude;
		if (dist < minDist || dist > maxDist) return -1;

		float angle = VectorHelper.ToDegrees(delta) + angleOffset;
		return (Mathf.FloorToInt((angle+360)%360 / 90) * (clockwise ? -1 : 1) + intOffset + leaves.Length) % leaves.Length;
	}

	/// <summary>
	/// Unlock a slot. This would happen if you picked up an (inventory) item for the first time.
	/// </summary>
	public void UnlockItem(int slot) {
		SetHUDVisable(true);
		Animator anim = leaves.Get(slot);
		anim.SetBool("Slow", true);
	}

	void SetHover(int slot) {
		for (int i = 0; i < inventory.coreItems.Length; i++) {
			Animator anim = leaves.Get(i);
			anim.SetBool("Hover", slot == i);
		}
	}

	void SetHUDVisable(bool state) {
		if (isOpen == state) return;

		// Loop each slot
		for (int slot = 0; slot < inventory.coreItems.Length; slot++) {
			// Update the leaves
			if (state) {
				// Add leaves
				GameObject clone = Instantiate(leavesPrefab) as GameObject;

				clone.transform.SetParent(leavesParent);
				clone.transform.localPosition = Vector3.zero;
				clone.transform.localScale = Vector3.one;
				clone.transform.localEulerAngles = new Vector3(0, 0, -slot * 90);

				Animator anim = clone.GetComponentInChildren<Animator>();
				_CoreItem item = inventory.coreItems.Get(slot);

				anim.SetBool("Open", item != null && item.unlocked);
				anim.SetBool("Slow", false);

				leaves[slot] = anim;
            } else {
				// Remove leaves
				Destroy(leaves[slot].transform.parent.gameObject);
			}
			
		}

		isOpen = state;
	}
	
}
