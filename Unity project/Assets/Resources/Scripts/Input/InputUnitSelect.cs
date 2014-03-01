using UnityEngine;
using System.Collections;

public class InputUnitSelect : MonoBehaviour
{
	public bool DebugMode = true;

	void OnEnable()
	{
		Selector.Selected = null;
	}

	void Update ()
	{
		if (!Input.GetMouseButtonDown(0))
			return;
		
		Vector3 mousePos = Input.mousePosition;
		
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		int layerMask = 1 << LayerMask.NameToLayer("Units");
		
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			Selector.Selected = hit.collider;
			gameObject.GetComponent<InputUnit>().enabled = true;
			enabled = false;
		}
		else if (DebugMode && Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
		{
			Selector.Selected = hit.collider;
			gameObject.GetComponent<InputDebug>().enabled = true;
			enabled = false;
		}
	}
}
