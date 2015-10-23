using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropoffStation : MonoBehaviour {

    public Transform targetTransform;

    public static List<DropoffStation> _ALL = new List<DropoffStation>();

    private _Equipable item;

    

    public virtual void OnDropoff(_Equipable item) {
        this.item = item;

        item.transform.parent = targetTransform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }


    public float GetDistance(Vector3 from, bool ignoreY = false) {
        Vector3 to = targetTransform.position;

        if (ignoreY) {
            to.y = from.y = 0;
        }

        return Vector3.Distance(from, to);
    }

    public static Closest GetClosest(Vector3 from, bool ignoreY = false) {
        return new Closest(from, ignoreY);
    }

    public struct Closest {
        public DropoffStation item;
        public float dist;
        public bool valid;

        public Closest(DropoffStation item, float dist) {
            this.item = item;
            this.dist = dist;
            this.valid = item != null;
        }

        public Closest(Vector3 from, bool ignoreY = false) {
            DropoffStation closestObj = null;
            float closestDist = Mathf.Infinity;

            // Look for the closest one
            _ALL.ForEach(delegate (DropoffStation obj) {
                // Ignore all that got items
                if (obj.item != null)
                    return;

                float dist = obj.GetDistance(from, ignoreY);

                if (closestObj == null || (dist < closestDist)) {
                    closestObj = obj;
                    closestDist = dist;
                }
            });

            this = new Closest(closestObj, closestDist);
        }
    }
}
