// /* ------------------
//       ${Name} 
//       (c)3Radical 2012
//           by Mike Talbot 
//     ------------------- */
// 
using UnityEngine;

[AddComponentMenu("Storage/Rooms/Examples/Player Locator")]
public class PlayerLocator : MonoBehaviour
{
	public static PlayerLocator current;
	public static GameObject player;
	
	void Start() {
		if (current != this && current != null)
			Destroy(transform.root.gameObject);
	}

	void Awake()
	{
		DontDestroyOnLoad(transform.root.gameObject);
		current = this;
		player = gameObject;
	}

}


