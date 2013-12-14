using UnityEngine;
using System.Collections;

public class SelectUnit : MonoBehaviour {
	public static Collider _selected;
	public static Collider Selected
	{
		get { return _selected; }
		set
		{
			if (_selected != null)
				Selected.SendMessage("Unselect");

			_selected = value;

			if (_selected != null)
				Selected.SendMessage("Select");
		}
	}

	void Update()
	{
		Vector3 mousePos = Input.mousePosition;

		if (!Input.GetMouseButtonDown(0))
			return;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			if (Selected != null && hit.collider.CompareTag("Bloc"))
				Selected.GetComponent<MoveToObject>().enabled = true;
			else if (hit.collider == Selected || !hit.collider.CompareTag("Unit"))
				Selected = null;
			else
				Selected = hit.collider;
		}
	    else
			Selected = null;
	}
}
