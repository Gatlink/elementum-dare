using UnityEngine;
using System.Collections;

public class Bloc : MonoBehaviour 
{
	public enum BlocType
	{
		TerrainBloc,
		Earth,
		Rock,
		Ice,
		Metal,
		Plant, 
		Upgraded_Plant
	};


	public BlocIndex indexInMap {get; private set;}

	private Source _source;

	public bool HoldASource()
	{
		return _source != null;
	}
	
	public void ReceiveSource(Source source)
	{
		_source = source;
	}

	public bool HostACharacter {get; set;} //TODO change that

	public void InsertedAt(BlocIndex pos, Map map)
	{
		indexInMap = pos;

		gameObject.transform.position = Map.IndexToPosition(pos);
		gameObject.transform.parent = map.transform;
	}

	public bool IsReachable() //TODO déterminer si on passe un Vec3 ou un BlocIndex
	{
		if(HoldASource() || HostACharacter)
			return false;

		//TODO déterminer si on lui passe le bloc de départ (potentiellement à 2 blocs d'écart, ou juste le bloc adjacent)
		//TODO déterminer si "à portée" (diff hauteur)
		return true;
	}

	public bool CanTakeFlow()
	{
		return !HoldASource();
	}

	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update() {}
}
