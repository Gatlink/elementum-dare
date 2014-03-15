using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FluidStream : Stream
{
	private void Flow() 
	{
		if(!_bloc)
		{
			Debug.Log("Updating a fluid stream affected to no bloc.");
			return;
		}

		//Debug.Log( GetAltitude().ToString() + "-" + GetVolume().ToString());

		if(_bloc.Streams[_type] <= 1)
			return;
		
		List<Bloc> surroundings = Map.FetchNeighbors2D(_bloc.indexInMap, 1);
		
		List<Bloc> validNeighbors = DiscardInvalidNeighbors(_bloc, ref surroundings);
		
		int neighborsNb = validNeighbors.Count;
		if(neighborsNb == 0)
			return;
		
		int maxNbOfNeighbors = 4;
		int oneShare = (int) Mathf.Floor((float) _bloc.Streams[_type] / (float)(maxNbOfNeighbors + 1));
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
			_bloc.Streams[_type] -= amountMoved;
			
			//Debug.Log (amountMoved + " from " + bloc.name + " to " + neighbor.name);
		}
	}
	
	private List<Bloc> DiscardInvalidNeighbors(Bloc refBloc, ref List<Bloc> neighbors)
	{
		List<Bloc> list = new List<Bloc>(neighbors);
		list.RemoveAll(x => Bloc.IsHigher(x, refBloc)); //TODO debordement
		list.RemoveAll(x => !Bloc.IsLower(x, refBloc) && (x.Streams[_type] > refBloc.Streams[_type]));
		return list;
	}

	public override void UpdateStream()
	{
		Flow ();
	}

	public override void UpdateStreamVisual()
	{
		const int maxVal = 48;
		
		//Update stream visual according to bloc value
		float delta = (GetVolume() * (1.0f / maxVal));
		Vector3 initialScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3(initialScale.x, delta, initialScale.z);
	}
}
