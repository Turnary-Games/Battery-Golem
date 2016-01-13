using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	[Header("Player parent class")]

	public PlayerController parent;
	public PlayerController controller { get { return parent; } }
	public PlayerInventory inventory { get { return parent.inventory; } }
	public PlayerMovement movement { get { return this; } }
	public PlayerHealth health { get { return parent.health; } }
	public PlayerInteraction interaction { get { return parent.interaction; } }

	[Header("Object references")]

	public CharacterController character;

	[Header("Movement settings")]

	[Tooltip("Speed in meters per second.")]
	public float moveSpeed = 5;
	[Tooltip("Time in seconds it takes to reach speed of /moveSpeed/. The lower the value the higer the acceleration.")]
	public float topSpeedAfter = 0.5f;
	[Tooltip("Speed in degrees per second.")]
	public float rotSpeed = 300;
	public float jumpHeight = 5;
	[Tooltip("Should the player be able to hold space and continue jumping?")]
	public bool continuousJumping = true;
	[Tooltip("When in a too steep hill the player will be moved away from the hill in /slopeForce/ meters per second.")]
	public float slopeForce = 2f;


	[HideInInspector]
	public Vector3 outsideForces;

	// Current movement velocity
	private Vector3 motion;

	// Current slope
	private float slopeAngle;
	private Vector3 slopePoint;

	void Update() {
		if (health.dead) {
			// Stop everything when dead.
			// This includes gravity and movement.
			return;
		}

		// Move the character
		Move();

		// Rotate the character
		Rotate();
	}

	#region Movement algorithms
	void Move() {
		// Motion to apply to the character
		Vector3 targetMotion = GetAxis() * moveSpeed + Vector3.up * motion.y;

		// Pushed out from slopes
		if (character.isGrounded && slopeAngle > character.slopeLimit) {
			Vector3 delta = slopePoint - transform.position;
			delta.y = 0;
			targetMotion -= delta.normalized * slopeForce;
		}

		// Apply acceleration to motion vector
		motion = Vector3.MoveTowards(motion, targetMotion, moveSpeed * Time.deltaTime / topSpeedAfter);

		if (IsGrounded()) {
			// Jumping
			if (ShouldJump())
				motion.y = jumpHeight;
		} else {
			// Apply gravity
			motion += Physics.gravity * Time.deltaTime;
		}


		// Move the character
		character.Move((motion + outsideForces) * Time.deltaTime);
	}

	void Rotate() {
		// Vector of the (looking) axis
		Vector3 rawAxis = new Vector3(Input.GetAxisRaw("HorizontalLook"), 0, Input.GetAxisRaw("VerticalLook"));

		// Not using the looking axis input, try the movement axis
		if (rawAxis.x == 0 && rawAxis.z == 0)
			rawAxis = new Vector3(Input.GetAxisRaw("HorizontalMove"), 0, Input.GetAxisRaw("VerticalMove"));

		// Not using that one either, then dont rotate at all
		if (rawAxis.x == 0 && rawAxis.z == 0)
			return;

		// Get the angles
		Vector3 rot = character.transform.eulerAngles;
		float angle = Mathf.Atan2(rawAxis.z, -rawAxis.x) * Mathf.Rad2Deg - 90f;

		// Change the value
		rot.y = Mathf.MoveTowardsAngle(rot.y, angle, rotSpeed * Time.deltaTime);
		// Set the value
		character.transform.eulerAngles = rot;
	}

	Vector3 GetAxis() {
		// Vector of the (movement) axis
		Vector3 inputAxis = new Vector3(Input.GetAxis("HorizontalMove"), 0, Input.GetAxis("VerticalMove"));

		// Normalize it so that all directions moves the same combined speed
		Vector3 axis = inputAxis.normalized;

		// Restore the movement lerp that's lost in the normalization
		axis.x *= Mathf.Abs(inputAxis.x);
		axis.z *= Mathf.Abs(inputAxis.z);

		return axis;
	}

	// Extended version of character.isGrounded
	// This one includes the slope the controller is currently standing on
	bool IsGrounded() {
		return character.isGrounded && slopeAngle <= character.slopeLimit;
	}

	// This is combined with the IsGrounded() function
	bool ShouldJump() {
		return (continuousJumping && Input.GetButton("Jump"))
			|| (!continuousJumping && Input.GetButtonDown("Jump"));
	}
	#endregion


	void OnControllerColliderHit(ControllerColliderHit hit) {
		// Calculate slope angle (in degrees)
		slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
		slopePoint = hit.point;
	}
}
