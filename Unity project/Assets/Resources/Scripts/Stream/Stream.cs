using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Stream : MonoBehaviour
{
	private const float TOTAL_ANIM_TIME = 1.5f;
	private float _animTime = -1.0f;

	protected float _granularity;
	protected int _treshold;
	protected int _erosion;
	protected float _flatFactor;
	protected float _slopeFactor;

	protected Source.SourceType _type;
	public Source.SourceType Type 
	{ get {return _type;} }

	public void Initialize(StreamInfo stream)
	{
		_type = stream.type;
		
		_granularity = stream.granularity;
		_treshold = stream.treshold;
		_erosion = stream.erosion;
		_flatFactor = stream.flatFactor;
		_slopeFactor = stream.slopeFactor;
		
		_animTime = -1.0f;
	}

	public static bool IsFluid( Source.SourceType streamType)
	{
		return ( streamType == Source.SourceType.Sand
		        || streamType == Source.SourceType.Water
		        || streamType == Source.SourceType.Lava
		        );
	}

	public bool IsFluid()
	{	return Stream.IsFluid(_type);	}
	
	protected Bloc _bloc;
	public Bloc Bloc
	{
		get { return _bloc; }
		set 
		{
			if (value != null)
			{
				BlocIndex streamIndex = value.indexInMap;
				streamIndex.z += 1;
				transform.position = Map.IndexToPosition(streamIndex);
				_bloc = value;
			}
			else
				_bloc = null;
		}
	}

	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	public virtual void Update() {}

	public int GetAltitude()
	{
		if(_bloc)
			return _bloc.indexInMap.z + 1;
		else
			return -1;
	}
	
	public int GetVolume()
	{
		if(_bloc)
			return _bloc.Streams[_type].Value;
		else
			return -1;
	}
	
	public void PlaceOnBloc(Bloc bloc)
	{
		_bloc = bloc;
	}

	public abstract void UpdateStream();
	public abstract void UpdateStreamVisual();
	public abstract void Erode();
}