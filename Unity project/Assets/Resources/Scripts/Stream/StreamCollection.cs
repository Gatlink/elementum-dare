using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StreamCollection : Dictionary<Source.SourceType, Stream> 
{
	Bloc _bloc; //bloc on which the collection is

	///////////////////////////////////////////////////// WATER
	public Stream Water
	{
		get{ return this[Source.SourceType.Water]; }
		set{ this[Source.SourceType.Water] = value; }
	}
	
	///////////////////////////////////////////////////// SAND
	public Stream Sand
	{
		get{ return this[Source.SourceType.Sand]; }
		set{ this[Source.SourceType.Sand] = value; }
	}
	
	///////////////////////////////////////////////////// LAVA
	public Stream Lava
	{
		get{ return this[Source.SourceType.Lava]; }
		set{ this[Source.SourceType.Lava] = value; }
	}
	
	///////////////////////////////////////////////////// ELECTRICITY
	public Stream Electricity
	{
		get{ return this[Source.SourceType.Electricity]; }
		set{ this[Source.SourceType.Electricity] = value; }
	}

	///////////////////////////////////////////////////// WIND
	public Stream Wind
	{
		get{ return this[Source.SourceType.Wind]; }
		set{ this[Source.SourceType.Wind] = value; }
	}
	
	///////////////////////////////////////////////////// ACCESSORS & HELPERS
	public List<Source.SourceType> CurrentTypes
	{
		get
		{
			List<Source.SourceType> list = new List<Source.SourceType>();
			
			foreach(KeyValuePair<Source.SourceType, Stream> pair in this)
			{
				if(pair.Value.GetVolume() > 0)
				{
					list.Add(pair.Key);
				}
			}
			
			return list;
		}
	}

	public Stream CurrentFluidStream
	{
		get
		{
			KeyValuePair<Source.SourceType, Stream> elem = this.FirstOrDefault( (pair) => pair.Value.IsFluid && pair.Value.GetVolume() > 0 );
			return elem.Value;
		}
	}

	public void Initialize(Bloc parent)
	{
		_bloc = parent;

		Source.SourceType[] tmp = (Source.SourceType[])System.Enum.GetValues(typeof(Source.SourceType));
		foreach(Source.SourceType sourceType in tmp )
		{
			Stream stream = StreamFactory.CreateStream(sourceType, _bloc, false);
			stream.Initialize(_bloc, sourceType);
			this.Add(sourceType, stream);
		}
	}

	public void Resolve(Source.SourceType type, bool animated)
	{
		Stream currentStream = CurrentFluidStream;
		Stream newStream = null;

		Stream interacting;
		if(this.TryGetValue(type, out interacting))
		{
			interacting.TransmitBuffer();

			if(interacting != currentStream)
			{
				if(currentStream != null)
				{
					int lostInInteraction = Mathf.Min(interacting.Value, currentStream.Value);
					interacting.LostInInteraction(lostInInteraction);
					currentStream.LostInInteraction(lostInInteraction);
				}

				newStream = (interacting.Value > 0) ? interacting : currentStream;

				if(newStream != null && newStream.Value <= 0)
					newStream = null;
			}
		}
		else
		{
			newStream = currentStream;
		}

		bool masterStreamHasChanged = (currentStream != newStream);

		if(masterStreamHasChanged)
		{
			if(currentStream != null)
			{
				//dryout
				StreamManager.Instance.UnregisterElement(currentStream);
				currentStream.renderer.enabled = false;
			}

			if(newStream != null)
			{
				newStream.renderer.enabled = true;
				//fill up
				StreamManager.Instance.RegisterElement(newStream);
			}
		}
		else
		{
			if(currentStream != null)
			{
				currentStream.UpdateStreamVisual();
			}
		}
	}
	
	public bool HasChanged
	{
		get
		{
			foreach(KeyValuePair<Source.SourceType, Stream> pair in this)
			{
				if(pair.Value.HasChanged)
					return true;
			}
			return false;
		}
	}

	public new string ToString()
	{
		string str = "";

		foreach(KeyValuePair<Source.SourceType, Stream> pair in this)
		{
			str += pair.Value.ToString() + "\n";
		}

		return str;
	}

	public void ClearStreams()
	{
		foreach(KeyValuePair<Source.SourceType, Stream> pair in this)
		{
			if(pair.Key != Source.SourceType.Electricity)
				pair.Value.Clear();
		}

		if(Electricity.GetVolume() > 0 && !_bloc.IsConductor())
			Electricity.Clear();
	}
}
