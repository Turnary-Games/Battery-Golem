using UnityEngine;
using System.Collections;
using Saving;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameSaveManager : SingletonBase<GameSaveManager> {

	/// <summary>
	/// The saved data is stored first in a table keyed by the unique id's,
	/// then the data table inside contains specific data that's set by the saving scripts.
	/// </summary>
	public static Dictionary<string, Dictionary<string, object>> roomData = new Dictionary<string, Dictionary<string, object>>();

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

	public delegate Dictionary<string, object> DataCallback(Dictionary<string, object> data);
	public static void ChangeData(string uuid, DataCallback callback) {
		Dictionary<string, object> data;
		if (roomData.ContainsKey(uuid))
			data = roomData[uuid];
		else
			data = new Dictionary<string, object>();

		roomData[uuid] = callback(data);
	}

	public static void SaveRoom() {
		rooms[SceneManager.GetActiveScene().buildIndex] = new RoomState();
	}

	public static void LoadRoom(int room) {
		SaveRoom();
		SceneManager.LoadSceneAsync(room);
	}

	void OnLevelWasLoaded(int index) {
		// This actually runs before Awake
		if (instance != this) return;
		
		RoomState.ApplyChanges();

		SaveRoom();
	}

}
