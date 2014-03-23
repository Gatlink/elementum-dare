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

	private StreamsState _streamsState = new StreamsState();
	public StreamsState Streams { get{ return _streamsState; } }

	public bool HasStreamOfType(Source.SourceType type)
	{	return Streams[type].value > 0;	}

	public bool HasPendingStreamOfType(Source.SourceType type)
	{	return Streams[type].buffer > 0; }

	public bool IsFlooded
	{
		get{ return HasStreamOfType(Source.SourceType.Water); }
	}
	
	public bool IsQuickSanded
	{
		get{ return HasStreamOfType(Source.SourceType.Sand); }
	}
	
	public bool IsUnderLava
	{
		get{ return HasStreamOfType(Source.SourceType.Lava); }
	}
	
	public bool IsElectrified 
	{
		get{ return HasStreamOfType(Source.SourceType.Electricity); } 
		set{ Streams[Source.SourceType.Electricity].value = (_type == BlocType.Metal || IsFlooded) && value ? 1 : 0; }
	}

	public bool HasWindBlowing
	{
		get{ return HasStreamOfType(Source.SourceType.Wind); } 
		set{ Streams[Source.SourceType.Wind].value = value ? 1 : 0; }
	}

	private BlocType _type;
	public BlocType Type 
	{ 
		get{ return _type; }
		set
		{
			if (value != _type)
			{
				_type = value;
				renderer.material = Resources.Load<Material>(string.Format("Mesh/Materials/Bloc_{0}", value));
			}
		}
	}

	public BlocIndex indexInMap {get; private set;}

	private Source _sourceObject = null;
	public Source Source
	{
		get { return _sourceObject; }
		set { _sourceObject = value;}
	}

	private Dictionary<Source.SourceType, Stream> _streamObjects = new Dictionary<Source.SourceType, Stream>();
	private Unit _unitObject = null;

	public bool HoldASource()
	{	return _sourceObject != null;	}

	public bool HostAUnit()
	{	return _unitObject != null;	}

	public bool WelcomeUnit(Unit unit)
	{	return _unitObject = unit;	}

	public void InsertedAt(BlocIndex pos)
	{
		indexInMap = pos;

		transform.position = Map.IndexToPosition(pos);
		transform.parent = Map.GetMapRefTransform();
		_gui.transform.position = transform.position;
	}

	public bool IsReachable()
	{
		if(HoldASource() || HostAUnit())
			return false;

		return true;
	}

	public static bool IsLower(Bloc a, Bloc b)
	{	return (a.indexInMap.z < b.indexInMap.z );	}

	public bool IsLowerThan(Bloc other)
	{	return (indexInMap.z < other.indexInMap.z );}

	public static bool IsHigher(Bloc a, Bloc b)
	{	return (a.indexInMap.z > b.indexInMap.z );	}

	public bool IsHigherThan(Bloc other)
	{	return (indexInMap.z > other.indexInMap.z );}

	// Use this for initialization
	void Awake()
	{
		_gui = (GameObject) UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefab/BlocGUI"));
		_gui.name = name + "GUI";
		_gui.transform.position = transform.position;
		_gui.SetActive(false);
	}
	
	// Update is called once per frame
	void Update() 
	{
		//TODO stream interaction
		UpdateStreamsState();
	}

	void UpdateStreamsState()
	{
		List<Source.SourceType> currentTypes = Streams.CurrentTypes;
		if(currentTypes.Count > 0)
		{
			//Remove all the streams that aren't supposed to still be there
			List<Source.SourceType> disposableStreams = new List<Source.SourceType>();
			foreach(KeyValuePair<Source.SourceType, Stream> p in _streamObjects)
			{
				if(!currentTypes.Contains(p.Key))
					disposableStreams.Add(p.Key);
			}

			//Remove the corresponding entries in the dictionary
			foreach(Source.SourceType key in disposableStreams)
			{
				StreamManager.Instance().RemoveStream(_streamObjects[key]);
				_streamObjects.Remove(key);
			}
			disposableStreams.Clear();

			//Create the missing streams
			Vector3 streamPos = Map.IndexToPosition(indexInMap.x, indexInMap.y, indexInMap.z + 1);
			foreach(Source.SourceType key in currentTypes)
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
			foreach(KeyValuePair<Source.SourceType, Stream> p in _streamObjects)
			{
				StreamManager.Instance().RemoveStream(p.Value);
			}
			_streamObjects.Clear();
		}

		foreach(KeyValuePair<Source.SourceType, Stream> p in _streamObjects)
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

	public bool IsTopMostBloc()
	{
		return Map.GetBlocAt(indexInMap.x, indexInMap.y) == this;
	}

	public struct SortByStreamVolume : IComparer<Bloc>
	{
		private Source.SourceType _checkType;
		private bool _desc;
		
		public SortByStreamVolume(Source.SourceType type, bool desc)
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
			if(left.Streams[_checkType].value < right.Streams[_checkType].value)
				return 1; //right goes after left
			else if (left.Streams[_checkType].value > right.Streams[_checkType].value)
				return -1; //left goes after right
			else
				return 0; //equal
		}

		public int CompareDesc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType].value > right.Streams[_checkType].value)
				return 1; //right goes after left
			else if (left.Streams[_checkType].value < right.Streams[_checkType].value)
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

	public Stream GetStreamOfType(Source.SourceType type)
	{
		if(_streamObjects.ContainsKey(type))
			return _streamObjects[type];
		else
			return null;
	}

	public void ClearStreams()
	{
		var streamKeys = _streamObjects.Keys.ToList();
		foreach(var streamKey in streamKeys)
		{
			if (streamKey != Source.SourceType.Electricity)
			{
				StreamManager.Instance().RemoveStream(_streamObjects[streamKey]);
				_streamObjects.Remove(streamKey);
			}
		}

		foreach(var streamType in (Source.SourceType[])Enum.GetValues(typeof(Source.SourceType)))
		{
			if (streamType != Source.SourceType.Electricity)
				_streamsState[streamType].value = 0;
		}

		if(!IsConductor() && _streamObjects.ContainsKey(Source.SourceType.Electricity))
		{
			_streamsState[Source.SourceType.Electricity].value = 0;
			StreamManager.Instance().RemoveStream(_streamObjects[Source.SourceType.Electricity]);
			_streamObjects.Remove(Source.SourceType.Electricity);
		}
	}

#region GUI

	private GameObject _gui;

	public void Highlight(Color color)
	{
		_gui.SetActive(true);
		_gui.renderer.material.color = color;
	}

	public void RemoveHighlight()
	{
		_gui.SetActive(false);
	}
	
#endregion
}