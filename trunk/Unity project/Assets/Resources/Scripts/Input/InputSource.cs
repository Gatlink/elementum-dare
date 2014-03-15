using UnityEngine;
using System.Linq;
using System.Collections;

public class InputSource : MonoBehaviour
{
	public int Range = 1;

	private Source _handledSource = null;
	private Unit _unit = null;
	private BlocAccessor _accessor = new BlocAccessor(Color.green);

	void Awake()
	{
		enabled = false;
	}

	void OnEnable()
	{
		_unit = Selector.Selected.GetComponent<Unit>();
		_accessor.Update(_unit.CurrentBloc, Range);

		if (_accessor.AccessibleBlocs.Count() == 0)
			Quit();

		_handledSource = _unit.CreateSource();
		_handledSource.Bloc = _accessor.AccessibleBlocs.First();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_handledSource.Bloc.ReceiveSource(null);
			GameObject.Destroy(_handledSource.gameObject);
			Quit();
			return;
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

		if (_handledSource.Bloc != bloc)
			_handledSource.Bloc = bloc;

		if (Input.GetMouseButtonDown(0))
		{
			_unit.HasActed = true;
			Quit();
		}
	}

	void Quit()
	{
		_accessor.Clear();
		_handledSource = null;
		_unit.UpdateAccessibleBlocs();
		_unit = null;
		enabled = false;
		GetComponent<InputUnit>().enabled = true;
	}
}
