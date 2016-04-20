using UnityEngine;
using System.Collections;
using Saving;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameSaveManager : SingletonBase<GameSaveManager> {

	public static Dictionary<int, RoomState> rooms = new Dictionary<int, RoomState>();

	protected override void Awake() {
		if (instance != null)
			Destroy(gameObject);
		else {
			base.Awake();
			if (transform.parent != null)
				transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
			SaveRoom();
		}
	}

	public static void SaveRoom() {
		rooms[SceneManager.GetActiveScene().buildIndex] = new RoomState();
	}

	public static void LoadRoom(string room) {
		LoadRoom(SceneManager.GetSceneByName(room).buildIndex);
	}

	public static void LoadRoom(int room) {
		if (rooms.ContainsKey(room))
			rooms[room].LoadState();
	}

	void OnLevelWasLoaded(int index) {
		// This actually runs before Awake
		if (instance != this) return;
		
		if (rooms.ContainsKey(index))
			rooms[index].OnLoadingComplete();

		SaveRoom();
	}

}
