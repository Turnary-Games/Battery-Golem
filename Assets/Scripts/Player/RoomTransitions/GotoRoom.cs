using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GotoRoom : MonoBehaviour {

	[SceneDropDown]
	public string room;
	public static string lastRoom = "";

	void Start () {
		if (lastRoom == "" || lastRoom == SceneManager.GetActiveScene().name)
			RoomManager.LoadRoom(room);
		else {
			RoomManager.LoadRoom(lastRoom);
			lastRoom = "";
		}
	}
}
