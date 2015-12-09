using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	[Header("Player sub-classes")]
	public PlayerController controller;
	public PlayerInventory inventory { get { return controller.inventory; } }
	public PlayerMovement movement { get { return controller.movement; } }
	public PlayerHealth health { get { return this; } }
	public PlayerInteraction interaction { get { return controller.interaction; } }

	[Header("Death settings")]

	public GameObject deathParticles;
	[Tooltip("Time spent being dead until the game resets")]
	public float resetDelay = 2.5f;
	public float deathShake = .5f;

	private float timeOfDeath;
	private Vector3 placeOfDeath;

	private bool _dead;
	public bool dead {
		get { return _dead; }
		set {
			if (value == true && !_dead) {
				_dead = value;
				OnDeath();
			}
		}
	}

	void Start() {
		deathParticles.SetActive(false);
	}

	void Update() {
		if (dead) {
			// Shake a little
			transform.position = placeOfDeath + Vector3.one * Random.value * deathShake;

			if (Time.time - timeOfDeath > resetDelay) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	void OnDeath() {
		deathParticles.SetActive(true);
		timeOfDeath = Time.time;
		placeOfDeath = transform.position;

		// Disable electrifying just in case
		var em = interaction.electricParticles.emission;
		em.enabled = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Water") {
			dead = true;
		}
	}

}
