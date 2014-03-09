using UnityEngine;
using System.Collections;
using System.Linq;

public class InputEnd : MonoBehaviour {

	private Rect _labelRect = new Rect(50, 50, Screen.width - 50, Screen.height - 50);

	void Start()
	{
		enabled = false;
	}

	void OnGUI()
	{
		if (Unit.Units.Count == 0)
			GUI.Label(_labelRect, "Draw!");
		else if (Unit.Totems.Count () == 0)
			GUI.Label(_labelRect, "Monsters win!");
		else if (Unit.Monsters.Count () == 0)
			GUI.Label(_labelRect, "Totems win!");
	}
}
