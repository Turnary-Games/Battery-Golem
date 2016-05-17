using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadingScreen : SingletonBase<LoadingScreen> {

	public static GameObject loadingPrefab;
	public static GameObject pausePrefab;
	public const string PREFAB_LOADING_PATH = "Prefabs/Loading_screen";
	public const string PREFAB_PAUSE_PATH = "Prefabs/Pause_menu";

	[SceneDropDown]
	public int targetRoom = 0;
	public Animator anim;
	public Image[] fadeUs;
	public float fadeTime = 1;
	public bool useUnscaledTime = false;

	public System.Action<LoadingScreen> loadedCallback;

	private float time { get { return useUnscaledTime ? Time.unscaledTime : Time.time; } }
	private float start;
	private State state = State.fadeIn;

	protected override void Awake() {
		base.Awake();

		start = time;
		DontDestroyOnLoad(gameObject);
	}

	void Update() {
		float fade;
		
		switch(state) {
			case State.fadeIn:
				if (fadeTime <= 0)
					fade = 1;
				else
					fade = Mathf.InverseLerp(0, fadeTime, time - start);

				if (BackgroundMusic.instance)
					BackgroundMusic.instance.SetAudioFading(1f - fade);
				
				foreach(var img in fadeUs)
					img.color = new Color(img.color.r, img.color.g, img.color.b, fade);

				if (Mathf.Approximately(fade, 1f)) {
					state = State.loading;

					GameSaveManager.LoadRoom(targetRoom);
				}
				break;

			case State.loading:

				break;

			case State.fadeOut:
				if (fadeTime <= 0)
					fade = 1;
				else
					fade = Mathf.InverseLerp(0, fadeTime, time - start);

				if (BackgroundMusic.instance)
					BackgroundMusic.instance.SetAudioFading(fade);

				foreach (var img in fadeUs)
					img.color = new Color(img.color.r, img.color.g, img.color.b, 1 - fade);

				if (Mathf.Approximately(fade, 1f)) {
					instance = null;
					Destroy(gameObject);
				}
				break;
		}
	}

	void OnLevelWasLoaded() {
		if (state == State.loading) {
			state = State.fadeOut;
			start = time;
			anim.SetBool("Loaded", true);
			if (loadedCallback != null)
				loadedCallback(this);
			loadedCallback = null;
		}
	}

	private enum State {
		fadeIn, loading, fadeOut
	}

	public static void LoadRoom(int room, bool fade = true) {
		UpdatePrefab();

		var clone = Instantiate(loadingPrefab) as GameObject;
		var script = clone.GetComponent<LoadingScreen>();
		script.targetRoom = room;
		if (!fade) script.fadeTime = 0;
	}

	public static void LoadRoom(int room, System.Action<LoadingScreen> loadedCallback, bool fade = true) {
		UpdatePrefab();

		var clone = Instantiate(loadingPrefab) as GameObject;
		var script = clone.GetComponent<LoadingScreen>();
		script.targetRoom = room;
		script.loadedCallback = loadedCallback;
		if (!fade) script.fadeTime = 0;
	}

	static void UpdatePrefab() {
		if (loadingPrefab == null) {
			loadingPrefab = Resources.Load(PREFAB_LOADING_PATH) as GameObject;
		}
	}

	public static void PauseGame() {
		if (Mathf.Approximately(Time.timeScale, 0)) return; // Already paused

		// Load if needed
		if (pausePrefab == null) {
			pausePrefab = Resources.Load(PREFAB_PAUSE_PATH) as GameObject;
		}
		// Pause teh gejm
		if (pausePrefab != null) {
			Instantiate(pausePrefab);
		}
	}
}

