using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LavaSource : Source
{
	public override void RunSource()
	{
		if(_bloc == null)
		{
			Debug.LogError ("Updating a source affected to no bloc.");
			return;
		}

		//put everything on self bloc
		_bloc.Streams.Lava.Generate(_generate); //TODO animate

		/*List<Bloc> update = new List<Bloc>();
		//update.Add(_bloc);

		//List<Bloc> processedList = MakeSpread(ref update);

		// KEEP FOR TWEAKING?
		// Reprocess all first-pass processed blocs for safety
		// Process them the other way around to balance the streams on the edges

		//processedList.Sort(new BlocRefDelegate(null));
		//MakeSpread(ref processedList);*/
	}
	
	public override void KillSource(){}

/*
	private List<Bloc> DiscardInvalidNeighbors(Bloc refBloc, ref List<Bloc> neighbors, ref List<Bloc> seen)
	{
		List<Bloc> list = new List<Bloc>(neighbors.Except(seen));

		SourceBlocRefDelegate del = new SourceBlocRefDelegate(Source.SourceType.Lava, refBloc);
		list.RemoveAll(del.BlocIsHigher);
		list.RemoveAll(del.BlocHasMoreOfStream);
		return list;
	}

	private List<Bloc> MakeSpread(ref List<Bloc> spreadFrom, List<Bloc> processed = null)
	{
		if(processed == null)
			processed = new List<Bloc>();

		if(spreadFrom.Count == 0)
			return processed;

		List<Bloc> nextRound = new List<Bloc>();

		foreach(Bloc bloc in spreadFrom)
		{
			processed.Add(bloc);

			if(bloc.Elements.Lava <= 1)
				continue;

			List<Bloc> surroundings = Map.FetchNeighbors2D(bloc.indexInMap, 1);
			
			List<Bloc> validNeighbors = DiscardInvalidNeighbors(bloc, ref surroundings, ref processed);

			int validated = validNeighbors.Count;

			if(validated == 0)
			{
				//check si passe au dessus

				continue;
			}

			int maxNbOfNeighbors = 4;
			int oneShare = (int) Mathf.Floor((float) bloc.Elements.Lava / (float)(maxNbOfNeighbors + 1));
			//int amountToShare = (int) Mathf.Floor((float) denominator * ((float) bloc.Elements.Lava / 5.0f));
			int amountToShare = validated * oneShare;
			amountToShare += (maxNbOfNeighbors - validated) * (int) Mathf.Floor(oneShare * 0.5f);
			//half a share for the invalid neighbors to be redistributed

			int denominator = validated;
			foreach(Bloc neighbor in validNeighbors)
			{
				denominator += Bloc.IsLower(neighbor, bloc) ? 2 : 0 ;
			}

			if((amountToShare / denominator) < 1) //not enough to share
				continue; //skip TODO replace ave les voisins directs

			foreach(Bloc neighbor in validNeighbors)
			{
				int share = Bloc.IsLower(neighbor, bloc) ? 3 : 1 ;
				int amountMoved = (int) Mathf.Round(amountToShare * ((float)share / (float)denominator));
				neighbor.Elements.Lava += amountMoved;
				bloc.Elements.Lava -= amountMoved;

				//Debug.Log (amountMoved + " from " + bloc.name + " to " + neighbor.name);

				if(neighbor.Elements.Lava > 1)
					nextRound.Add(neighbor);
			}
		}

		nextRound.Sort(new StreamDescSorter(Source.SourceType.Lava));

		return MakeSpread(ref nextRound, processed);
	}
	*/
}
