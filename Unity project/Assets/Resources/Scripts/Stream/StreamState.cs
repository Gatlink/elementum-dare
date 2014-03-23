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

		public void TransmitBuffer()
		{
			value += buffer;
			buffer = 0;
		}
	}

	private Dictionary<Source.SourceType, StreamValues> _state;
	
	///////////////////////////////////////////////////// WATER
	public StreamValues Water
	{
		get{ return _state[Source.SourceType.Water]; }
		set{ _state[Source.SourceType.Water] = value; }
	}
	
	///////////////////////////////////////////////////// SAND
	public StreamValues Sand
	{
		get{ return _state[Source.SourceType.Sand]; }
		set{ _state[Source.SourceType.Sand] = value; }
	}
	
	///////////////////////////////////////////////////// LAVA
	public StreamValues Lava
	{
		get{ return _state[Source.SourceType.Lava]; }
		set{  _state[Source.SourceType.Lava] = value; }
	}
	
	///////////////////////////////////////////////////// ELEC & WIND
	//irrelevant
	
	///////////////////////////////////////////////////// ACCESSORS & HELPERS
	public List<Source.SourceType> CurrentTypes
	{
		get
		{
			List<Source.SourceType> list = new List<Source.SourceType>();
			
			foreach(KeyValuePair<Source.SourceType, StreamValues> pair in _state)
			{
				if(pair.Value.value > 0)
				{
					list.Add(pair.Key);
				}
			}
			
			return list;
		}
	}
	
	public StreamValues this[Source.SourceType type]
	{
		get{ return _state[type]; }
		set{ _state[type] = value; }
	}
	
	public StreamsState()
	{
		_state = new Dictionary<Source.SourceType, StreamValues>();
		
		_state.Add(Source.SourceType.Sand, new StreamValues());
		_state.Add(Source.SourceType.Lava, new StreamValues());
		_state.Add(Source.SourceType.Water, new StreamValues());
		_state.Add(Source.SourceType.Electricity, new StreamValues());
		_state.Add(Source.SourceType.Wind, new StreamValues());
	}
	
	public new string ToString()
	{
		string msg = "ELEMENTS:\n";
		
		foreach(KeyValuePair<Source.SourceType, StreamValues> pair in _state)
		{
			msg += pair.Key.ToString() + " -> " + pair.Value.ToString() + "\n";
		}
		
		return msg;
	}

	public void Resolve(Source.SourceType type, bool animated)
	{
		//TODO resolve interactions with current stream type
		//TODO animate
		_state[type].TransmitBuffer(); 
	}
}
