using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NPCController))]
public abstract class NPCDialogOverrideBase : MonoBehaviour {

	public int dialogID = 1;
	[HideInInspector]
	public List<NPCController.Dialog> dialogs = new List<NPCController.Dialog>();

	protected void Override() {
		GetComponent<NPCController>().Override(dialogs, dialogID);
	}

}
