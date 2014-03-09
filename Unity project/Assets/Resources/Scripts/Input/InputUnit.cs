using UnityEngine;
using System.Collections;

public class InputUnit : MonoBehaviour
{
	private void Leave<NextState>() where NextState : MonoBehaviour
	{
		enabled = false;
		Selector.Selected.Accessor.Clear();
		Selector.Selected.enabled = false;
		GetComponent<NextState>().enabled = true;
	}

	void Update ()
	{
		if (GameTicker.GameEnded)
		{
			Leave<InputEnd>();
		}

		Selector.Selected.enabled = true;

		// KEYS
		if(Input.GetKeyDown(KeyCode.Space))
			Leave<InputSource>();
		else if(Input.GetKeyDown(KeyCode.LeftAlt))
			Leave<InputBloc>();
		else if(Input.GetKeyDown(KeyCode.Return))
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
		}

		// MOVEMENT
		Vector3 mousePos = Input.mousePosition;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Unit unit = Selector.Selected;
				unit.Target = hit.collider.GetComponent<Bloc>();
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
				Debug.Log(hit.collider.GetComponent<Bloc>().ToString());
		}
	}
}
