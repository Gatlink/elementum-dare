using UnityEngine;
using System.Collections;

public class InputSource : MonoBehaviour
{
	private Source _handledSource = null;

	void Start() { enabled = false; }

	void Update()
	{
		if (!Selector.HasTargetSelected("Unit"))
			return;

		Bloc bloc = null;
		if (_handledSource == null)
		{
			Unit unit = Selector.Selected.gameObject.GetComponent<Unit>();
			bloc = unit.CurrentBloc;
			_handledSource = SourceManager.Instance().SpawnSource(unit.SourceType);
		}
		else
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int layerMask = 1 << LayerMask.NameToLayer("Terrain");
			
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				bloc = hit.collider.gameObject.GetComponent<Bloc>();
		}
		
		if (_handledSource.Bloc != bloc)
			_handledSource.Bloc = bloc;

		if (Input.GetMouseButtonDown(0))
			Quit();
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			bloc.ReceiveSource(null);
			GameObject.Destroy(_handledSource.gameObject);
			Quit();
		}
	}

	void Quit()
	{
		_handledSource = null;
		GetComponent<InputUnit>().enabled = true;
		enabled = false;
		Unit unit = Selector.Selected.GetComponent<Unit>();
		if (unit != null)
			unit.UpdateAccessibleBlocs();
	}
}
