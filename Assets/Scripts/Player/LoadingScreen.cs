using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[DontStore]
public class LoadingScreen : SingletonBase<LoadingScreen> {

	public static GameObject loadingPrefab;
	public const string RESUME_CHECKPOINT = "RESUME_CHECKPOINT";
	public const string PREFAB_PATH = "Prefabs/Loading_screen";
	public const string INIT_SCENE = "InitializingScene";

	[SceneDropDown]
	public string targetRoom = "mane_menu";
	public Image background;
	public Text text;
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

				background.color = new Color(background.color.r, background.color.g, background.color.b, fade);
				text.color = new Color(text.color.r, text.color.g, text.color.b, fade);

				if (Mathf.Approximately(fade, 1f)) {
					state = State.loading;
					if (targetRoom == RESUME_CHECKPOINT)
						LevelSerializer.Resume();
					else
						RoomManager.LoadRoom(targetRoom);
				}
				break;

			case State.loading:

				break;

			case State.fadeOut:
				if (fadeTime <= 0)
					fade = 1;
				else
					fade = Mathf.InverseLerp(0, fadeTime, Time.time - start);

				background.color = new Color(background.color.r, background.color.g, background.color.b, 1 - fade);
				text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - fade);

				if (Mathf.Approximately(fade, 1f)) {
					instance = null;
					Destroy(gameObject);
				}
				break;
		}
	}

	void OnLevelWasLoaded() {
		if (state == State.loading) {
			text.text = "Loading complete";
			state = State.fadeOut;
			start = Time.time;
			if (loadedCallback != null)
				loadedCallback(this);
		}
	}

	private enum State {
		fadeIn, loading, fadeOut
	}

	public static void LoadRoom(string room, bool fade = true) {
		UpdatePrefab();

		var clone = Instantiate(loadingPrefab) as GameObject;
		var script = clone.GetComponent<LoadingScreen>();
		script.targetRoom = room;
		if (!fade) script.fadeTime = 0;
	}

	public static void LoadRoom(string room, System.Action<LoadingScreen> loadedCallback, bool fade = true) {
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

	public static void FetchPlayer(bool fade = false) {
		string lastRoom = SceneManager.GetActiveScene().name;
		LoadRoom(INIT_SCENE, script => {
			Destroy(script.gameObject);
			ResetSaves.Reset();
			LoadRoom(lastRoom, false);
		}, fade);
	}
}
