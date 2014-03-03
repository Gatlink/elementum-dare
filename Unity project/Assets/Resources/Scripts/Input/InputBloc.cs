using UnityEngine;
using System.Collections;

public class InputBloc : MonoBehaviour
{
	private Bloc _handledBloc = null;
	
	void Start() { enabled = false; }
	
	void Update()
	{
		if (!Selector.HasTargetSelected("Unit"))
			return;

		Bloc bloc = null;
		if (_handledBloc == null)
		{
			Unit unit = Selector.Selected.gameObject.GetComponent<Unit>();
			bloc = unit.CurrentBloc;
			_handledBloc = BlocFactory.CreateBloc(unit.BlocType);
		}
		else
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int layerMask = 1 << LayerMask.NameToLayer("Terrain");
			
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				bloc = hit.collider.gameObject.GetComponent<Bloc>();
		}

		BlocIndex index = bloc.indexInMap;
		index.z += 1;
		if (_handledBloc.indexInMap != index)
			_handledBloc.transform.position = Map.IndexToPosition(index);
		
		if (Input.GetMouseButtonDown(0))
		{
			Map.InsertBloc(index.x, index.y, _handledBloc);
			Quit();
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameObject.Destroy(_handledBloc.gameObject);
			Quit();
		}
	}
	
	void Quit()
	{
		_handledBloc = null;
		GetComponent<InputUnit>().enabled = true;
		enabled = false;
		Unit unit = Selector.Selected.GetComponent<Unit>();
		if (unit != null)
			unit.UpdateAccessibleBlocs();
	}
}
