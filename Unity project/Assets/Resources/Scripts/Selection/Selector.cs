﻿using UnityEngine;
using System.Collections;

public static class Selector {
	private static Unit _selected;
	public static Unit Selected
	{
		get { return _selected; }
		set
		{
			if (_selected != null)
				Selected.SendMessage("Unselect");

			bool valueIsSelectable = (value != null) && (value.GetComponent<Selectable>() != null);
			_selected = valueIsSelectable? value : null;

			if (_selected != null)
				Selected.SendMessage("Select");
		}
	}

	public static string GetSelectedTag()
	{
		return Selected ? Selected.tag : "Untagged";
	}

	public static bool HasTargetSelected(string tag)
	{
		return (Selected && Selected.tag == tag);
	}
}
