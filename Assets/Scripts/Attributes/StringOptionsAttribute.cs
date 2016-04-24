using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class StringOptionsAttribute : PropertyAttribute {

	public string[] options;

	public StringOptionsAttribute() {
		this.options = new string[] { "N/A" };
	}

	public StringOptionsAttribute(string[] options) {
		this.options = options;
	}

}

public class PrefabOptionsAttribute : StringOptionsAttribute {

	public PrefabOptionsAttribute() : base() {
		options = new string[] {
			"Prefabs/Items/Fan",
			"Prefabs/Items/Nut",
			"Prefabs/Items/S1_Lost_cog",
			"Prefabs/Items/Wheel-couple_rod",
			"Prefabs/Items/Wheel-couple_wheel",
		};
	}

}