using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricitySource : Source
{
	private List<Bloc> _electrified = new List<Bloc>();

	public override void RunSource()
	{
		if(_bloc == null)
		{
			Debug.LogError ("Updating a source affected to no bloc.");
			return;
		}

		ShutDownPower();

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
		for(int i = 0; i <  _electrified.Count; ++i)
		{
			Bloc bloc = _electrified[i];
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
		ShutDownPower();
	}

	private void ShutDownPower()
	{
		foreach(Bloc bloc in _electrified)
		{
			bloc.IsElectrified = false;
		}
		_electrified.Clear();
	}
}
