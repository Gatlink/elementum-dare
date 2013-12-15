using UnityEngine;
using System.Collections;

public class SourceSpawner : MonoBehaviour {

	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update () 
	{
		if(!Input.GetKeyDown(KeyCode.Space))
			return;

		if(!Selector.HasTargetSelected("Bloc"))
			return;

		Bloc bloc = Selector.Selected.gameObject.GetComponent<Bloc>();

		Source source = SpawnSource(Source.SourceType.Lava, bloc);
		source.SetOnBloc(bloc);
		bloc.ReceiveSource(source);

		SourceManager.RegisterElement(source);
	}

	public static Source SpawnSource(Source.SourceType type, Bloc bloc)
	{
		if(bloc == null)
			return null;

		BlocIndex sourceIndex = bloc.indexInMap;
		sourceIndex.z += 1;

		Vector3 pos = Map.IndexToPosition(sourceIndex);

		Source source = SourceFactory.CreateSource(type);
		source.gameObject.transform.position = pos;

		return source;
	}
}
