using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadingScreen : SingletonBase<LoadingScreen> {

	public static GameObject loadingPrefab;
	public const string RESUME_CHECKPOINT = "RESUME_CHECKPOINT";
	public const string PREFAB_PATH = "Prefabs/Loading_screen";
	public const string INIT_SCENE = "InitializingScene";

	[SceneDropDown]
	public int targetRoom = 0;
	public Animator anim;
	public Image[] fadeUs;
	public float fadeTime = 1;
	public bool destroyIfPassthrough = false;

	public System.Action<LoadingScreen> loadedCallback;

	private float start;
	private State state = State.fadeIn;

	protected override void Awake() {
		if (instance && destroyIfPassthrough)
			Destroy(gameObject);
		else {
			base.Awake();

			start = Time.time;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Update() {
		float fade;
		
		switch(state) {
			case State.fadeIn:
				if (fadeTime <= 0)
					fade = 1;
				else
					fade = Mathf.InverseLerp(0, fadeTime, Time.time - start);

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
					fade = Mathf.InverseLerp(0, fadeTime, Time.time - start);

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
			start = Time.time;
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
			loadingPrefab = Resources.Load(PREFAB_PATH) as GameObject;
		}
	}
}

