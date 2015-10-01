using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public CharacterController character;
	public Transform electricPoint;

	[Header("Movement settings")]

	[Tooltip("Speed in meters per second.")]
	public float moveSpeed = 1;
	[Tooltip("Speed in degrees per second.")]
	public float rotSpeed = 1;
	public float jumpHeight = 1;
	public bool continuousJumping = true;

	[Header("Pushing/pulling settings")]

	public float pushPower = 2.0F;
	public float pullPower = 2.0f;
	[Tooltip("If true, the user is required to press the required keys to push " + 
		"in the correct direction (W and D for example if you wish to push in a 45% " + 
		"angle). If false, when holding an object while pressing the forwards key " + 
		"(W for example) the player will push, no matter the direction of the player. ")]
	public bool pushRelative = true;
	[Range(0f,1f)]
	[Tooltip("When moving any movement value below the deadzone is counted as 0.")]
	public float pushDeadzone = 0.2f;
	[Tooltip("If true, only grab an object when pressing the Grab button. If false, then " + 
		"continously try to grab everything when the Grab button is held down.")]
	public bool grabOnlyOnDown = true;

	// Current movement velocity
	private Vector3 motion;

	// Object to push/pull
	public Rigidbody grabbed;

	void Update () {
		// Check if still grabbing
		if ((!Input.GetButton("Grab") || !IsGrounded()) && grabbed != null) {
			// Release the grabbed object
			grabbed = null;
		}

		Electrify ();
		Rotate();
	}

	void FixedUpdate() {
		Movement();

		// Reset slopes list
		
	}

	void Movement() {
		// Reset the motion
		motion = Vector3.up * motion.y;

		// Input axis
		Vector3 axis = GetAxis();

		// Add the push
		if (grabbed != null) {
			Vector3 pushDir = grabbed.worldCenterOfMass - transform.position;
			pushDir.Normalize();
			pushDir.y = 0;

			float amount = CalcPushAmount(pushDir, axis);
			float power = amount > 0 ? amount * pushPower : amount * pullPower;

			// Move grabbed object
			grabbed.velocity += Time.fixedDeltaTime * pushDir * power / grabbed.mass;

			// Move player
			if (pushRelative)
				motion += axis * power / grabbed.mass;
			else
				motion += pushDir * power / grabbed.mass;
		} else {
			// Just move normally
			motion += axis * moveSpeed;
		}

		if (IsGrounded()) {
			// Jumping
			if (ShouldJump())
				motion.y = jumpHeight;
		} else {
			// Apply gravity
			motion += Physics.gravity * Time.fixedDeltaTime;
		}

		// Move the character
		character.Move (motion * Time.fixedDeltaTime);
	}

	void Rotate() {
		//Vector3 rawAxis = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Vector3 rawAxis = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		if (rawAxis.x == 0 && rawAxis.z == 0)
			return;

		// Get the angles
		Vector3 rot = character.transform.eulerAngles;
		float angle = Mathf.Atan2 (rawAxis.z, -rawAxis.x) * Mathf.Rad2Deg - 90f;

		// Change the value
		rot.y = Mathf.MoveTowardsAngle (rot.y, angle, rotSpeed * Time.deltaTime);
		// Set the value
		character.transform.eulerAngles = rot;
	}

	Vector3 GetAxis() {
		// Vector of the axis
		Vector3 inputAxis = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

		// Normalize it so that all directions moves the same combined speed
		Vector3 axis = inputAxis.normalized;

		// Restore the movement lerp that's lost in the normalization
		axis.x *= Mathf.Abs (inputAxis.x);
		axis.z *= Mathf.Abs (inputAxis.z);

		return axis;
	}

	// Extended version of character.isGrounded
	bool IsGrounded() {
		return (character.collisionFlags & CollisionFlags.Below) != 0;
	}

	// This is combined with the IsGrounded() function
	bool ShouldJump() {
		return (continuousJumping && Input.GetButton ("Jump"))
			|| (!continuousJumping && Input.GetButtonDown ("Jump"));
	}

	// This is combined with the IsGrounded() function
	bool ShouldGrab() {
		return (grabOnlyOnDown && Input.GetButtonDown("Grab"))
			|| (!grabOnlyOnDown && Input.GetButton("Grab"));
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		// Check if valid hit for grabbing
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;
		
		if (hit.moveDirection.y < -0.3F)
			return;

		// Grab onto it
		if (ShouldGrab()) {
			grabbed = body;
			print("GRAB!");
		}
	}

	// Calculated the amount (from -1 to 1) based of the players rotation
	float CalcPushAmount(Vector3 pushDir, Vector3 axis) {
		if (pushRelative) {
			// Calculate relative to the pushDir, W=push if pushDir is fowards

		} else {
			// Calculate unrelative to the pushDir, W=push S=pull
			return axis.z;
		}
		return 0;
	}

	// Try to electrify "something"
	void Electrify() {
		if (electricPoint != null && Input.GetButton ("Electrify")) {
			ElectricListener.ElectrifyAllAt (electricPoint.position);
		}
	}
}
