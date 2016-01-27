using UnityEngine;
using System.Collections;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T> {

	public static T instance;

	protected virtual void Awake() {
		if (instance != null)
			Debug.LogError("Only one instance plox!");
		instance = this as T;
	}
}
