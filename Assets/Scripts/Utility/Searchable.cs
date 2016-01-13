using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Searchable : MonoBehaviour {

	public static List<Searchable> _ALL = new List<Searchable>();
	/// <summary>
	/// If not valid then skip when searching.
	/// </summary>
	public virtual bool valid {
		get {
			return true;
		}
	}

	protected virtual void Awake() {
		_ALL.Add(this);
	}

	public virtual float GetDistance(Vector3 from, bool ignoreY = false) {
		Vector3 to = transform.position;

		if (ignoreY) {
			to.y = from.y = 0;
		}

		return Vector3.Distance(from, to);
	}
	
	public static Closest<Searchable> GetClosest(Vector3 from, bool ignoreY = false) {
		return new Closest<Searchable>(from, ignoreY);
	}

	public static Closest<T> GetClosest<T>(Vector3 from, bool ignoreY = false) where T : Searchable {
		return new Closest<T>(from, ignoreY);
	}

	public static Closest<T> GetClosest<T>(Vector3 inRangeFrom, float radius, Vector3 closestTo, bool ignoreY = false) where T : Searchable {
		return new Closest<T>(inRangeFrom, radius, closestTo, ignoreY);
	}

}

public struct Closest<T> where T : Searchable {
	public T obj;
	public float dist;
	public bool valid {
		get {
			return obj != null;
		}
	}

	public Closest(T obj, float dist) {
		this.obj = obj;
		this.dist = dist;
	}


	public Closest(Vector3 from, bool ignoreY = false) {
		List<T> list = Searchable._ALL.ConvertAll(delegate (Searchable obj) {
			return obj as T;
		});

		list.RemoveAll(delegate (T obj) {
			return obj == null;
		});

		this = new Closest<T>(list, from, ignoreY);
	}
	
	public Closest(Vector3 inRangeFrom, float radius, Vector3 closestTo, bool ignoreY = false) {
		List<T> list = Searchable._ALL.ConvertAll(delegate (Searchable obj) {
			return obj as T;
		});

		list.RemoveAll(delegate (T obj) {
			return obj == null || obj.GetDistance(inRangeFrom, ignoreY) > radius;
		});

		this = new Closest<T>(list, closestTo, ignoreY);
	}

	public Closest(List<T> list, Vector3 to, bool ignoreY = false) {
		T closestObj = null;
		float closestDist = Mathf.Infinity;

		// Look for the closest one
		list.ForEach(delegate (T obj) {
			// Skip invalid items
			if (!obj.valid)
				return;

			float dist = obj.GetDistance(to, ignoreY);

			if (closestObj == null || (dist < closestDist)) {
				closestObj = obj as T;
				closestDist = dist;
			}
		});

		this.obj = closestObj;
		this.dist = closestDist;
	}

	public override string ToString() {
		return "{ [Closest] valid=" + valid.ToString() + "; dist=" + dist.ToString() + "; obj=" + obj + " }";
	}
}
