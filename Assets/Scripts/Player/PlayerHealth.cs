using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : PlayerSubClass {

	[Header("Death settings")]

	public GameObject deathParticles;
	[Tooltip("Time spent being dead until the game resets")]
	public float resetDelay = 2.5f;
	public float deathShake = .5f;
	public GameObject model;

	private float timeOfDeath;

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
			model.transform.localPosition = Vector3.one * Random.value * deathShake;

			if (Time.time - timeOfDeath > resetDelay && !hasReset) {
				//SetParticles(false);
				hasReset = true;

				//GameSaveManager.SaveRoom();
				LoadingScreen.LoadRoom(GameSaveManager.currentRoom);
				/*
				if (LevelSerializer.CanResume) {
					// Jump back to checkpoint
					LoadingScreen.LoadRoom(LoadingScreen.RESUME_CHECKPOINT, false);
					//print("PLAYER DIED: RESUME CHECKPOINT");
					//LevelSerializer.Resume();
				} else {
					// Hardreset level
					LoadingScreen.LoadRoom(SceneManager.GetActiveScene().name, false);
					//print("PLAYER DIED: HARD RESET ROOM");
					//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}*/
			}
		} else if (hasReset) {
			hasReset = false;
			SetParticles(false);
			model.transform.localPosition = Vector3.zero;
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
		movement.body.isKinematic = true;

		// Disable electrifying just in case
		var em = interaction.electricParticles.emission;
		em.enabled = false;

		if (sound)
			sound.OnDeath();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Water" && !dead) {
			OnDeath();
		}
	}

}
