using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayerHelper {

	public static int[] GetAllLayers() {
		List<int> layers = new List<int> ();

		for (int n=0; n<32; n++) {
			// If name of the layer has > 0 characters
			if (LayerMask.LayerToName(n).Length > 0) {
				layers.Add (n);
			}
		}

		return layers.ToArray();
	}

}

