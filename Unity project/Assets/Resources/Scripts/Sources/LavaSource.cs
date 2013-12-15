using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LavaSource : Source
{
	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update() {}
	
	public override void RunSource()
	{
		if(_bloc == null)
		{
			Debug.LogError ("Updating a source affected to no bloc.");
			return;
		}

		//put everything on self bloc
		_bloc.Elements.Lava += _generate;

		List<Bloc> update = new List<Bloc>();
		update.Add(_bloc);
		List<Bloc> processed = new List<Bloc>();

		MakeSpread(ref update, ref processed);
	}

	private List<Bloc> DiscardInvalidNeighbors(Bloc refBloc, ref List<Bloc> neighbors, ref List<Bloc> seen)
	{
		List<Bloc> list = new List<Bloc>(neighbors.Except(seen));

		BlocRefDelegate del = new BlocRefDelegate(refBloc);
		list.RemoveAll(del.BlocIsHigher);
		list.RemoveAll(del.BlocHasMoreLava);
		return list;
	}

	private void MakeSpread(ref List<Bloc> spreadFrom, ref List<Bloc> processed)
	{
		if(spreadFrom.Count == 0)
			return;

		List<Bloc> nextRound = new List<Bloc>();

		foreach(Bloc bloc in spreadFrom)
		{
			List<Bloc> surroundings = Map.FetchNeighbors2D(bloc.indexInMap, 1);
			
			List<Bloc> validNeighbors = DiscardInvalidNeighbors(bloc, ref surroundings, ref processed);
			
			if(validNeighbors.Count == 0)
			{
				//check si passe au dessus
				continue;
			}
			
			int denominator = validNeighbors.Count;
			
			foreach(Bloc neighbor in validNeighbors)
			{
				denominator += Bloc.IsLower(neighbor, bloc) ? 1 : 0 ;
			}

			int amountToShare = bloc.Elements.Lava - 1;

			if((amountToShare / denominator) < 1) //not enough to share
				continue; //skip TODO replace ave les voisins directs

			foreach(Bloc neighbor in validNeighbors)
			{
				int share = Bloc.IsLower(neighbor, bloc) ? 2 : 1 ;
				int amountMoved = (int) Mathf.Round(amountToShare * ((float)share / (float)denominator));
				neighbor.Elements.Lava += amountMoved;
				bloc.Elements.Lava -= amountMoved;
				Debug.Log (amountMoved + " from " + bloc.name + " to " + neighbor.name);
				if(neighbor.Elements.Lava > 1)
					nextRound.Add(neighbor);
			}

			processed.Add(bloc);
		}

		nextRound.Sort(new BlocRefDelegate(null));

		MakeSpread(ref nextRound, ref processed);
	}
}

class BlocRefDelegate : IComparer<Bloc>
{
	Bloc _refBloc;

	public BlocRefDelegate( Bloc refBloc )
	{	_refBloc = refBloc;	}

	public bool BlocIsHigher(Bloc bloc)
	{
		return Bloc.IsHigher(bloc, _refBloc);  
	}

	public bool BlocHasMoreLava(Bloc bloc)
	{
		return !Bloc.IsLower(bloc, _refBloc) && (bloc.Elements.Lava > _refBloc.Elements.Lava);
	}

	public int Compare(Bloc left, Bloc right)
	{
		if(left.Elements.Lava < right.Elements.Lava)
			return 1;
		else if (left.Elements.Lava > right.Elements.Lava)
			return -1;
		else
			return 0;
	}
}
