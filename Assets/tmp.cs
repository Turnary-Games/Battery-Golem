using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;

[ExecuteInEditMode]
public class tmp : MonoBehaviour {

	public bool run = false;

	// Update is called once per frame
	void Update () {
		if (run) {
			run = false;

			List<string> msg = new List<string>();

			foreach (var ps in FindObjectsOfType<ParticleSystem>()) {
				msg.Add("simSpace=" + ps.simulationSpace.ToString() + "\tplayOnAwake=" + ps.playOnAwake.ToString() + "\t" + ps.transform.GetPath());
				
			}

			msg.Sort();
			string output = "";

			foreach(string v in msg) {
				output += v + "\n";
			}

			print(output);
		}
	}
}
