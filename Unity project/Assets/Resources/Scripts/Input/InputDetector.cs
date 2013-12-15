using UnityEngine;
using System.Collections;

class InputDetector : MonoBehaviour
{
	void Start() {}

	void Update() 
	{
		if(!Input.anyKeyDown)
			return;

		if(Input.GetKeyDown(KeyCode.Space) && Selector.Selected.CompareTag("Unit"))
		{
			GetComponent<InputSource>().enabled = true;
			enabled = false;
		}

		if(Input.GetKeyDown(KeyCode.Return))
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
		}
	}
}