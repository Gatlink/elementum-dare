using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Bloc : MonoBehaviour 
{
	public enum BlocType
	{
		TerrainBloc,
		Earth,
		Rock,
		Ice,
		Metal,
		Plant, 
		UpgradedPlant
	};
	public static int NB_OF_TYPES = (int)BlocType.UpgradedPlant;
	// must be last of enum

#region StreamState
	public class StreamsState
	{
		private Dictionary<Stream.StreamType, ushort> _state;

		///////////////////////////////////////////////////// WATER
		public ushort Water
		{
			get{ return _state[Stream.StreamType.Water]; }
			set{ _state[Stream.StreamType.Water] = value; }
		}

		///////////////////////////////////////////////////// SAND
		public ushort Sand
		{
			get{ return _state[Stream.StreamType.Sand]; }
			set{ _state[Stream.StreamType.Sand] = value; }
		}

		///////////////////////////////////////////////////// LAVA
		public ushort Lava
		{
			get{ return _state[Stream.StreamType.Lava]; }
			set{  _state[Stream.StreamType.Lava] = value; }
		}

		///////////////////////////////////////////////////// ELEC & WIND


		///////////////////////////////////////////////////// ACCESSORS & HELPERS
		public List<Stream.StreamType> CurrentTypes
		{
			get
			{
				List<Stream.StreamType> list = new List<Stream.StreamType>();

				foreach(KeyValuePair<Stream.StreamType, ushort> pair in _state)
				{
					if(pair.Value > 0)
					{
						list.Add(pair.Key);
					}
				}

				return list;
			}
		}

		public ushort this[Stream.StreamType type]
		{
			get{ return _state[type]; }
			set{ _state[type] = value; }
		}

		public StreamsState()
		{
			_state = new Dictionary<Stream.StreamType, ushort>();

			_state.Add(Stream.StreamType.Sand, 0);
			_state.Add(Stream.StreamType.Lava, 0);
			_state.Add(Stream.StreamType.Water, 0);
			_state.Add(Stream.StreamType.Electricity, 0);
			_state.Add(Stream.StreamType.Wind, 0);
		}

		public new string ToString()
		{
			string msg = "ELEMENTS:\n";

			foreach(KeyValuePair<Stream.StreamType, ushort> pair in _state)
			{
				msg += pair.Key.ToString() + " -> " + pair.Value.ToString() + "\n";
			}

			return msg;
		}
	}

	private StreamsState _streamsState = new StreamsState();
	public StreamsState Streams { get{ return _streamsState; } }

	public bool IsFlooded
	{
		get{ return _streamsState[Stream.StreamType.Water] > 0; }
	}
	
	public bool IsQuickSanded
	{
		get{ return _streamsState[Stream.StreamType.Sand] > 0; }
	}
	
	public bool IsUnderLava
	{
		get{ return _streamsState[Stream.StreamType.Lava] > 0; }
	}
	
	public bool IsElectrified 
	{
		get{ return _streamsState[Stream.StreamType.Electricity] > 0; } 
		set{ _streamsState[Stream.StreamType.Electricity] = (_type == BlocType.Metal || IsFlooded) && value ? (ushort)1 : (ushort)0; }
	}

	public bool HasWindBlowing
	{
		get{ return _streamsState[Stream.StreamType.Wind] > 0; } 
		set{ _streamsState[Stream.StreamType.Wind] = value ? (ushort)1 : (ushort)0; }
	}
#endregion

	private BlocType _type;
	public BlocType Type 
	{ 
		get{ return _type; }
		set{ _type = value; }
	}

	public BlocIndex indexInMap {get; private set;}

	private Source _sourceObject = null;
	private Dictionary<Stream.StreamType, Stream> _streamObjects = new Dictionary<Stream.StreamType, Stream>();
	private Unit _unitObject = null;

	public bool HoldASource()
	{	return _sourceObject != null;	}
	
	public void ReceiveSource(Source source)
	{	_sourceObject = source;	}

	public bool HostAUnit()
	{	return _unitObject != null;	}

	public bool WelcomeUnit(Unit unit)
	{	return _unitObject = unit;	}

	public void InsertedAt(BlocIndex pos)
	{
		indexInMap = pos;

		gameObject.transform.position = Map.IndexToPosition(pos);
		gameObject.transform.parent = Map.GetMapRefTransform();
	}

	public bool IsReachable()
	{
		if(HoldASource() || HostAUnit())
			return false;

		return true;
	}

	public static bool IsLower(Bloc a, Bloc b)
	{
		return (a.indexInMap.z < b.indexInMap.z );
	}

	public static bool IsHigher(Bloc a, Bloc b)
	{	return (a.indexInMap.z > b.indexInMap.z );	}

	// Use this for initialization
	void Start(){}
	
	// Update is called once per frame
	void Update() 
	{
		//TODO stream interaction
		UpdateStreamsState();
		UpdateStreamsVisuals();
	}

	void UpdateStreamsState()
	{
		List<Stream.StreamType> currentTypes = Streams.CurrentTypes;
		if(currentTypes.Count > 0)
		{
			//Remove all the streams that aren't supposed to still be there
			List<Stream.StreamType> disposableStreams = new List<Stream.StreamType>();
			foreach(KeyValuePair<Stream.StreamType, Stream> p in _streamObjects)
			{
				if(!currentTypes.Contains(p.Key))
				{
					Stream tmp = p.Value;
					StreamManager.Instance().RemoveStream(ref tmp);
					disposableStreams.Add(p.Key);
				}
			}

			//Remove the corresponding entries in the dictionary
			foreach(Stream.StreamType key in disposableStreams)
			{
				_streamObjects.Remove(key);
			}

			//Create the missing streams
			Vector3 streamPos = Map.IndexToPosition(indexInMap.x, indexInMap.y, indexInMap.z + 1);
			foreach(Stream.StreamType key in currentTypes)
			{
				if(!_streamObjects.ContainsKey(key))
				{
					Stream tmpStream = StreamManager.Instance().CreateStream(key, this);
					tmpStream.gameObject.transform.position = streamPos;
					_streamObjects.Add(key, tmpStream);
				}
			}
		}
		else
		{
			//Remove all the streams
			foreach(KeyValuePair<Stream.StreamType, Stream> p in _streamObjects)
			{
				Stream tmp = p.Value;
				StreamManager.Instance().RemoveStream(ref tmp);
			}
			_streamObjects.Clear();
		}
	}

	void UpdateStreamsVisuals()
	{
		foreach(var p in _streamObjects)
		{
			p.Value.UpdateStreamVisual();
		}
	}

	public bool IsConductor()
	{	return (_type == Bloc.BlocType.Metal) || IsFlooded;	}

	public new string ToString()
	{
		string blocIdentifier = gameObject.name.ToUpper() + ":";
		string blocCoords = "COORDS: " + indexInMap.ToString();
		string blocType = "TYPE: " + Type.ToString();
		string blocState = string.Format("STATE:\nElec: {0}\nWind: {1}", IsElectrified, HasWindBlowing);
		string blocStreams = Streams.ToString();
		string msg = string.Format ("{0}\n{1}\n{2}\n{3}\n{4}\n", blocIdentifier, blocCoords, blocType, blocState, blocStreams);
		return msg;
	}

	public int FlatDistance(Bloc other)
	{
		BlocIndex othIdx = other.indexInMap;
		var distance = Math.Abs(othIdx.x - indexInMap.x) + Math.Abs(othIdx.y - indexInMap.y);
		return distance;
	}

	public struct SortByStreamVolume : IComparer<Bloc>
	{
		private Stream.StreamType _checkType;
		private bool _desc;
		
		public SortByStreamVolume(Stream.StreamType type, bool desc)
		{ 
			_checkType = type;
			_desc = desc;
		}

		public int Compare(Bloc left, Bloc right)
		{
			if(_desc)
				return CompareDesc(left, right);
			else
				return CompareAsc(left, right);
		}

		public int CompareAsc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType] < right.Streams[_checkType])
				return 1; //right goes after left
			else if (left.Streams[_checkType] > right.Streams[_checkType])
				return -1; //left goes after right
			else
				return 0; //equal
		}

		public int CompareDesc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType] > right.Streams[_checkType])
				return 1; //right goes after left
			else if (left.Streams[_checkType] < right.Streams[_checkType])
				return -1; //left goes after right
			else
				return 0; //equal
		}
	}
	
	public static Vector3 GetBlocSizeByType(Bloc.BlocType type = Bloc.BlocType.TerrainBloc)
	{
		return BlocFactory.GetBlocSizeByType(type);
	}

	public Vector3 GetBlocSize()
	{
		return BlocFactory.GetBlocSizeByType(_type);
	}

	public Stream GetStreamOfType(Stream.StreamType type)
	{
		if(_streamObjects.ContainsKey(type))
			return _streamObjects[type];
		else
			return null;
	}
}