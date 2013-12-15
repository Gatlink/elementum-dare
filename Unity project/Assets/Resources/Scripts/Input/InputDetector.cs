using UnityEngine;
using System.Collections;

class InputDetector : MonoBehaviour
{
	void Start() {}

	void Update() 
	{
		if(!Input.anyKeyDown)
			return;

		if(Input.GetKeyDown(KeyCode.Space) && Selector.HasTargetSelected("Unit"))
		{
			GetComponent<InputSource>().enabled = true;
			enabled = false;
		}
		else if(Input.GetKeyDown(KeyCode.LeftAlt))
		{
			GetComponent<InputBloc>().enabled = true;
			enabled = false;
		}
		else if(Input.GetKeyDown(KeyCode.Return))
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
		}

		if(Input.GetKeyDown(KeyCode.LeftControl) && Selector.HasTargetSelected("Bloc"))
		{
			Debug.Log(Selector.Selected.gameObject.GetComponent<Bloc>().Elements.ToString());
		}
	}
}