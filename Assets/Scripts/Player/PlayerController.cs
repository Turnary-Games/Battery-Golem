using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public CharacterController character;

	[Header("Movement settings")]

	[Tooltip("Speed in meters per second.")]
	public float moveSpeed = 1;
	public float jumpHeight = 1;
	public bool continuousJumping = true;

	// Current movement velocity
	private Vector3 motion;

	void Update () {
		Movement ();
	}

	void Movement() {
		// Motion to apply to the character
		motion = GetAxis () * moveSpeed + Vector3.up * motion.y;

		if (character.isGrounded) {
			// Jumping
			if (IsJumping())
				motion.y = jumpHeight;
		} else {
			// Apply gravity
			motion += Physics.gravity * Time.deltaTime;
		}

		// Move the character
		character.Move (motion * Time.deltaTime);

		// Rotate the character
		if (motion.x != 0 || motion.z != 0)
			character.transform.eulerAngles = Vector3.up * (Mathf.Atan2 (motion.z, -motion.x) * Mathf.Rad2Deg - 90f);
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

	bool IsJumping() {
		return (continuousJumping && Input.GetButton ("Jump"))
			|| (!continuousJumping && Input.GetButtonDown ("Jump"));
	}
}
