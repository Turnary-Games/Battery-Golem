using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public CharacterController character;

	[Header("Movement settings")]

	[Tooltip("Speed in meters per second.")]
	public float moveSpeed = 1;
	public float rotSpeed = 1;
	public float jumpHeight = 1;
	public bool continuousJumping = true;
	public float pushPower = 2.0F;

	// Current movement velocity
	private Vector3 motion;

	// Current slope
	private float slope;

	void Update () {
		Movement ();
	}

	void Movement() {
		// Motion to apply to the character
		motion = GetAxis () * moveSpeed + Vector3.up * motion.y;

		if (IsGrounded()) {
			// Jumping
			if (ShouldJump())
				motion.y = jumpHeight;
		} else {
			// Apply gravity
			motion += Physics.gravity * Time.deltaTime;
		}

		// Move the character
		character.Move (motion * Time.deltaTime);

		// Rotate the character
		if (motion.x != 0 || motion.z != 0)
			character.transform.rotation = Quaternion.Lerp (character.transform.rotation, Quaternion.Euler (Vector3.up * (Mathf.Atan2 (motion.z, -motion.x) * Mathf.Rad2Deg - 90f)), Time.deltaTime * rotSpeed);
	}

	Vector3 GetAxis() {
		// Vector of the axis
		Vector3 raw_axis = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

		// Normalize it so that all directions moves the same combined speed
		Vector3 axis = raw_axis.normalized;

		// Restore the movement lerp that's lost in the normalization
		axis.x *= Mathf.Abs (raw_axis.x);
		axis.z *= Mathf.Abs (raw_axis.z);

		return axis;
	}

	bool IsGrounded() {
		return character.isGrounded && slope <= character.slopeLimit;
	}

	bool ShouldJump() {
		return (continuousJumping && Input.GetButton ("Jump"))
			|| (!continuousJumping && Input.GetButtonDown ("Jump"));
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		// Calculate slope angle (in degrees)
		slope = Vector3.Angle (Vector3.up, hit.normal);

		PushObjects (hit);

	}

	void PushObjects(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;
		
		if (hit.moveDirection.y < -0.3F)
			return;
		
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;
	}
}
