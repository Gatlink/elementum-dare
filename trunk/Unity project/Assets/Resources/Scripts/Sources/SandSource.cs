using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SandSource : Source
{
	public override void RunSource()
	{
		if(_bloc == null)
		{
			Debug.LogError ("Updating a source affected to no bloc.");
			return;
		}
		
		//put everything on self bloc
		_bloc.Streams.Sand.Generate(_generate);
	}

	public override void KillSource(){}
}
