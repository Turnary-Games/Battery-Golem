using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_TouchListener))]
public class ItemCarrier : _Platform {

    private List<_Equipable> items = new List<_Equipable>();

	void OnTouchStart(Touch touch) {
        var item = touch.source as _Equipable;
        if (item != null)
            items.Add(item);
    }

    void OnTouchEnd(Touch touch) {
        var item = touch.source as _Equipable;
        if (item != null && items.Contains(item)) {
            items.Remove(item);
            item.rbody.velocity = Vector3.zero;
        }
    }

    void OnTouch(Touch touch) {
        var item = touch.source as _Equipable;
        if (item == null || !items.Contains(item))
            return;

        item.rbody.velocity = body.velocity;
    }

}
