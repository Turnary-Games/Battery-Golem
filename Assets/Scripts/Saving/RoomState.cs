using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using System.Collections.Generic;

namespace Saving {
	public class RoomState {
		
		int scene;
		/// <summary>
		/// The saved data is stored first in a table keyed by the unique id's,
		/// then the data table inside contains specific data that's set by the saving scripts.
		/// </summary>
		static Dictionary<string, Dictionary<string, object>> allData = new Dictionary<string, Dictionary<string, object>>();
		
		public RoomState() {
			scene = SceneManager.GetActiveScene().buildIndex;

			foreach(var unique in Object.FindObjectsOfType<UniqueId>()) {
				// Dont store data to items without unique ids
				if (unique.uniqueId == "") continue;

				var data = new Dictionary<string, object>();
				foreach(var comp in unique.GetComponents<ISaveable>()) {
					comp.OnSave(ref data);
				}

				allData[unique.uniqueId] = data;
			}
		}

		public void LoadState() {
			Debug.Log("Start loading scene #" + scene);
			SceneManager.LoadSceneAsync(scene);
		}

		void ApplyChanges() {
			Debug.Log("Loading complete, apply changes for scene #" + scene);

			foreach (var unique in Object.FindObjectsOfType<UniqueId>()) {
				if (!allData.ContainsKey(unique.uniqueId)) continue;
				if (unique.uniqueId == "") continue;

				foreach (var comp in unique.GetComponents<ISaveable>()) {
					comp.OnLoad(allData[unique.uniqueId]);
				}
			}
		}

		// Called by GameSaveManager
		public void OnLoadingComplete() {
			ApplyChanges();
		}
	}

	public interface ISaveable {
		/// <summary>
		/// Write your variables to the data dictionary, the keys may be anything you wish.
		/// Please note that all components with the same uniqueId shares data
		/// </summary>
		void OnSave(ref Dictionary<string, object> data);
		/// <summary>
		/// The data variable contains the same data you stored earlier.
		/// So if you say store the position in the key "position",
		/// then you can grab it here under the key "position".
		/// </summary>
		void OnLoad(Dictionary<string, object> data);
	}
}