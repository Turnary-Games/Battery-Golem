using UnityEngine;
using System.Collections;

// Placeholder for UniqueIdDrawer script
public class UniqueIdentifierAttribute : PropertyAttribute { }

public class UniqueId : MonoBehaviour {
	[UniqueIdentifier]
	public string uniqueId;
}