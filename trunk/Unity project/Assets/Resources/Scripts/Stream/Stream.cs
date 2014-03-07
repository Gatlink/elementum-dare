using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Stream : MonoBehaviour
{
	public enum StreamType
	{
		None,
		Sand,
		Lava,
		Water,
		Wind,
		Electricity
	}
	public static int NB_OF_TYPES = (int)StreamType.Electricity;
	// must be last of enum

	public StreamType type { get; set;}
	
	protected Bloc _bloc;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

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
			return _bloc.Streams[type];
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