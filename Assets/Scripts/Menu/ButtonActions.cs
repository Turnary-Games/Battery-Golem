using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class ButtonActions : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject optionsMenu;
	[Header("Volume options")]
	public AudioMixer mixer;
	
	public Slider masterVolume;
	public Text masterPercent;

	public Slider musicVolume;
	public Text musicPercent;

	public Slider SFXVolume;
	public Text SFXPercent;

	public Slider NPCVolume;
	public Text NPCPercent;

	/// <summary>
	/// Use UnitySerialization-NG RoomManager to jump to a scene.
	/// </summary>
	public void JumpToRoom(string name) {
		//RoomManager.LoadRoom(name);
		LoadingScreen.LoadRoom(name);
	}

	/// <summary>
	/// Use Unity's built in SceneManager to jump to a scene.
	/// </summary>
	public void JumpToScene(string name) {
		SceneManager.LoadScene(name);
	}

	public void ExitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void OpenMainMenu() {
		optionsMenu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void OpenOptionsMenu() {
		mainMenu.SetActive(false);
		optionsMenu.SetActive(true);
	}

	public void UpdateVolume() {
		mixer.SetFloat("Volume_Master", masterVolume.value);
		mixer.SetFloat("Volume_Music", musicVolume.value);
		mixer.SetFloat("Volume_SFX", SFXVolume.value);
		mixer.SetFloat("Volume_NPC", NPCVolume.value);

		masterPercent.text = ToPercent(masterVolume);
		musicPercent.text = ToPercent(musicVolume);
		SFXPercent.text = ToPercent(SFXVolume);
		NPCPercent.text = ToPercent(NPCVolume);
	}

	string ToPercent(Slider slider) {
		return Mathf.FloorToInt(slider.normalizedValue * 100).ToString() + "%";
	} 
}
