using UnityEngine;
using System.Collections.Generic;

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
	public static int NB_OF_TYPES = (int)SourceType.Water;
	// must be last of enum

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


class SourceBlocRefDelegate
{
	Bloc _refBloc;
	Stream.StreamType checkType;
	
	public SourceBlocRefDelegate( Stream.StreamType type, Bloc refBloc = null )
	{	
		checkType = type;
		_refBloc = refBloc;	
	}
	
	public bool BlocIsHigher(Bloc bloc)
	{
		if(_refBloc == null)
			return true;

		return Bloc.IsHigher(bloc, _refBloc);  
	}
	
	public bool BlocHasMoreOfStream(Bloc bloc)
	{
		if(_refBloc == null)
			return false;

		return !Bloc.IsLower(bloc, _refBloc) && (bloc.Elements[checkType] > _refBloc.Elements[checkType]);
	}
}

public struct StreamAscSorter : IComparer<Bloc>
{
	private Stream.StreamType checkType;

	public StreamAscSorter(Stream.StreamType type)
	{ checkType = type; }

	public int Compare(Bloc left, Bloc right)
	{
		if(left.Elements[checkType] < right.Elements[checkType])
			return 1; //right goes after left
		else if (left.Elements[checkType] > right.Elements[checkType])
			return -1; //left goes after right
		else
			return 0; //equal
	}
}


public struct StreamDescSorter : IComparer<Bloc>
{
	private Stream.StreamType checkType;
	
	public StreamDescSorter(Stream.StreamType type)
	{ checkType = type; }

	public int Compare(Bloc left, Bloc right)
	{
		if(left.Elements[checkType] > right.Elements[checkType])
			return 1; //right goes after left
		else if (left.Elements[checkType] < right.Elements[checkType])
			return -1; //left goes after right
		else
			return 0; //equal
	}
}