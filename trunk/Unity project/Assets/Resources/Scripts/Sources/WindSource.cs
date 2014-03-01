using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindSource : Source
{	
	public override void RunSource() 
	{
		List<Bloc> surroundings =  Map.FetchNeighbors(_bloc, 1, true);
		
		foreach(Bloc bloc in surroundings)
		{
			bloc.HasWindBlowing = true;
		}
	}

	public override void KillSource()
	{
		List<Bloc> surroundings =  Map.FetchNeighbors(_bloc, 1, true);

		foreach(Bloc bloc in surroundings)
		{
			bloc.HasWindBlowing = false;
		}
	}
}
