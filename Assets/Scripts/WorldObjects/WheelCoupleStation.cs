using UnityEngine;
using System.Collections;

public class WheelCoupleStation : MonoBehaviour {

	public Animator anim;
	public AnimationPlayer electricPlayer;

	[Space]
	public Renderer rodRenderer;
	public int rodID;
	[Space]
	public Renderer wheelRenderer;
	public int wheelID;

	[SerializeThis]
	private bool wheelInPlace = false;
	
	void OnInteractStart(PlayerController source) {
		_Item item = source.inventory.equipped;

		if (item == null) return;
		if (item is _CoreItem) return;

		if (item.id == rodID && wheelInPlace) {
			item.SendMessage(ItemMethods.OnItemDroppedOff, this, SendMessageOptions.DontRequireReceiver);
			Destroy(item.gameObject);

			rodRenderer.enabled = true;
			anim.SetBool("RodInPlace", true);
			electricPlayer.enabled = true;
		} else if (item.id == wheelID && !wheelInPlace) {
			item.SendMessage(ItemMethods.OnItemDroppedOff, this, SendMessageOptions.DontRequireReceiver);
			Destroy(item.gameObject);

			wheelRenderer.enabled = true;
			wheelInPlace = true;
		}
	}

}
