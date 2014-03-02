using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricitySource : Source
{
	private List<Bloc> _electrified = new List<Bloc>();
	System.Func<Bloc, bool> checkConductivity = (bloc) => { return bloc.IsConductor(); };

	public override void RunSource()
	{
		if(_bloc == null)
		{
			Debug.LogError ("Updating a source affected to no bloc.");
			return;
		}

		ShutDownPower();

		List<Bloc> neighbors =  Map.FetchNeighborsIf(_bloc, 1, checkConductivity, true, true);
		foreach(Bloc bloc in neighbors)
		{
			bloc.IsElectrified = true;
			_electrified.Add(bloc);
		}

		//Look for conducting neighbors not electrified
		for(int i = 0; i <  _electrified.Count; ++i)
		{
			Bloc bloc = _electrified[i];
			List<Bloc> surroundings =  Map.FetchNeighborsIf(bloc, 1, checkConductivity, true, false);
			foreach(Bloc neighbor in surroundings)
			{
				if(!neighbor.IsElectrified)
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
