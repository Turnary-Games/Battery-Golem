using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : PlayerSubClass {

	[Header("Death settings")]

	public GameObject deathParticles;
	[Tooltip("Time spent being dead until the game resets")]
	public float resetDelay = 2.5f;
	public float deathShake = .5f;

	private float timeOfDeath;
	private Vector3 placeOfDeath;

	[HideInInspector]
	public bool dead;

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
		dead = true;

		deathParticles.SetActive(true);
		timeOfDeath = Time.time;
		placeOfDeath = transform.position;

		// Disable electrifying just in case
		var em = interaction.electricParticles.emission;
		em.enabled = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Water" && !dead) {
			OnDeath();
		}
	}

}
