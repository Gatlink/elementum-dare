using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StreamManager : IManager<Stream>
{
	private class StreamSimulationPass
		: IEnumerable<KeyValuePair<Source.SourceType, HashSet<Bloc>>>
	{
		private Dictionary<Source.SourceType, HashSet<Bloc>> _blocs;
		private int _id;

		public StreamSimulationPass(StreamSimulationPass previousPass) 
		{
			_id = (previousPass != null) ? previousPass._id + 1 : 0;
			_blocs = new Dictionary<Source.SourceType, HashSet<Bloc>>();
		}

		~StreamSimulationPass() 
		{
			_blocs.Clear();
			_blocs = null;
		}

		public HashSet<Bloc> this[Source.SourceType type]
		{
			get{ return _blocs[type]; }
			set{ _blocs[type].UnionWith(value); }
		}

		public void Add(Source.SourceType type, Bloc bloc)
		{
			if(!_blocs.ContainsKey(type))
				_blocs.Add(type, new HashSet<Bloc>());

			_blocs[type].Add(bloc);
		}

		public void AddRange(Source.SourceType type, List<Bloc> bloc)
		{
			if(!_blocs.ContainsKey(type))
				_blocs.Add(type, new HashSet<Bloc>());
			
			_blocs[type].UnionWith(bloc);
		}

		public void Clear()
		{
			_blocs.Clear();
		}

		public IEnumerator<KeyValuePair<Source.SourceType, HashSet<Bloc>>> GetEnumerator() { return _blocs.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public void Resolve()
		{
			foreach(KeyValuePair<Source.SourceType, HashSet<Bloc>> pass in _blocs)
			{
				foreach(Bloc bloc in pass.Value)
				{
					//Debug.Log("Resolving " + bloc + "type: " + pass.Key);
					bloc.Streams.Resolve(pass.Key, true);
				}
			}
		}

		public int Count
		{
			get 
			{
				int sum = 0;
				foreach(KeyValuePair<Source.SourceType, HashSet<Bloc>> pass in _blocs)
				{
					sum += pass.Value.Count;
				}
				return sum; 
			}
		}

		public new string ToString()
		{
			string str = "Pass #" + _id + " (" + Count + ")\n";
			string sep;
			foreach(KeyValuePair<Source.SourceType, HashSet<Bloc>> pass in _blocs)
			{
				sep = "";
				str += pass.Key + ":";

				foreach(Bloc b in pass.Value)
				{
					str += sep + " " + b.gameObject.name; 
					sep = ",";
				}

				str += "\n";
			}

			return str;
		}
	}

	public Stream CreateStream(Source.SourceType type, Bloc onBloc)
	{
		Stream stream = StreamFactory.CreateStream(type, onBloc);

		if(!(type == Source.SourceType.Wind || type == Source.SourceType.Electricity))
			RegisterElement(stream);

		return stream;
	}

	public void RemoveStream(Stream stream)
	{
		if(!(stream.Type == Source.SourceType.Wind || stream.Type == Source.SourceType.Electricity))
			UnregisterElement(stream);

		Object.Destroy(stream.gameObject); //TODO change for a smoother transition
	}
	
	public void UpdateStreams()
	{
		if(_items.Count <= 0)
			return;

		//Simulate streams (all values go into buffers)
		foreach(Stream s in _items)
		{
			s.UpdateStreamState();
		}

		Debug.Log("STARTING RESOLUTION");

		ResolveStreamsFromSources();
		ResolveStreamsWithoutSources();
		ResolveErodingStreams();

		Debug.Log("END OF RESOLUTION");
	}

	private bool FetchFirstPassFromSources(out StreamSimulationPass firstPass)
	{
		firstPass = new StreamSimulationPass(null);

		//Retrieve all the blocs that hold a source
		List<Bloc> firstPassBlocs = SourceManager.Instance.GetSourceBlocs();
		foreach(Bloc bloc in firstPassBlocs)
		{
			firstPass.Add(bloc.Source.Type, bloc);
		}

		return firstPass.Count > 0;
	}

	private bool FetchFirstPassFromUnresolvedStream(out StreamSimulationPass firstPass)
	{
		firstPass = new StreamSimulationPass(null);

		//Retrieve all the streams that have changed this pass but haven't been resolved yet (no source around)
		List<Stream> noSourceStreams = _items.Where( (stream) => stream.HasChanged && !stream.Resolved );
		//Among those, keep only the streams with a negative buffer. As they transmit to the others, they act like a source
		List<Stream> givingStreams = noSourceStreams.Where( (stream) => stream.Buffer < 0 );
		//Sort them by buffer order (the one that transmitted the most is first)
		givingStreams.Sort(new StreamUtils.StreamBufferComparer());

		foreach(Stream stream in givingStreams)
		{
			firstPass.Add(stream.Type, stream.Bloc);
		}
		
		return firstPass.Count > 0;
	}

	private bool FetchNextPass(ref StreamSimulationPass currentPass, out StreamSimulationPass nextPass)
	{		
		nextPass = new StreamSimulationPass(currentPass);

		if(currentPass == null) //fetching the first pass
		{
			return false;
		}

		foreach(KeyValuePair<Source.SourceType, HashSet<Bloc>> pass in currentPass)
		{
			foreach(Bloc bloc in pass.Value)
			{
				List<Bloc> neighbors = Map.FetchNeighborsIf(bloc, 1, 
			                                            (b) => !b.IsHigherThan(bloc) 
				                                            && b.IsTopMostBloc() 
				                                            && b.HasPendingStreamOfType(pass.Key),
				                                            true, false); //TODO
				nextPass.AddRange(pass.Key, neighbors);
			}
		}

		return nextPass.Count > 0;
	}

	private void ResolveStreamsFromSources()
	{
		Debug.Log("RESOLVING [1]: streams with sources");

		//Start stream animations and buffer validation
		StreamSimulationPass currentPass = null;
		StreamSimulationPass nextPass = null;

		bool continueSimulation = FetchFirstPassFromSources(out currentPass);		
		while(continueSimulation)
		{
			Debug.Log ("Resolving " + currentPass.ToString());
			currentPass.Resolve();
			continueSimulation = FetchNextPass(ref currentPass, out nextPass);
			currentPass = nextPass;
			nextPass = null;
		}

		Debug.Log("END OF RESOLUTION [1]: streams with sources");
	}

	private void ResolveStreamsWithoutSources()
	{
		Debug.Log("RESOLVING [2]: streams without sources");

		StreamSimulationPass currentPass = null;
		StreamSimulationPass nextPass = null;
		
		bool continueSimulation = FetchFirstPassFromUnresolvedStream(out currentPass);		
		while(continueSimulation)
		{
			Debug.Log ("Resolving " + currentPass.ToString());
			currentPass.Resolve();
			continueSimulation = FetchNextPass(ref currentPass, out nextPass);
			currentPass = nextPass;
			nextPass = null;
		}

		Debug.Log("END OF RESOLUTION [2]: streams without sources");
	}

	private void ResolveErodingStreams()
	{
		Debug.Log("RESOLVING [3]: eroding streams");

		//Retrieve all the streams that haven't changed this pass but haven't been resolved yet
		List<Stream> toErode = _items.Where( (stream) => stream.IsFluid && !stream.Resolved && !stream.Bloc.HasPendingStreamChanges() );
		foreach(Stream str in toErode)
		{
			FluidStream fluid = str as FluidStream;
			if(fluid != null)
				fluid.Erode();
		}

		Debug.Log("END OF RESOLUTION [3]: eroding streams");
	}

	private List<Bloc> GetStreamBlocs(System.Func<Stream, bool> functor)
	{
		List<Bloc> blocs = new List<Bloc>();
		foreach(Stream s in _items)
		{
			if( (functor == null) || ((functor != null) && functor(s)) )
				blocs.Add(s.Bloc);
		}
		return blocs;
	}

	//Singleton
	private static StreamManager _instance = null;
	
	public static StreamManager Instance
	{
		get
		{ return (_instance != null) ? _instance : _instance = new StreamManager(); }
	}
	
	private StreamManager()	{}

	//Nested comparers
	private class StreamAltitudeComparer : IComparer<Stream>
	{
		public int Compare(Stream left, Stream right)
		{
			int leftHeight = left.GetAltitude();
			int rightHeight = right.GetAltitude();

			if(leftHeight < rightHeight)
				return 1;
			else if(leftHeight > rightHeight)
				return -1;
			else 
				return 0;
		}
	}
}
