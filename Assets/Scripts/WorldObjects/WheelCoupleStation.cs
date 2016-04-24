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
	
	private bool wheelInPlace { get { return wheelRenderer ? wheelRenderer.enabled : false; } }
	
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
		} else if (item.id == wheelID && !wheelInPlace) {
			// Place wheel
			item.SendMessage(ItemMethods.OnItemDroppedOff, this, SendMessageOptions.DontRequireReceiver);
			Destroy(item.gameObject);

			wheelRenderer.enabled = true;
		}
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
