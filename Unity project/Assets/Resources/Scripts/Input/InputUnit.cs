using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputUnit : MonoBehaviour
{
	public GUIStyle squareButton;
	public GUIStyle arrowRight;
	public GUIContent[] iconBloc;
	public GUIContent[] iconSource;

	private Dictionary<Bloc.BlocType, int> _blocDict = new Dictionary<Bloc.BlocType, int>();
	private Dictionary<Source.SourceType, int> _sourceDict = new Dictionary<Source.SourceType, int>();


	void Awake()
	{

		_blocDict.Add (Bloc.BlocType.Earth, 0);
		_blocDict.Add (Bloc.BlocType.Ice, 1);
		_blocDict.Add (Bloc.BlocType.Metal, 2);
		_blocDict.Add (Bloc.BlocType.Plant, 3);
		_blocDict.Add (Bloc.BlocType.Rock, 4);

		_sourceDict.Add (Source.SourceType.Electricity, 0);
		_sourceDict.Add (Source.SourceType.Lava, 1);
		_sourceDict.Add (Source.SourceType.Sand, 2);
		_sourceDict.Add (Source.SourceType.Water, 3);
		_sourceDict.Add (Source.SourceType.Wind, 4);

	}

	void OnGUI()
	{

		if (GUI.Button (new Rect (10, Screen.height - 70, 60, 60), iconBloc[_blocDict[Selector.Selected.BlocType]], squareButton) && (!Selector.Selected.HasActed)) 
			Leave<InputBloc>();

		if (GUI.Button (new Rect (80, Screen.height - 70, 60, 60), iconSource[_sourceDict[Selector.Selected.SourceType]], squareButton) && (!Selector.Selected.HasActed)) 
			Leave<InputSource>();

		if (GUI.Button (new Rect (150, Screen.height - 70, 60, 60), "", arrowRight)) 
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
		}

	}

	private void Leave<NextState>() where NextState : MonoBehaviour
	{
		enabled = false;
		if (Selector.Selected != null)
		{
			Selector.Selected.Accessor.Clear();
			Selector.Selected.enabled = false;
		}
		GetComponent<NextState>().enabled = true;
	}

	void Update ()
	{
		if (GameTicker.GameEnded)
		{
			Leave<InputEnd>();
		}

        if (!Selector.Selected)
            return;

		Selector.Selected.enabled = true;

		// KEYS
		if(!Selector.Selected.HasActed && Input.GetKeyDown(KeyCode.Space))
			Leave<InputSource>();
		else if(!Selector.Selected.HasActed && Input.GetKeyDown(KeyCode.LeftAlt))
			Leave<InputBloc>();
		else if(Input.GetKeyDown(KeyCode.Return))
		{
			GameTicker.EndPhase();
			GameTicker.StartNewPhase();
		}



		// MOVEMENT
		Vector3 mousePos = Input.mousePosition;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Unit unit = Selector.Selected;
				unit.Target = hit.collider.GetComponent<Bloc>();
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
				Debug.Log(hit.collider.GetComponent<Bloc>().ToString());
		}
	}
}
