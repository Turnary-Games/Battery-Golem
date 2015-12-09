using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[Header("Object references")]

	public CharacterController character;
	public Transform electricTransform;
	public PlayerInventory inventory;
	public GameObject deathParticles;
	public ParticleSystem electricParticles;

	[Header("Health")]

	[Tooltip("Time spent being dead until the game resets")]
	public float resetDelay = 2.5f;
	public float deathShake = .5f;

	[Header("Movement settings")]

	[Tooltip("Speed in meters per second.")]
	public float moveSpeed = 1;
	[Tooltip("Time in seconds it takes to reach speed of /moveSpeed/. The lower the value the higer the acceleration.")]
	public float topSpeedAfter = 0.5f;
	[Tooltip("Speed in degrees per second.")]
	public float rotSpeed = 1;
	public float jumpHeight = 1;
	[Tooltip("Should the player be able to hold space and continue jumping?")]
	public bool continuousJumping = true;
	public float pushPower = 2.0F;
	[Tooltip("When in a too steep hill the player will be moved away from the hill in /slopeForce/ meters per second.")]
	public float slopeForce;

	[Header("Inventory settings")]

	[Tooltip("Range in meters")]
	public float pickupRange;
	[Tooltip("When calculating which item is closest should it ignore the y axis? (Which would count everything as on the same height)")]
	public bool ignoreYAxis = false;
	
	[HideInInspector]
	public Vector3 outsideForces;

	public Vector3 characterCenter {
		get { return transform.position + (character != null ? character.center : Vector3.zero); }
	}
	public Vector3 electricPoint {
		get { return (electricTransform ?? transform).position; }
	}

	// Current movement velocity
	private Vector3 motion;

	// Current slope
	private float slopeAngle;
	private Vector3 slopePoint;

	private bool electrifying;

	public bool dead {
		get { return _dead; }
		set {
			if (value == true && !_dead) {
				_dead = value;
				OnDeath();
			}
		}
	}
	private bool _dead;

	void Start() {
		deathParticles.SetActive(false);
	}

	void Update () {
		if (dead) {
			DeadStep();

			// Stop everything when dead.
			// This includes gravity and movement.
			return;
		}

		Movement ();

		if (Input.GetButtonDown("GrabNDrop")) {
			GrabNDrop();
		}

		electrifying = false;
		if (Input.GetAxis("Interact") != 0) {
			ElectrifyNInteract();
		}
		if (electrifying && !electricParticles.isPlaying)
			electricParticles.Play();
		if (!electrifying && !electricParticles.isStopped)
			electricParticles.Stop();
		

		for (int slot = 1; slot <= 3; slot++) {
			if (Input.GetButtonDown("Slot " + slot)) {
				inventory.SwapItems(0, slot);
			}
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(characterCenter, pickupRange);
	}
#endif

	#region Health
	private float timeOfDeath;
	private Vector3 placeOfDeath;

	void OnDeath() {
		deathParticles.SetActive(true);
		timeOfDeath = Time.time;
		placeOfDeath = transform.position;
		
		// Disable electrifying just in case
		var em = electricParticles.emission;
		em.enabled = false;
	}

	void DeadStep() {

		// Shake a little
		transform.position = placeOfDeath + Vector3.one * Random.value * deathShake;

		if (Time.time - timeOfDeath > resetDelay) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	#endregion

	#region Movement algorithms
	void Movement() {
		// Move the character
		Move();

		// Rotate the character
		Rotate ();
	}

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
		Vector3 rawAxis = new Vector3 (Input.GetAxisRaw ("HorizontalLook"), 0, Input.GetAxisRaw ("VerticalLook"));
		
		// Not using the looking axis input, try the movement axis
		if (rawAxis.x == 0 && rawAxis.z == 0)
			rawAxis = new Vector3 (Input.GetAxisRaw("HorizontalMove"), 0, Input.GetAxisRaw("VerticalMove"));

		// Not using that one either, then dont rotate at all
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
		// Vector of the (movement) axis
		Vector3 inputAxis = new Vector3 (Input.GetAxis ("HorizontalMove"), 0, Input.GetAxis ("VerticalMove"));

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
		return character.isGrounded && slopeAngle <= character.slopeLimit;
	}

	// This is combined with the IsGrounded() function
	bool ShouldJump() {
		return (continuousJumping && Input.GetButton ("Jump"))
			|| (!continuousJumping && Input.GetButtonDown ("Jump"));
	}
	#endregion

	#region Collisions

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Water") {
			dead = true;
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		// Calculate slope angle (in degrees)
		slopeAngle = Vector3.Angle (Vector3.up, hit.normal);
		slopePoint = hit.point;

		// Collision listener
		GameObject main = hit.collider.attachedRigidbody != null ? hit.collider.attachedRigidbody.gameObject : hit.gameObject;
		if (main != null) {
			main.SendMessage (TouchMethods.Touch, this, SendMessageOptions.DontRequireReceiver);
		}

		PushObjects (hit);
	}

	#endregion

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

	/* OLD PICKUP METHOD
	// Pickup everything on collision
	void OnTriggerEnter(Collider other) {
		if (other.isTrigger) {
			inventory.Pickup(other.gameObject);
		}
	}
	*/

	#region Picking up/Dropping items & Interacting

	void ElectrifyNInteract() {
		if (!_ElectricListener.InteractAt(this, electricPoint)) {
			// Didn't interact with anything... Try to electrify
			if (inventory.equipped != null && inventory.equipped.canBeElectrified) {
				// Electrify the equipped item
				inventory.equipped.SendMessage(ElectricMethods.Electrify, this, SendMessageOptions.DontRequireReceiver);
				electrifying = true;
			} else if (inventory.equipped == null) {
				// Try to electrify at your fingertips
				_ElectricListener.ElectrifyAllAt(this, electricPoint);
				electrifying = true;
			}
		}

	}

	public _Equipable GetItemInRange() {
		var closest = Searchable.GetClosest<_Equipable>(characterCenter, ignoreYAxis);
		return closest.valid && closest.dist <= pickupRange ? closest.obj : null;
	}

	void GrabNDrop() {
		if (inventory.equipped == null) {
            // No item equipped. Try to grab the nearby item
            var item = GetItemInRange();
			if (item != null)
				inventory.AddItem(item);
		} else {
			// Item equipped. Drop it.
			inventory.Unequip();
		}
	}

	#endregion

	// Get all listeners of the touching 
	public List<_TouchListener> GetListeners() {
		return _TouchListener.FindListeners(this);
	}
}
