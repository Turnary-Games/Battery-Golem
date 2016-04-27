using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class ButtonActions : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject creditsMenu;

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

	public Slider ambientVolume;
	public Text ambientPercent;

	[Header("Audio")]
	public AudioSource buttonClicked;

	void Start() {
		// Add PlayClickSound to ALL buttons
		foreach (var btn in GetComponentsInChildren<Button>(true)) {
			btn.onClick.AddListener(PlayClickSound);
		}
	}

	public void JumpToScene(string name) {
		LoadingScreen.LoadRoom(SceneManager.GetSceneByName(name).buildIndex);
	}

	public void JumpToScene(int build_index) {
		LoadingScreen.LoadRoom(build_index);
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
		creditsMenu.SetActive(false);
	}

	public void OpenOptionsMenu() {
		mainMenu.SetActive(false);
		optionsMenu.SetActive(true);
		creditsMenu.SetActive(false);
	}

	public void OpenCreditsMenu() {
		optionsMenu.SetActive(false);
		mainMenu.SetActive(false);
		creditsMenu.SetActive(true);
	}

	public void UpdateVolume() {
		mixer.SetFloat("Volume_Master", masterVolume.value);
		mixer.SetFloat("Volume_Music", musicVolume.value);
		mixer.SetFloat("Volume_SFX", SFXVolume.value);
		mixer.SetFloat("Volume_NPC", NPCVolume.value);
		mixer.SetFloat("Volume_Ambient", ambientVolume.value);

		masterPercent.text = ToPercent(masterVolume);
		musicPercent.text = ToPercent(musicVolume);
		SFXPercent.text = ToPercent(SFXVolume);
		NPCPercent.text = ToPercent(NPCVolume);
		ambientPercent.text = ToPercent(ambientVolume);
	}

	string ToPercent(Slider slider) {
		return Mathf.FloorToInt(slider.normalizedValue * 100).ToString() + "%";
	}

	public void PlayClickSound() {
		if (buttonClicked)
			buttonClicked.Play();
	}

}
