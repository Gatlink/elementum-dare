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
					Debug.Log("Resolving " + bloc + "type: " + pass.Key);
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
			return "Pass #" + _id + " (" + Count + ")";
		}
	}

	public Stream CreateStream(Source.SourceType type, Bloc onBloc)
	{
		Stream stream = StreamFactory.CreateStream(type);

		if(!(type == Source.SourceType.Wind || type == Source.SourceType.Electricity))
			RegisterElement(stream);

		stream.Bloc = onBloc;

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
/*
		Stream[] orderedStreams = _items.ToArray();

		System.Array.Sort(orderedStreams, new StreamAltitudeComparer());

		int count = 1;
		int mark = 0;
		int currentHeight = orderedStreams[0].GetAltitude();
		for(int i = 1; i < orderedStreams.Length; ++i)
		{
			if(orderedStreams[i].GetAltitude() != currentHeight)
			{
				System.Array.Sort<Stream>(orderedStreams, mark, count, new StreamVolumeComparer());

				mark = i;
				count = 1;
				currentHeight = orderedStreams[i].GetAltitude();
			}
			else
			{
				count += 1;
			}
		}

		System.Array.Sort<Stream>(orderedStreams, mark, count, new StreamVolumeComparer());

		foreach(Stream s in orderedStreams)
		{
			s.UpdateStream();
		}
*/
		//Simulate streams (all values go into buffers)
		foreach(Stream s in _items)
		{
			s.UpdateStream();
		}

		Debug.Log("STARTING RESOLUTION");

		Debug.Log("Fetching eroding streams");
		//Retrieve and store all the blocs of streams that haven't changed this pass
		List<Bloc> toErode = GetStreamBlocs( (stream) => !stream.Bloc.HasPendingStreamChanges() );

		//Start stream animations and buffer validation
		StreamSimulationPass currentPass = null;
		StreamSimulationPass nextPass = null;
		bool continueSimulation = true;

		Debug.Log("Resolving streams around sources");

		while(continueSimulation)
		{
			continueSimulation = FetchNextPass(ref currentPass, out nextPass);
			Debug.Log ("Resolving " + currentPass.ToString());
			currentPass.Resolve();
			Debug.Log ("Next pass is " + nextPass.ToString());
			currentPass = nextPass;
			nextPass = null;
		}

		Debug.Log("Fetching streams without sources");
		//Retrieve all the blocs of streams that have changed this pass but haven't been resolved yet (no source around)
		List<Bloc> noSourceStreams = GetStreamBlocs( (stream) => stream.Bloc.HasPendingStreamChanges() );
		//Sort them by buffer order (the one that transmitted the most is first)
		noSourceStreams.Sort(); //TODO

		Debug.Log("Resolving streams without sources");
		//TODO

		Debug.Log("Resolving streams erosion");
		//TODO

		Debug.Log("END OF RESOLUTION");
		/*foreach(Stream s in _items)
		{
			s.CommitStreamChange();
		}*/
	}

	private bool FetchNextPass(ref StreamSimulationPass currentPass, out StreamSimulationPass nextPass)
	{
		if(currentPass == null) //fetching the first pass
		{
			currentPass = new StreamSimulationPass(null);
			List<Bloc> firstPassBlocs = SourceManager.Instance().GetSourceBlocs();
			foreach(Bloc bloc in firstPassBlocs)
			{
				currentPass.Add(bloc.Source.Type, bloc);
			}
			Debug.Log("Deduced first pass: " + currentPass.ToString());
		}

		nextPass = new StreamSimulationPass(currentPass);

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
		Debug.Log("Fetched " + nextPass.ToString());

		return nextPass.Count > 0;
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
	private static StreamManager _instance = new StreamManager();
	
	public static StreamManager Instance()
	{
		return _instance;
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

	private class StreamVolumeComparer : IComparer<Stream>
	{
		public int Compare(Stream left, Stream right)
		{
			int leftAmount = left.GetVolume();
			int rightAmount = right.GetVolume();
			
			if(leftAmount < rightAmount)
				return 1;
			else if(leftAmount > rightAmount)
				return -1;
			else 
				return 0;
		}
	}

	private class StreamBufferComparer : IComparer<Stream>
	{
		public int Compare(Stream left, Stream right)
		{
			int leftAmount = left.GetVolume();
			int rightAmount = right.GetVolume();
			
			if(leftAmount < rightAmount)
				return 1;
			else if(leftAmount > rightAmount)
				return -1;
			else 
				return 0;
		}
	}
}
