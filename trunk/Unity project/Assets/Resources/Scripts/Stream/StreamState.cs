using UnityEngine;
using System.Collections.Generic;

public class StreamsState
{
	public class StreamValues
	{
		public StreamValues()
		{ _value = 0; _buffer = 0; }

		public StreamValues(int val)
		{ _value = val; _buffer = 0; }

		public StreamValues(int val, int buf)
		{ _value = val; _buffer = buf; }

		private int _value;
		public int Value
		{ 
			get {return _value;}
		}

		private int _buffer;
		public int Buffer
		{
			get { return _buffer; }
			set 
			{
				_buffer = value;
				_hasChanged = true;
			}
		}

		private bool _hasChanged;
		public bool HasChanged
		{
			get { return _hasChanged; }
		}

		public new string ToString()
		{
			string msg = "v:" + _value + " / b:" + _buffer;
			return msg;
		}

		public void TransmitBuffer()
		{
			_value += _buffer;
			_buffer = 0;
			_hasChanged = false;
		}

		public void Generate( int val )
		{	_value += val;	}

		public void Erode( int val )
		{ 	_value -= val;	}

		public void Reset()
		{
			_value = 0;
			_buffer = 0;
		}

		public void Trigger(bool state)
		{	_value = state ? 1 : 0;	}
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
				if(pair.Value.Value > 0)
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

	public bool HasChanged
	{
		get
		{
			foreach(KeyValuePair<Source.SourceType, StreamValues> pair in _state)
			{
				if(pair.Value.HasChanged)
					return true;
			}
			return false;
		}
	}
}
