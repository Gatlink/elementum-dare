using UnityEngine;
using System.Collections;

public class SelectUnit : MonoBehaviour {
	private static Collider Selected = null;

	void Update()
	{
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			if (hit.collider == Selected || !hit.collider.CompareTag("Unit"))
				Selected = null;
			else
				Selected = hit.collider;
		}
	    else
		    Selected = null;
	}
}
