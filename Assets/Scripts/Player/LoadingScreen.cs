using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : SingletonBase<LoadingScreen> {

	public const string RESUME_CHECKPOINT = "RESUME_CHECKPOINT";

	[SceneDropDown]
	public string targetRoom = "mane_menu";
	public Image background;
	public Text text;
	public float fadeTime = 1;

	private float start;
	private State state = State.fadeIn;

	protected override void Awake() {
		base.Awake();

		start = Time.time;
		DontDestroyOnLoad(gameObject);
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
		}
	}

	private enum State {
		fadeIn, loading, fadeOut
	}

	public static void LoadRoom(string room, bool fade = true) {
		var clone = Instantiate(PlayerController.instance.hud.loadingPrefab) as GameObject;
		var script = clone.GetComponent<LoadingScreen>();
		script.targetRoom = room;
		if (!fade) script.fadeTime = 0;
	}
}
