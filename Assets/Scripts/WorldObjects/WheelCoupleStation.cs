using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueId))]
public class WheelCoupleStation : MonoBehaviour, ISavable {

	public Animator anim;
	public AnimationPlayer electricPlayer;

	[Space]
	public Renderer rodRenderer;
	public int rodID;
	[Space]
	public Renderer wheelRenderer;
	public int wheelID;

	[Header("Audio")]
	public AudioSource noWheelNorRod;
	public AudioSource wheelButNoRod;
	public AudioSource wheelAndRod;

	[Header("Saving")]
	public bool saveAfterItemAdded;
	public int spawnPoint = -1;
	
	private bool wheelInPlace { get { return wheelRenderer ? wheelRenderer.enabled : false; } }
	private bool rodInPlace { get { return rodRenderer ? rodRenderer.enabled : false; } }
	
	void OnInteractStart(PlayerController source) {
		_Item item = source.inventory.equipped;

		if (item == null) return;
		if (item is _CoreItem) return;

		if (item.id == rodID && wheelInPlace) {
			// Place rod
			item.SendMessage(ItemMethods.OnItemDroppedOff, this, SendMessageOptions.DontRequireReceiver);
			Destroy(item.gameObject);

			rodRenderer.enabled = true;
			anim.SetBool("RodInPlace", true);
			electricPlayer.enabled = true;

			if (saveAfterItemAdded) {
				PlayerSaving.SetExitID(spawnPoint);
				StartCoroutine(GameSaveManager.SaveRoomWait());
			}
		} else if (item.id == wheelID && !wheelInPlace) {
			// Place wheel
			item.SendMessage(ItemMethods.OnItemDroppedOff, this, SendMessageOptions.DontRequireReceiver);
			Destroy(item.gameObject);

			wheelRenderer.enabled = true;

			if (saveAfterItemAdded) {
				PlayerSaving.SetExitID(spawnPoint);
				StartCoroutine(GameSaveManager.SaveRoomWait());
			}
		}
	}

	// Since acceptElectrifying is false on this go it wont trigger by mistake
	// But these will invoke via the FunctionRelay thats attatched to the electricspot
	void OnElectrify() {
		if (rodInPlace) {
			if (noWheelNorRod && noWheelNorRod.isPlaying) noWheelNorRod.Stop();
			if (wheelButNoRod && wheelButNoRod.isPlaying) wheelButNoRod.Stop();
			if (wheelAndRod && !wheelAndRod.isPlaying) wheelAndRod.Play();
		} else if (wheelInPlace) {
			if (noWheelNorRod && noWheelNorRod.isPlaying) noWheelNorRod.Stop();
			if (wheelButNoRod && !wheelButNoRod.isPlaying) wheelButNoRod.Play();
			if (wheelAndRod && wheelAndRod.isPlaying) wheelAndRod.Stop();
		} else {
			if (noWheelNorRod && !noWheelNorRod.isPlaying) noWheelNorRod.Play();
			if (wheelButNoRod && wheelButNoRod.isPlaying) wheelButNoRod.Stop();
			if (wheelAndRod && wheelAndRod.isPlaying) wheelAndRod.Stop();
		}
	}

	void OnElectrifyEnd() {
		if (noWheelNorRod && noWheelNorRod.isPlaying) noWheelNorRod.Stop();
		if (wheelButNoRod && wheelButNoRod.isPlaying) wheelButNoRod.Stop();
		if (wheelAndRod && wheelAndRod.isPlaying) wheelAndRod.Stop();
	}

	public void OnSave(ref Dictionary<string, object> data) {
		data["wheelCouple@rodInPlace"] = rodRenderer.enabled;
		data["wheelCouple@wheelInPlace"] = wheelRenderer.enabled;
	}

	public void OnLoad(Dictionary<string, object> data) {
		electricPlayer.enabled = rodRenderer.enabled = (bool)data["wheelCouple@rodInPlace"];
		wheelRenderer.enabled = (bool)data["wheelCouple@wheelInPlace"];
	}
}
