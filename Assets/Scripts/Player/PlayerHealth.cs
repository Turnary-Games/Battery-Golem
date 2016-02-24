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

	private bool hasReset = false;

	void Start() {
		//deathParticles.SetActive(false);
		SetParticles(false);
	}

	void Update() {
		if (dead) {
			// Shake a little
			transform.position = placeOfDeath + Vector3.one * Random.value * deathShake;

			if (Time.time - timeOfDeath > resetDelay && !hasReset) {
				hasReset = true;
				if (LevelSerializer.CanResume)
					// Jump back to checkpoint
					LevelSerializer.Resume();
				else
					// Hardreset level
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	void SetParticles(bool state) {
		foreach (ParticleSystem ps in deathParticles.GetComponentsInChildren<ParticleSystem>()) {
			var em = ps.emission;
			em.enabled = state;
		}
	}

	void OnDeath() {
		dead = true;

		//deathParticles.SetActive(true);
		SetParticles(true);
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
