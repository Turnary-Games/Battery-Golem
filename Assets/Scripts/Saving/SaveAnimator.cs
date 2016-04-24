using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueId))]
[RequireComponent(typeof(Animator))]
public class SaveAnimator : MonoBehaviour, ISavable {

	public void OnLoad(Dictionary<string, object> data) {
		Animator anim = GetComponent<Animator>();

		// Load parameters
		List<ParameterState> parameters = (List<ParameterState>)data["anim@parameters"];
		foreach (var p in parameters)
			SetParameterValue(anim, p);

		// Jump back to correct animation clips
		LayerState[] layers = (LayerState[])data["anim@layers"];
		for (int layer = 0; layer < layers.Length; layer++) {
			LayerState state = layers[layer];
			anim.Play(state.clipHash, layer, state.time);
		}
	}

	public void OnSave(ref Dictionary<string, object> data) {
		Animator anim = GetComponent<Animator>();

		// Save parameters
		List<ParameterState> parameters = new List<ParameterState>();
		foreach (var p in anim.parameters) {
			parameters.Add(new ParameterState {
				nameHash = p.nameHash,
				type = p.type,
				value = GetParameterValue(anim, p.type, p.nameHash)
			});
		}
		data["anim@parameters"] = parameters;

		// Save animationstates in layers
		LayerState[] layers = new LayerState[anim.layerCount];
		for(int layer = 0; layer < anim.layerCount; layer++) {
			var stateInfo = anim.GetCurrentAnimatorStateInfo(layer);
			layers[layer] = new LayerState {
				clipHash = stateInfo.shortNameHash,
				time = stateInfo.normalizedTime
			};
		}
		data["anim@layers"] = layers;
	}

	object GetParameterValue(Animator anim, AnimatorControllerParameterType type, int nameHash) {
		switch (type) {
			case AnimatorControllerParameterType.Bool:
			case AnimatorControllerParameterType.Trigger:
				return anim.GetBool(nameHash);

			case AnimatorControllerParameterType.Float:
				return anim.GetFloat(nameHash);

			case AnimatorControllerParameterType.Int:
				return anim.GetInteger(nameHash);

			default:
				return null;
		}
	}

	void SetParameterValue(Animator anim, ParameterState para) {
		switch (para.type) {
			case AnimatorControllerParameterType.Bool:
			case AnimatorControllerParameterType.Trigger:
				anim.SetBool(para.nameHash, (bool)para.value);
				break;

			case AnimatorControllerParameterType.Float:
				anim.SetFloat(para.nameHash, (float)para.value);
				break;

			case AnimatorControllerParameterType.Int:
				anim.SetInteger(para.nameHash, (int)para.value);
				break;
		}
	}

	struct ParameterState {
		public AnimatorControllerParameterType type;
		public object value;
		public int nameHash;
	}

	struct LayerState {
		public float time;
		public int clipHash;
	}

}
