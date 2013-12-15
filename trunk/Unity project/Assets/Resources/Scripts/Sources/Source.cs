using UnityEngine;
using System.Collections;

public abstract class Source : MonoBehaviour
{
	public enum SourceType
	{
		Sand,
		Lava,
		Electricity,
		Wind,
		Water
	}

	protected SourceType _type;

	protected int _generate;
	protected int _duration;

	private Bloc _bloc;

	public bool IsOnBloc()
	{
		return _bloc != null;
	}

	public void SetOnBloc(Bloc bloc)
	{
		_bloc = bloc;
	}

	public void Initialize(SourceInfo source)
	{
		_type = source.type;

		_generate = source.generate;
		_duration = source.duration;
	}

	public abstract void RunSource();

	public void UpdateSourceState()
	{
		--_duration;

		if(_duration == 0)
		{
			//Debug.Log ("Dead " + ToString());

			//TODO remove source from game
		}
	}

	public static SourceType GetRandomSourceType()
	{
		SourceType begin = Source.SourceType.Sand;
		SourceType end = Source.SourceType.Water;

		float startRange = (int)begin - 0.49f;
		float endRange = (int)end + 0.49f;

		int rand = (int)System.Math.Round(Random.Range(startRange, endRange));

		return (SourceType) rand;
	}

	public new string ToString()
	{
		return gameObject.name;
	}
}