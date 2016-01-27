using UnityEngine;
using System.Collections;

public abstract class _DropoffItem : _Equipable {

	private bool _isDroppedOff;
	public bool isDroppedOff { get { return _isDroppedOff; } }

	public override bool valid { get { return !isDroppedOff; } }

	public virtual void OnItemDroppedOff<Item>(_DropoffStation<Item> station) where Item : _DropoffItem {
		if (!isDroppedOff) {
			_isDroppedOff = true;
		}
	}

}
