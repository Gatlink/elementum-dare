using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Stream : MonoBehaviour
{
	public enum StreamType
	{
		Sand,
		Lava,
		Water,
		Wind,
		Electricity
	}
	public static int NB_OF_TYPES = (int)StreamType.Electricity + 1;
	// must be last of enum

	private const float TOTAL_ANIM_TIME = 1.5f;
	private float _animTime = -1.0f;

	protected float _granularity;
	protected int _treshold;
	protected int _erosion;
	protected float _flatFactor;
	protected float _slopeFactor;

	protected StreamType _type;
	public StreamType Type 
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

	public static bool IsFluid( StreamType streamType)
	{
		return ( streamType == StreamType.Sand
		        || streamType == StreamType.Water
		        || streamType == StreamType.Lava
		        );
	}

	public bool IsFluid()
	{	return Stream.IsFluid(_type);	}
	
	protected Bloc _bloc;

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
			return _bloc.Streams[_type].value;
		else
			return -1;
	}
	
	public void PlaceOnBloc(Bloc bloc)
	{
		_bloc = bloc;
	}

	public abstract void UpdateStream();
	public abstract void UpdateStreamVisual();
}