using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class RandomAnimationStart : MonoBehaviour {

	void Start() {
		var anim = GetComponent<Animator>();
		for (int i=0; i<anim.layerCount; i++) {
			var info = anim.GetCurrentAnimatorStateInfo(i);
			anim.Play(info.shortNameHash, i, Random.value);
		}
	}
	
}
