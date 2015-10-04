using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/* TODO: Add dynamic platforms! For example lilypads.
	 * When standing on it, make it your parent.
	 * If the lilypad moves, the player moves.
	 */

	[Header("Variables (DONT ALTER)")]

	public CharacterController character;
	public Transform electricPoint;
	public PlayerInventory inventory;

	[Header("Movement settings")]

	[Tooltip("Speed in meters per second.")]
	public float moveSpeed = 1;
	[Tooltip("Speed in degrees per second.")]
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
		Electrify ();
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
		Rotate ();
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
	// This one includes the slope the controller is currently standing on
	bool IsGrounded() {
		return character.isGrounded && slope <= character.slopeLimit;
	}

	// This is combined with the IsGrounded() function
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
		//body.AddForceAtPosition (pushDir * pushPower, hit.point);
	}

	// Try to electrify "something"
	void Electrify() {
		if (Input.GetButton("Electrify")) {
			if (inventory.equipped != null) {
				// Electrify the equipped item
				inventory.equipped.SendMessage(ElectricMethods.Electrify, SendMessageOptions.DontRequireReceiver);
			} else if (electricPoint != null) {
				// Try to electrify at your fingertips
				_ElectricListener.ElectrifyAllAt(electricPoint.position);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.isTrigger) {
			inventory.Pickup(other.gameObject);
		}
	}
}
