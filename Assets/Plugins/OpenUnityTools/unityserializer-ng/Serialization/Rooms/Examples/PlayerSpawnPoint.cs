// /* ------------------
//       ${Name} 
//       (c)3Radical 2012
//           by Mike Talbot 
//     ------------------- */
// 
using UnityEngine;

[AddComponentMenu("Storage/Rooms/Examples/Player Spawn Point")]
[RequireComponent(typeof(StoreInformation))]
public class PlayerSpawnPoint : MonoBehaviour
{
	public static PlayerSpawnPoint currentSpawnPoint;
	
	public bool current
	{
		get
		{
			return currentSpawnPoint == this;
		}
		set
		{
			if(value)
				currentSpawnPoint = this;
			else if(currentSpawnPoint == this)
				currentSpawnPoint = null;
		}
	}
	
	void Awake()
	{
		GetComponent<Collider>().isTrigger = true;
	}
	
	void OnTriggerEnter(Collider col)
	{
		GameObject main = col.attachedRigidbody ? col.attachedRigidbody.gameObject : col.gameObject;
		if (main == PlayerLocator.player)
		{
			current = true;
		}
	}
	
	
	void OnRoomWasLoaded()
	{
		if(current)
		{
			PlayerLocator.current.transform.position = transform.position;
			PlayerLocator.current.transform.rotation = transform.rotation;
		}
	}
	
}

