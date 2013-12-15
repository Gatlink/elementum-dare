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
		Upgraded_Plant
	};

	public class ElementsState
	{
		private Dictionary<Stream.StreamType, int> _state;

		///////////////////////////////////////////////////// WATER
		public bool IsFlooded
		{
			get
			{ return _state[Stream.StreamType.Water] > 0; }
		}
		public int Water
		{
			get
			{ return _state[Stream.StreamType.Water]; }

			set 
			{ _state[Stream.StreamType.Water] = value; }
		}

		///////////////////////////////////////////////////// SAND
		public bool IsQuickSanded
		{
			get
			{ return _state[Stream.StreamType.Sand] > 0; }
		}
		public int Sand
		{
			get
			{ return _state[Stream.StreamType.Sand]; }
			
			set 
			{ _state[Stream.StreamType.Sand] = value; }
		}

		///////////////////////////////////////////////////// LAVA
		public bool IsUnderLava
		{
			get
			{ return _state[Stream.StreamType.Lava] > 0; }
		}
		public int Lava
		{
			get
			{ return _state[Stream.StreamType.Lava]; }
			
			set 
			{  _state[Stream.StreamType.Lava] = value; }
		}

		///////////////////////////////////////////////////// ELEC & WIND
		public bool IsElectrified {get; set;}
		public bool HasWindBlowing {get; set;}

		public bool HasStreamOfType(Stream.StreamType type)
		{
			return _state[type] > 0;
		}

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

		public ElementsState()
		{
			_state = new Dictionary<Stream.StreamType, int>();

			_state.Add(Stream.StreamType.Sand, 0);
			_state.Add(Stream.StreamType.Lava, 0);
			_state.Add(Stream.StreamType.Water, 0);
		}
	}

	private ElementsState _elements;

	public ElementsState Elements { get{ return _elements; } }

	public BlocIndex indexInMap {get; private set;}

	private Source _source;
	private Unit _unit = null;

	public bool HoldASource()
	{
		return _source != null;
	}
	
	public void ReceiveSource(Source source)
	{
		_source = source;
	}

	public bool HostAUnit()
	{
		return _unit != null;
	}

	public bool WelcomeUnit(Unit unit)
	{
		return _unit = unit;
	}

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

	public bool CanTakeFlow()
	{
		return !HoldASource();
	}

	// Use this for initialization
	void Start()
	{
		_elements = new ElementsState();
	}
	
	// Update is called once per frame
	void Update() {}
}
