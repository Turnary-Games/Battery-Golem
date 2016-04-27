using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	[SceneDropDown]
	public int maneMenu;
	
	public void JumpToManeMenu() {
		SceneManager.LoadScene(maneMenu);
	}	

}
