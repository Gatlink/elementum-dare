using UnityEngine;
using System.Collections.Generic;

public class StreamsState
{
	public class StreamValues
	{
		public StreamValues()
		{ value = 0; buffer = 0; }

		public StreamValues(int val)
		{ value = val; buffer = 0; }

		public StreamValues(int val, int buf)
		{ value = val; buffer = buf; }

		public int value;
		public int buffer;

		public new string ToString()
		{
			string msg = "v:" + value + " / b:" + buffer;
			return msg;
		}
	}

	private Dictionary<Stream.StreamType, StreamValues> _state;
	
	///////////////////////////////////////////////////// WATER
	public StreamValues Water
	{
		get{ return _state[Stream.StreamType.Water]; }
		set{ _state[Stream.StreamType.Water] = value; }
	}
	
	///////////////////////////////////////////////////// SAND
	public StreamValues Sand
	{
		get{ return _state[Stream.StreamType.Sand]; }
		set{ _state[Stream.StreamType.Sand] = value; }
	}
	
	///////////////////////////////////////////////////// LAVA
	public StreamValues Lava
	{
		get{ return _state[Stream.StreamType.Lava]; }
		set{  _state[Stream.StreamType.Lava] = value; }
	}
	
	///////////////////////////////////////////////////// ELEC & WIND
	//irrelevant
	
	///////////////////////////////////////////////////// ACCESSORS & HELPERS
	public List<Stream.StreamType> CurrentTypes
	{
		get
		{
			List<Stream.StreamType> list = new List<Stream.StreamType>();
			
			foreach(KeyValuePair<Stream.StreamType, StreamValues> pair in _state)
			{
				if(pair.Value.value > 0)
				{
					list.Add(pair.Key);
				}
			}
			
			return list;
		}
	}
	
	public StreamValues this[Stream.StreamType type]
	{
		get{ return _state[type]; }
		set{ _state[type] = value; }
	}
	
	public StreamsState()
	{
		_state = new Dictionary<Stream.StreamType, StreamValues>();
		
		_state.Add(Stream.StreamType.Sand, new StreamValues());
		_state.Add(Stream.StreamType.Lava, new StreamValues());
		_state.Add(Stream.StreamType.Water, new StreamValues());
		_state.Add(Stream.StreamType.Electricity, new StreamValues());
		_state.Add(Stream.StreamType.Wind, new StreamValues());
	}
	
	public new string ToString()
	{
		string msg = "ELEMENTS:\n";
		
		foreach(KeyValuePair<Stream.StreamType, StreamValues> pair in _state)
		{
			msg += pair.Key.ToString() + " -> " + pair.Value.ToString() + "\n";
		}
		
		return msg;
	}
}
