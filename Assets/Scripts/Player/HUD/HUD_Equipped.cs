using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Equipped : MonoBehaviour {

	public PlayerInventory inventory;
	public Sprite placeholderIcon;

	[Header("UI elements")]
	public Text textTitle;
	public Text textItemInfo;
	public Image imageFrame;
	public Image imageIcon;

	// Current item
	private _Equipable item;

	public void SetItem(_Equipable item) {
		textItemInfo.text = GetItemDescription(item);
		imageIcon.sprite = GetItemSprite(item);
	}

	public string GetItemDescription(_Equipable item) {
		return item != null
		/* Valid */		? "name: " + item.name
		/* Invalid */	: "<no item>";
	}

	public Sprite GetItemSprite(_Equipable item) {
		return item != null
		/* Valid */		? (item.icon != null ? item.icon : placeholderIcon)
		/* Invalid */	: null;
	}

	public void DropItem() {
		print("HUD sais drop it!");
		inventory.Drop();
	}
}
