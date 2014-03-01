using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Stream : MonoBehaviour
{
	public enum StreamType
	{
		None,
		Sand,
		Lava,
		Water
	}
	public static int NB_OF_TYPES = (int)StreamType.Water;
	// must be last of enum

	public StreamType type { get; set;}
	
	private Bloc _bloc;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public int GetAltitude()
	{
		if(_bloc)
			return _bloc.indexInMap.z + 1;
		else
			return -1;
	}
	
	public int GetVolume()
	{
		if(_bloc)
			return _bloc.Streams[type];
		else
			return -1;
	}
	
	public void PlaceOnBloc(Bloc bloc)
	{
		_bloc = bloc;
	}

	public void Flow() 
	{
		if(!_bloc)
			return;

		//Debug.Log( GetAltitude().ToString() + "-" + GetVolume().ToString());

		if(_bloc.Streams[type] <= 1)
			return;
		
		List<Bloc> surroundings = Map.FetchNeighbors2D(_bloc.indexInMap, 1);
		
		List<Bloc> validNeighbors = DiscardInvalidNeighbors(_bloc, ref surroundings);

		int neighborsNb = validNeighbors.Count;
		if(neighborsNb == 0)
			return;
		
		int maxNbOfNeighbors = 4;
		int oneShare = (int) Mathf.Floor((float) _bloc.Streams[type] / (float)(maxNbOfNeighbors + 1));
		//int amountToShare = (int) Mathf.Floor((float) denominator * ((float) bloc.Elements.Lava / 5.0f));
		int amountToShare = neighborsNb * oneShare;
		amountToShare += (maxNbOfNeighbors - neighborsNb) * (int) Mathf.Floor(oneShare * 0.5f);
		//half a share for the invalid neighbors to be redistributed
		
		int denominator = neighborsNb;
		foreach(Bloc neighbor in validNeighbors)
		{
			denominator += Bloc.IsLower(neighbor, _bloc) ? 2 : 0 ;
		}
		
		if((amountToShare / denominator) < 1) //not enough to share
			return;
		
		foreach(Bloc neighbor in validNeighbors)
		{
			int share = Bloc.IsLower(neighbor, _bloc) ? 3 : 1 ;
			int amountMoved = (int) Mathf.Round(amountToShare * ((float)share / (float)denominator));
			neighbor.Streams.Lava += amountMoved;
			_bloc.Streams[type] -= amountMoved;
			
			//Debug.Log (amountMoved + " from " + bloc.name + " to " + neighbor.name);
		}
	}
	
	private List<Bloc> DiscardInvalidNeighbors(Bloc refBloc, ref List<Bloc> neighbors)
	{
		List<Bloc> list = new List<Bloc>(neighbors);
		list.RemoveAll(x => Bloc.IsHigher(x, refBloc)); //TODO debordement
		list.RemoveAll(x => !Bloc.IsLower(x, refBloc) && (x.Streams[type] > refBloc.Streams[type]));
		return list;
	}
}