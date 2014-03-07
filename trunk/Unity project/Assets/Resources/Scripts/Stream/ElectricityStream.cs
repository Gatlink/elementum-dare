using UnityEngine;
using System.Collections;

public class ElectricityStream : Stream
{
	public override void UpdateStream()
	{ /*do nothing, handled by the source */}

	public override void UpdateStreamVisual()
	{
		Vector3 bias = new Vector3(1.15f, 1.15f, 1.15f);

		float height = 0;
		if(_bloc.IsFlooded)
		{
			height = _bloc.GetStreamOfType(Stream.StreamType.Water).gameObject.transform.localScale.y;
		}

		//Update stream to surround the bloc
		Vector3 delta = new Vector3(_bloc.transform.localScale.x * bias.x, (_bloc.transform.localScale.y * bias.y) + height, _bloc.transform.localScale.z * bias.z);
		gameObject.transform.localScale = delta;//(_bloc.GetBlocSize() + bias);
		gameObject.transform.position = _bloc.transform.position;
	}
}
