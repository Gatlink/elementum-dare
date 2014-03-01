using UnityEngine;
using System.Collections.Generic;

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

	public class StreamsState
	{
		private Dictionary<Stream.StreamType, int> _state;

		///////////////////////////////////////////////////// WATER
		public int Water
		{
			get{ return _state[Stream.StreamType.Water]; }
			set{ _state[Stream.StreamType.Water] = value; }
		}

		///////////////////////////////////////////////////// SAND
		public int Sand
		{
			get{ return _state[Stream.StreamType.Sand]; }
			set{ _state[Stream.StreamType.Sand] = value; }
		}

		///////////////////////////////////////////////////// LAVA
		public int Lava
		{
			get{ return _state[Stream.StreamType.Lava]; }
			set{  _state[Stream.StreamType.Lava] = value; }
		}

		///////////////////////////////////////////////////// ELEC & WIND


		///////////////////////////////////////////////////// ACCESSORS & HELPERS
		public Stream.StreamType CurrentType
		{
			get
			{
				Stream.StreamType t = Stream.StreamType.None;
				int val = 0;

				foreach(KeyValuePair<Stream.StreamType, int> pair in _state)
				{
					if(pair.Value > val)
					{
						t = pair.Key;
						val = pair.Value;
					}
				}

				return t;
			}
		}

		public int this[Stream.StreamType type]
		{
			get{ return _state[type]; }
			set{ _state[type] = value; }
		}

		public StreamsState()
		{
			_state = new Dictionary<Stream.StreamType, int>();

			_state.Add(Stream.StreamType.Sand, 0);
			_state.Add(Stream.StreamType.Lava, 0);
			_state.Add(Stream.StreamType.Water, 0);
		}

		public new string ToString()
		{
			string msg = "ELEMENTS:\n";

			foreach(KeyValuePair<Stream.StreamType, int> pair in _state)
			{
				msg += pair.Key.ToString() + " -> " + pair.Value.ToString() + "\n";
			}

			return msg;
		}
	}

	private StreamsState _streamsState;
	public StreamsState Streams { get{ return _streamsState; } }

	private BlocType _type;
	public BlocType Type 
	{ 
		get{ return _type; }
		set{ _type = value; }
	}

	public BlocIndex indexInMap {get; private set;}

	private Source _source = null;
	private Stream _stream = null;
	private Unit _unit = null;

	public bool HoldASource()
	{	return _source != null;	}
	
	public void ReceiveSource(Source source)
	{	_source = source;	}

	public bool HostAUnit()
	{	return _unit != null;	}

	public bool WelcomeUnit(Unit unit)
	{	return _unit = unit;	}

	public void InsertedAt(BlocIndex pos)
	{
		indexInMap = pos;

		gameObject.transform.position = Map.IndexToPosition(pos);
		gameObject.transform.parent = Map.GetMapRefTransform();
	}

	public bool IsReachable() //TODO déterminer si on passe un Vec3 ou un BlocIndex
	{
		if(HoldASource() || HostAUnit())
			return false;

		//TODO déterminer si on lui passe le bloc de départ (potentiellement à 2 blocs d'écart, ou juste le bloc adjacent)
		//TODO déterminer si "à portée" (diff hauteur)
		return true;
	}

	public static bool IsLower(Bloc a, Bloc b)
	{
		return (a.indexInMap.z < b.indexInMap.z );
	}

	public static bool IsHigher(Bloc a, Bloc b)
	{	return (a.indexInMap.z > b.indexInMap.z );	}

	// Use this for initialization
	void Start()
	{	_streamsState = new StreamsState();	}
	
	// Update is called once per frame
	void Update() 
	{
		Stream.StreamType currentType = Streams.CurrentType;
		if(currentType != Stream.StreamType.None)
		{
			if(_stream == null || (_stream != null && _stream.type != currentType))
			{
				if(_stream != null)
					StreamManager.Instance().RemoveStream(ref _stream);

				_stream = StreamManager.Instance().CreateStream(currentType, this);
				_stream.gameObject.transform.position = Map.IndexToPosition(indexInMap.x, indexInMap.y, indexInMap.z + 1);
			}
		}
		else
		{
			if(_stream != null)
				StreamManager.Instance().RemoveStream(ref _stream);

			_stream = null;
		}

		if(_stream != null)
		{
			const int maxVal = 48;

			//Update stream visual according to bloc value
			float delta = (Streams[currentType] * (1.0f / maxVal)) - _stream.gameObject.transform.localScale.y;
			_stream.gameObject.transform.localScale += new Vector3(0, delta, 0);

			//Check if a unit is in the stream
			if(HostAUnit())
			{
				//Should it be damaged ?
				if(currentType == Stream.StreamType.Lava)
				{
					//Apply Lava damages
				}
				else if(currentType == Stream.StreamType.Water && IsElectrified )
				{
					//Apply Electricity damages
				}
			}
		}
	}

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

	private bool _isElectrified;
	public bool IsElectrified 
	{
		get{ return _isElectrified;	} 
		set
		{
			Selectable select = gameObject.GetComponent<Selectable>();

			if(select)
			{
				select.OutlineColor = Color.cyan;
				select.IsSelected = value;
			}
			_isElectrified = value;
		}
	}

	private bool _windBlows;
	public bool HasWindBlowing
	{
		get{ return _windBlows;	} 
		set
		{
			_windBlows = value;
		}
	}

	public bool IsConductor()
	{	return (_type == Bloc.BlocType.Metal) || IsFlooded;	}

	public new string ToString()
	{
		string msg = string.Format("BLOC: {0}\nElectrified -> {1}\nWind blowing -> {2}\n{3}", Type, IsElectrified, HasWindBlowing, Streams.ToString());
		return msg;
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
}