﻿using UnityEngine;
using System.Collections;

public class InputUnit : MonoBehaviour
{
	private void Leave<NextState>() where NextState : MonoBehaviour
	{
		enabled = false;
		GetComponent<NextState>().enabled = true;
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
		}

		// SELECTION
		Vector3 mousePos = Input.mousePosition;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

//		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Units")))
//		{
//			if (hit.collider != Selector.Selected)
//				Selector.Selected =  hit.collider;
//		}
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Unit unit = Selector.Selected.GetComponent<Unit>();
				unit.Target = hit.collider.transform;
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
				Debug.Log(hit.collider.GetComponent<Bloc>().ToString());
		}
	}
}