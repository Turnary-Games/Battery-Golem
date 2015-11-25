using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElement : MonoBehaviour {
	
	public static Sprite placeholder;
	
	public Sprite shadowSprite;
	public bool unlocked = false;

	[Space]

	public Text title;
	public Image frame;
	public Image icon;
	
	private int slot = -1;
	private PlayerHUD hud;

	public void UpdateElement(PlayerHUD hud, int slot, _Equipable item) {
		this.hud = hud;
		this.slot = slot;

		var desc = GetItemDescription(item);
		var sprite = GetItemSprite(item);
		var color = item == null ? Color.clear : Color.white;

		if (sprite == null && shadowSprite != null && unlocked) {
			// If there is no sprite (i.e. no item)
			// Use the saved one, to indicate what goes here
			SetShadow();
		} else {

			icon.sprite = sprite;
			icon.color = color;
		}

		title.text = desc;
	}

	public void OnClick() {
		if (slot >= 0)
			hud.OnClick(slot);
	}

	public void SetShadow(Sprite sprite = null) {
		icon.sprite = shadowSprite = sprite ?? shadowSprite;
		icon.color = new Color(1, 1, 1, 0.5f);
	}

	static string GetItemDescription(_Equipable item) {
		return item != null
		/* Valid */      ? "name: " + item.itemName
		/* Invalid */    : "<no item>";
	}

	static Sprite GetItemSprite(_Equipable item) {
		return item != null
		/* Valid */      ? (item.icon != null ? item.icon : placeholder)
		/* Invalid */    : null;
	}

}
