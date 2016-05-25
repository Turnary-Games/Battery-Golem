using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using System.Linq;

public class GameSaveManager : SingletonBase<GameSaveManager> {

	/// <summary>
	/// The saved data is stored first in a table keyed by the unique id's,
	/// then the data table inside contains specific data that's set by the saving scripts.
	/// </summary>
	public static Dictionary<string, Dictionary<string, object>> roomData = new Dictionary<string, Dictionary<string, object>>();
	public static int currentRoom { get { return _currentRoom; } }
	public static bool freshLoad { get { return roomLoads[_currentRoom] == 1; } }
	public static bool isLoading;

	private static int _currentRoom = -1;
	private static int[] roomLoads = new int[SceneManager.sceneCountInBuildSettings];

	protected override void Awake() {
		if (instance != null)
			Destroy(gameObject);
		else {
			base.Awake();
			if (transform.parent != null)
				transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
			_currentRoom = SceneManager.GetActiveScene().buildIndex;
			roomLoads[_currentRoom]++;
			SaveRoom();
			UnityEngine.Analytics.Analytics.CustomEvent("usingVersion", new Dictionary<string, object> {
				{ "version", BatteryGolemVersion.FormatVersion(BatteryGolemVersion.CURRENT) }
			});
		}
	}

	#region Saving algorithms
	public static void SaveRoom() {
		string log = "";

		foreach (var unique in Object.FindObjectsOfType<UniqueId>()) {
			// Dont store data to items without unique ids
			if (unique.uniqueId == "") continue;

			// Fetch the appropiet data
			Dictionary<string, object> data;
			if (roomData.ContainsKey(unique.uniqueId))
				 data = roomData[unique.uniqueId];
			else data = new Dictionary<string, object>();

			// Let all components write onto it
			foreach (var comp in unique.GetComponents<ISavable>()) {
				comp.OnSave(ref data);
			}

			// Save it
			if (data.Count != 0 || GameSaveManager.roomData.ContainsKey(unique.uniqueId)) {
				GameSaveManager.roomData[unique.uniqueId] = data;
				log += "Saved \"" + unique.transform.GetPath() + "\" (" + unique.uniqueId + ")\n";
			}
		}

		if (log.Length == 0)
			log = "Nothing to save...";
		else
			log = "Saved data for " + log.Count(c => c == '\n') + " components\n=======================\n" + log;
		Debug.Log(log);
	}

	public static IEnumerator SaveRoomWait() {
		yield return new WaitForEndOfFrame();

		SaveRoom();
	}

	public static void LoadRoom(int room) {
		isLoading = true;
		SceneManager.LoadSceneAsync(room);
	}

	public static void ApplyChanges() {
		string log = "";

		foreach (var unique in Object.FindObjectsOfType<UniqueId>()) {
			if (!roomData.ContainsKey(unique.uniqueId)) continue;
			if (unique.uniqueId == "") continue;

			var savables = unique.GetComponents<ISavable>();
			foreach (var comp in savables) {
				comp.OnLoad(roomData[unique.uniqueId]);
			}

			if (savables.Length > 0)
				log += "Loaded values for \"" + unique.transform.GetPath() + "\" (" + unique.uniqueId + ")\n";
		}

		if (log.Length == 0)
			log = "Nothing to load...";
		else
			log = "Loaded values for " + log.Count(c => c == '\n') + " components\n=======================\n" + log;
		Debug.Log(log);
	}
	
	void OnLevelWasLoaded(int index) {
		// This actually runs before Awake
		if (instance != this) return;
		roomLoads[index]++;
		_currentRoom = index;
		
		ApplyChanges();
		//SaveRoom();
		StartCoroutine(SaveRoomWait());
		isLoading = false;
	}
	#endregion

}

public interface ISavable {
	/// <summary>
	/// Write your variables to the data dictionary, the keys may be anything you wish.
	/// Please note that all components with the same uniqueId shares data
	/// </summary>
	void OnSave(ref Dictionary<string, object> data);
	/// <summary>
	/// The data variable contains the same data you stored earlier.
	/// So if you say store the position in the key "position",
	/// then you can grab it here under the key "position".
	/// <para>You must save something for it to save the first time,
	/// so it won't store empty data tables</para>
	/// </summary>
	void OnLoad(Dictionary<string, object> data);
}