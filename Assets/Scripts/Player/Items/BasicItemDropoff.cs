using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueId))]
[RequireComponent(typeof(_ElectricListener))]
public class BasicItemDropoff : MonoBehaviour, ISavable {

	public int filterID = -1;
	public Renderer activate;
	public bool activated;
	[Header("Send message")]
	public GameObject sendMessageTo;
	public string message = "OnDropoff";

	[Header("Audio")]
	public AudioSource audioSource;

	void OnInteractStart(PlayerController source) {
		if (activated) return;

		_Item item = source.inventory.equipped;
		if (item == null) return;

		if (filterID == -1 || item.id == filterID) {
			Destroy(item.gameObject);
			activated = true;

			if (activate != null)
				activate.enabled = true;

			if (sendMessageTo != null)
				sendMessageTo.SendMessage(message, SendMessageOptions.DontRequireReceiver);

			if (audioSource != null)
				audioSource.Play();
		}
	}

	public void OnSave(ref Dictionary<string, object> data) {
		data["dropoff@activated"] = activated;
	}

	public void OnLoad(Dictionary<string, object> data) {
		activated = (bool)data["dropoff@activated"];

		if (activate != null)
			activate.enabled = activated;

		if (sendMessageTo != null && activated)
			sendMessageTo.SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
}
