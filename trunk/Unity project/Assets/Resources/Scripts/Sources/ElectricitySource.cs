using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricitySource : Source
{
	private List<Bloc> _electrified = new List<Bloc>();

	public override void RunSource()
	{
		_electrified.Clear();

		List<Bloc> neighbors =  Map.FetchNeighbors(_bloc, 1, true, true);
		foreach(Bloc bloc in neighbors)
		{
			if(bloc.IsConductor())
				_electrified.Add(bloc);
		}

		//Be sure all direct conducting neighbors are electrified
		foreach(Bloc bloc in _electrified)
		{
			bloc.IsElectrified = true;
		}

		//Then look for conducting neighbors not electrified
		foreach(Bloc bloc in _electrified)
		{
			List<Bloc> surroundings =  Map.FetchNeighbors(bloc, 1, true, false);
			foreach(Bloc neighbor in surroundings)
			{
				if(neighbor.IsConductor() && !neighbor.IsElectrified)
				{
					neighbor.IsElectrified = true;
					_electrified.Add(neighbor);
				}
			}
		}

	}

	public override void KillSource()
	{
		foreach(Bloc bloc in _electrified)
		{
			bloc.IsElectrified = false;
		}
		_electrified.Clear();
	}
}
