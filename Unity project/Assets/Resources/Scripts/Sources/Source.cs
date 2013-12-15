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

	protected Bloc _bloc;
	public Bloc Bloc
	{
		get { return _bloc; }
		set 
		{
			if (_bloc != null)
				_bloc.ReceiveSource(null);

			if (value != null)
			{
				BlocIndex sourceIndex = value.indexInMap;
				sourceIndex.z += 1;
				transform.position = Map.IndexToPosition(sourceIndex);
				value.ReceiveSource(this);
				_bloc = value;
			}
			else
				_bloc = null;
		}
	}

	public void Initialize(SourceInfo source)
	{
		_type = source.type;

		_generate = source.generate;
		_duration = source.duration;
	}

	public abstract void RunSource();

	public bool UpdateSourceState()
	{
		--_duration;

		if(_duration == 0)
		{
			//Debug.Log ("Dead " + ToString());

			//TODO remove source from game

			return true;
		}

		return false;
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