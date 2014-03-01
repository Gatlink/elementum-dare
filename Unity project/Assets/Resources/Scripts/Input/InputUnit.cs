using UnityEngine;
using System.Collections;

public class InputUnit : MonoBehaviour
{
	private void Leave<NextState>() where NextState : MonoBehaviour
	{
		enabled = false;
		GetComponent<NextState>().enabled = true;
	}

	void Start()
	{
		enabled = false;
	}

	void Update ()
	{
		// KEYS
		if(Input.GetKeyDown(KeyCode.Space))
			Leave<InputSource>();
		else if(Input.GetKeyDown(KeyCode.LeftAlt))
			Leave<InputBloc>();
		else if(Input.GetKeyDown(KeyCode.Return))
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
			Leave<InputUnitSelect>();
		}

		// SELECTION
		if (!Input.GetMouseButtonDown(0))
			return;

		Vector3 mousePos = Input.mousePosition;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Units")))
		{
			if (hit.collider != Selector.Selected)
				Selector.Selected =  hit.collider;
			else
				Leave<InputUnitSelect>();
		}
		else if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
		{
			MoveToObject script = Selector.Selected.GetComponent<MoveToObject>();
			script.Target = hit.collider.transform;
			script.enabled = true;
		}
		else
			Leave<InputUnitSelect>();
	}
}
