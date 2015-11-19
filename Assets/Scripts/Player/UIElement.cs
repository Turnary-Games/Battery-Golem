using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElement : MonoBehaviour {
	
	public static Sprite placeholder;

	public bool shadowAfterFirst = false;

	public Text title;
	public Image frame;
	public Image icon;

	private Sprite savedSprite;
	private int slot = -1;
	private PlayerHUD hud;

	public void UpdateElement(PlayerHUD hud, int slot, _Equipable item) {
		this.hud = hud;
		this.slot = slot;

		var desc = GetItemDescription(item);
		var sprite = GetItemSprite(item);
		var color = item == null ? Color.clear : Color.white;

		if (shadowAfterFirst) {
			if (sprite != null)
				// If there is a sprite (i.e. valid item)
				// Save it for later referance
				savedSprite = sprite;
			else if (savedSprite != null) {
				// If there is no sprite (i.e. no item)
				// Use the saved one, to indicate what goes here
				sprite = savedSprite;
				color = new Color(1, 1, 1, 0.5f);
			}
		}

		title.text = desc;
		icon.sprite = sprite;
		icon.color = color;
	}

	public void OnClick() {
		if (slot >= 0)
			hud.OnClick(slot);
	}

	public static string GetItemDescription(_Equipable item) {
		return item != null
		/* Valid */      ? "name: " + item.itemName
		/* Invalid */    : "<no item>";
	}

	public static Sprite GetItemSprite(_Equipable item) {
		return item != null
		/* Valid */      ? (item.icon != null ? item.icon : placeholder)
		/* Invalid */    : null;
	}

}
