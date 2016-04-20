using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Saving {
	public class RoomState {
		
		// Constructor.
		// Tell everything to save
		public RoomState() {
			string log = "";

			foreach(var unique in Object.FindObjectsOfType<UniqueId>()) {
				// Dont store data to items without unique ids
				if (unique.uniqueId == "") continue;

				var data = new Dictionary<string, object>();
				foreach(var comp in unique.GetComponents<ISaveable>()) {
					comp.OnSave(ref data);
				}

				if (data.Count != 0 || GameSaveManager.roomData.ContainsKey(unique.uniqueId)) {
					GameSaveManager.roomData[unique.uniqueId] = data;
					log += "Saved \"" + unique.transform.GetPath() + "\" ("+unique.uniqueId+")\n";
				}
			}

			if (log.Length == 0)
				log = "Nothing to save...";
			else
				log = "Saved data for " + log.Count(c => c == '\n') + " components\n=======================\n" + log;
			Debug.Log(log);
		}

		public static void ApplyChanges() {
			string log = "";

			foreach (var unique in Object.FindObjectsOfType<UniqueId>()) {
				if (!GameSaveManager.roomData.ContainsKey(unique.uniqueId)) continue;
				if (unique.uniqueId == "") continue;

				var savables = unique.GetComponents<ISaveable>();
				foreach (var comp in savables) {
					comp.OnLoad(GameSaveManager.roomData[unique.uniqueId]);
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
		/// <para>You must save something for it to save the first time,
		/// so it won't store empty data tables</para>
		/// </summary>
		void OnLoad(Dictionary<string, object> data);
	}
}