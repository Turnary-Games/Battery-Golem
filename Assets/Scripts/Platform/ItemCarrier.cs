using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_TouchListener))]
public class ItemCarrier : MonoBehaviour {

    private List<_Equipable> items = new List<_Equipable>();
    private Vector3 last;

    void Start() {
        last = transform.position;
    }

    void LateUpdate() {
        Vector3 motion = transform.position - last;
        items.ForEach(delegate (_Equipable item) {
            item.transform.position += motion;
        });
        last = transform.position;
    }

	void OnTouchStart(Touch touch) {
        var item = touch.source as _Equipable;
        if (item != null)
            items.Add(item);
    }

    void OnTouchEnd(Touch touch) {
        var item = touch.source as _Equipable;
        if (item != null)
            items.Remove(item);
    }

}
