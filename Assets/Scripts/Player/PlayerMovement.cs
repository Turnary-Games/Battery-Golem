using UnityEngine;
using System.Collections;

public class PlayerMovement : PlayerSubClass {

	[Header("Object references")]

	public Rigidbody body;
	public CapsuleCollider capsule;
	public Animator anim;

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

	[Header("Ground raycasting")]

	public float groundDist = 1f;
	public LayerMask groundLayer;
	[Range(0,90)]
	public float slopeLimit = 30f;

	[HideInInspector]
	public Vector3 outsideMotion;

	[HideInInspector]
	public bool grounded;

	[HideInInspector]
	public _Platform platform;

	void Update() {
		if (health.dead) {
			// Stop everything when dead.
			// This includes gravity and movement.
			return;
		}

		RaycastGround();
		Move();
		Rotate();

		UpdateAnimator();
	}

	#region Movement algorithms
	void RaycastGround() {
		RaycastHit hit;

		if (Physics.Raycast(transform.position + Vector3.up * groundDist / 2, Vector3.down, out hit, groundDist, groundLayer)) {
			grounded = Vector3.Angle(hit.normal, Vector3.up) <= slopeLimit;

			GameObject main = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;

			_TouchListener listener = main.GetComponent<_TouchListener>();
			if (listener) listener.Touch(this);

			platform = main.GetComponent<_Platform>();
		} else {
			grounded = false;
			platform = null;
		}
	}

	private Vector3 lastOutsideMotion;
	void Move() {
		// Motion to apply to the character
		Vector3 motion = Vector3.zero;
		if (pushing.point != null)
			motion = pushing.GetMovement();
		else
			motion = GetAxis() * moveSpeed;

		//body.AddForce(force);

		// Apply acceleration to motion vector
		motion = Vector3.MoveTowards(body.velocity - lastOutsideMotion, motion, moveSpeed * Time.deltaTime / topSpeedAfter);

		if (grounded) {
			// Jumping
			if (ShouldJump())
				motion.y = jumpHeight;
		} else {
			// Apply gravity
			motion.y = body.velocity.y;
		}

		// Move the character
		//character.Move((motion + outsideForces) * Time.deltaTime);
		body.velocity = motion + outsideMotion;
		lastOutsideMotion = outsideMotion;
	}

	void UpdateAnimator() {
		// Velocity, relative to the current platform (if any)
		Vector3 motion = platform ? body.velocity - platform.body.velocity : body.velocity;
		float magn = new Vector2(motion.x, motion.z).magnitude;

		// Tell animator
		anim.SetBool("Walking", magn > 0);
		anim.SetFloat("MoveSpeed", magn > 0 ? magn / 5 : 1);
		anim.SetBool("Grounded", grounded);
		anim.SetFloat("VertSpeed", motion.y);
	}

	void Rotate() {
		// Vector of the (looking) axis
		Vector3 rawAxis;

		if (hud.isOpen)
			rawAxis = Vector3.zero;
		else if (pushing.point != null)
			rawAxis = pushing.GetAxis();
		else {
			rawAxis = new Vector3(Input.GetAxisRaw("HorizontalLook"), 0, Input.GetAxisRaw("VerticalLook"));

			// Not using the looking axis input, try the movement axis
			if (rawAxis.x == 0 && rawAxis.z == 0)
				rawAxis = new Vector3(Input.GetAxisRaw("HorizontalMove"), 0, Input.GetAxisRaw("VerticalMove"));
		}

		// No rotation
		if (rawAxis.x == 0 && rawAxis.z == 0) {
			body.angularVelocity = Vector3.zero;
			return;
		}

		// Get the angles
		Vector3 rot = transform.eulerAngles;
		float angle = Mathf.Atan2(rawAxis.z, -rawAxis.x) * Mathf.Rad2Deg - 90f;

		// Change the value
		rot.y = Mathf.MoveTowardsAngle(rot.y, angle, rotSpeed * Time.deltaTime);
		// Set the value
		body.MoveRotation(Quaternion.Euler(rot));
		//transform.eulerAngles = rot;
	}

	public Vector3 GetAxis() {
		// Vector of the (movement) axis
		Vector3 inputAxis = new Vector3(Input.GetAxis("HorizontalMove"), 0, Input.GetAxis("VerticalMove"));

		// Normalize it so that all directions moves the same combined speed
		Vector3 axis = inputAxis.normalized;

		// Restore the movement lerp that's lost in the normalization
		axis.x *= Mathf.Abs(inputAxis.x);
		axis.z *= Mathf.Abs(inputAxis.z);

		return axis;
	}

	// This is combined with the grounded field
	bool ShouldJump() {
		return !pushing.hasPoint && ((continuousJumping && Input.GetButton("Jump"))
			|| (!continuousJumping && Input.GetButtonDown("Jump")));
	}
	#endregion
}
