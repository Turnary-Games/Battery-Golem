using UnityEngine;
using System.Collections;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T> {

	public static T instance;

	protected virtual void Awake() {
		instance = this as T;
	}
}
