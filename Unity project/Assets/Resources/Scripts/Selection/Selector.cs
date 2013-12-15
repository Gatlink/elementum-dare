using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {
	private static Collider _selected;
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

	public static string GetSelectedTag()
	{
		return Selected ? Selected.tag : "Untagged";
	}

	public static bool HasTargetSelected(string tag)
	{
		return (Selected && Selected.tag == tag);
	}

	void Update()
	{
		if (!Input.GetMouseButtonDown(0))
			return;

		Vector3 mousePos = Input.mousePosition;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if(Selected != null) //Something is selected already
		{
			string tag = Selected.tag;
			if(tag == "Unit") //It is a UNIT
			{
				int layerMask = 1 << LayerMask.NameToLayer("Units");
				//Check only against objects in the layer "Units", which contains only units

				if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) //clicked on a unit
				{
					Selected = (hit.collider == Selected) ? null : hit.collider;
				}
				else //not clicked on a unit
				{
					layerMask = 1 << LayerMask.NameToLayer("Terrain");
					//Check only against objects in the layer "Terrain", which contains only blocs
					
					if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) //clicked on a block
					{
						MoveToObject script = Selected.GetComponent<MoveToObject>();
						script.Target = hit.collider.transform;
						script.enabled = true;
					}
					else
						Selected = null;
				}
			}
			else if(tag == "Bloc") //It is a BLOC
			{
				if(Physics.Raycast(ray, out hit, Mathf.Infinity))
				{
					Selected = (hit.collider == Selected) ? null : hit.collider;
				}
			}
		}
		else //nothing selected
		{
			if(Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				Selected = hit.collider;
			}
		}
	}
}
