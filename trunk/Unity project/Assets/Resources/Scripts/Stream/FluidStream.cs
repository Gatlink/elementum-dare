using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FluidStream : Stream
{
	public static int MAX_FLUID = 48;

	private void Flow() 
	{
		if(!_bloc)
		{
			Debug.Log("Updating a fluid stream affected to no bloc.");
			return;
		}

		//Debug.Log( GetAltitude().ToString() + "-" + GetVolume().ToString());

		if(GetVolume() <= Settings.treshold)
			return;
		
		List<Bloc> surroundings = Map.FetchNeighbors2D(_bloc.indexInMap, 1);
		List<Bloc> flatNeighbors, slopeNeighbors;

		if(DivideNeighbors(_bloc, surroundings, out flatNeighbors, out slopeNeighbors) == 0) //no valid neighbors
			return; //TODO débordement

		float amountToRedist = GetVolume() * Settings.granularity;
		float nbOfShares = flatNeighbors.Count * Settings.flatFactor + slopeNeighbors.Count * Settings.slopeFactor;
		float oneShare = amountToRedist / nbOfShares;

		foreach(Bloc destBloc in flatNeighbors)
		{
			int amountMoved = Mathf.FloorToInt(oneShare * Settings.flatFactor);
			_bloc.Streams[_type].Buffer -= amountMoved;
			destBloc.Streams[_type].Buffer += amountMoved;
		}

		foreach(Bloc destBloc in slopeNeighbors)
		{
			int amountMoved = Mathf.FloorToInt(oneShare * Settings.slopeFactor);
			_bloc.Streams[_type].Buffer -= amountMoved;
			destBloc.Streams[_type].Buffer += amountMoved;
		}
	}

	/*
	private void Flow() 
	{
		if(!_bloc)
		{
			Debug.Log("Updating a fluid stream affected to no bloc.");
			return;
		}
		
		//Debug.Log( GetAltitude().ToString() + "-" + GetVolume().ToString());
		
		if(_bloc.Streams[_type] <= _treshold)
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
			neighbor.Streams[_type] += amountMoved;
			_bloc.Streams[_type] -= amountMoved;
			
			//Debug.Log (amountMoved + " from " + bloc.name + " to " + neighbor.name);
		}
	}
	*/
	
	private List<Bloc> DiscardInvalidNeighbors(Bloc refBloc, List<Bloc> neighbors)
	{
		List<Bloc> list = new List<Bloc>(neighbors);
		list.RemoveAll(x => Bloc.IsHigher(x, refBloc));
		list.RemoveAll(x => !Bloc.IsLower(x, refBloc) && (x.Streams[_type].GetVolume() > refBloc.Streams[_type].GetVolume()));
		return list; 
	}

	private int DivideNeighbors(Bloc refBloc, List<Bloc> neighbors, out List<Bloc> flat, out List<Bloc> slope)
	{
		flat = new List<Bloc>();
		slope = new List<Bloc>();

		foreach(Bloc b in neighbors)
		{
			bool invalid = b.IsHigherThan(refBloc) 
						|| (!b.IsLowerThan(refBloc) 
				    	&& (b.Streams[_type].GetVolume() > refBloc.Streams[_type].GetVolume()));
			if(invalid)
				continue;
			else if(b.IsLowerThan(refBloc))
				slope.Add(b);
			else
				flat.Add(b);
		} 

		return slope.Count + flat.Count;
	}

	public override void UpdateStreamState()
	{
		_resolved = false;
		Flow();
	}

	public override void UpdateStreamVisual(bool animated = false)
	{
		//Update stream visual according to bloc value
		float delta = (GetVolume() * (1.0f / MAX_FLUID));

		if(animated)
		{
			iTween.ScaleTo(gameObject, iTween.Hash(
													"y", delta,
													"time", 1f,
													"easeType", iTween.EaseType.easeOutElastic,
													"oncomplete", "SetVisible",
													"oncompletetarget", Visual,
													"oncompleteparams", (delta > 0)
												)
			               );
		}
		else
		{
			Vector3 initialScale = gameObject.transform.localScale;
			Vector3 newScale = new Vector3(initialScale.x, delta, initialScale.z);
			gameObject.transform.localScale = newScale;
			Visible = (delta > 0);
		}
	}

	public void Erode()
	{
		_value = Mathf.Max(_value - Settings.erosion, 0);
		_resolved = true;
	}

	public void FillUp(bool animated)
	{
		float delta = (GetVolume() * (1.0f / MAX_FLUID));

		Vector3 initialScale = gameObject.transform.localScale;
		Vector3 newScale = new Vector3(initialScale.x, delta, initialScale.z);
		gameObject.transform.localScale = newScale;

		if(delta > 0)
			Visible = true;

		if(animated)
		{
			iTween.ScaleFrom(gameObject, iTween.Hash(
														"y", 0f,
														"time", 1f,
														"easeType", iTween.EaseType.easeOutElastic
													)
			                 );
		}
	}

	public void DryOut(bool animated)
	{
		if(animated)
		{
			iTween.ScaleTo(gameObject, iTween.Hash(
														"y", 0f,
														"time", 1f,
														"easeType", iTween.EaseType.easeOutElastic,
														"oncompletetarget", gameObject,
														"oncomplete", "SetVisible",
														"oncompleteparams", false
													)
			                 );
		}
		else
		{
			Vector3 initialScale = gameObject.transform.localScale;
			Vector3 newScale = new Vector3(initialScale.x, 0, initialScale.z);
			gameObject.transform.localScale = newScale;
			Visible = false;
		}
	}
}
