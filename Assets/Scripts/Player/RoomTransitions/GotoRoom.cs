using UnityEngine;
using System.Collections;

public class GotoRoom : MonoBehaviour {

	[SceneDropDown]
	public string room;

	void Start () {
		RoomManager.LoadRoom(room);
	}
}
