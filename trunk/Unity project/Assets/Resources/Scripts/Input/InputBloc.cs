using UnityEngine;
using System.Linq;
using System.Collections;

public class InputBloc : MonoBehaviour
{
	public int Range = 1;

	public GUIStyle arrowLeft;

	private Bloc _handledBloc = null;
	private Unit _unit = null;
	private BlocAccessor _accessor = new BlocAccessor(Color.blue);
	
	void Awake()
	{
		enabled = false;
	}
	
	void OnEnable()
	{
		_unit = Selector.Selected;
		if (_unit.BlocType == Bloc.BlocType.Plant)
			_accessor.Update(_unit.CurrentBloc, Range, Bloc.BlocType.Earth);
		else
			_accessor.Update(_unit.CurrentBloc, Range);
		
		if (_accessor.AccessibleBlocs.Count() == 0)
		{
			Quit();
			return;
		}
		
		_handledBloc = _unit.CreateBloc();
		PlaceHandledBloc(_accessor.AccessibleBlocs.First());
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameObject.Destroy(_handledBloc.gameObject);
			Quit();
		}

		Bloc bloc = null;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << LayerMask.NameToLayer("Terrain");
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			bloc = hit.collider.gameObject.GetComponent<Bloc>();

		if (bloc == null
		    || _accessor.AccessibleBlocs == null
		    || !_accessor.AccessibleBlocs.Contains(bloc))
			return;

		PlaceHandledBloc(bloc);

		if (Input.GetMouseButtonDown(0))
		{
			_unit.HasActed = true;
			Map.InsertBloc(bloc.indexInMap.x, bloc.indexInMap.y, _handledBloc);
			Quit();
		}
	}

	private void Quit()
	{
		_accessor.Clear();
		_handledBloc = null;
		GetComponent<InputUnit>().enabled = true;
		enabled = false;
		Unit unit = Selector.Selected;
		if (unit != null)
			unit.UpdateAccessibleBlocs();
	}

	private void PlaceHandledBloc(Bloc bloc)
	{
		BlocIndex index = bloc.indexInMap;

		if (_unit.BlocType == Bloc.BlocType.Plant)
			_handledBloc.Type = bloc.IsQuickSanded ? Bloc.BlocType.UpgradedPlant : Bloc.BlocType.Plant;

		index.z += 1;
		if (_handledBloc.indexInMap != index)
			_handledBloc.transform.position = Map.IndexToPosition(index);
	}
	
	void OnGUI() 
	{

		if (GUI.Button (new Rect (10, Screen.height - 70, 60, 60), "", arrowLeft)) 
		{
			//shameless copié collé de la section "if getKeyDown = escape"
			GameObject.Destroy(_handledBloc.gameObject);
			Quit();		
		}
	}
}
