using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using System;

public class NPCDialogOverrideDateTime : NPCDialogOverrideBase {

	[Header("Replace on this date")]
	public Month month = Month.january;
	public int day = 1;

	void OnValidate() {
		day = Mathf.Clamp(day, 1, 31);
	}

	private List<NPCController.Dialog> original;

	private bool _replace;
	void FixedUpdate() {

		/*	Now to decide the fun date :3
			
			How about each saturday?
			Or like our birthdays *u*
			Mmm like that idea.

			One qoute from each of us
			that only works on our birthdays
		*/

		DateTime now = DateTime.Now;
		bool replace = now.Day == day && now.Month == MonthOfYear(month);

		// Simple algorithm for switching back n forth
		if (replace != _replace) {
			if (replace) {
				var npc = GetComponent<NPCController>();
				if (npc.isTalking)
					// Skip, do nothing this cycle
					replace = _replace;
				else {
					// Override
					original = NPCController.CopyDialog(npc.dialogs);
					Override();
				}
			} else {
				var npc = GetComponent<NPCController>();
				if (npc.isTalking)
					// Skip, do nothing this cycle
					replace = _replace;
				else
					// Override
					npc.dialogs = NPCController.CopyDialog(original);
			}
		}

		_replace = replace;
	}

	int MonthOfYear(Month month) {
		switch (month) {
			case Month.january:		return 1;
			case Month.february:	return 2;
			case Month.march:		return 3;
			case Month.april:		return 4;
			case Month.may:			return 5;
			case Month.june:		return 6;
			case Month.july:		return 7;
			case Month.august:		return 8;
			case Month.september:	return 9;
			case Month.october:		return 10;
			case Month.november:	return 11;
			case Month.december:	return 12;

			default: return -1;
		}
	}

	public enum Month {
		january, february, march, april, may, june, july, august, september, october, november, december
	}

}
