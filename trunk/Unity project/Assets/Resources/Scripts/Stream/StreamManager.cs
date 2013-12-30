using UnityEngine;
using System.Collections.Generic;

public class StreamManager : IManager<Stream>
{
	public Stream CreateStream(Stream.StreamType type, Bloc onBloc)
	{
		Stream stream = StreamFactory.CreateStream(type);
			
		RegisterElement(stream);

		stream.PlaceOnBloc(onBloc);

		return stream;
	}

	public void RemoveStream(ref Stream stream)
	{
		UnregisterElement(stream);

		Object.DestroyImmediate(stream.gameObject); //TODO change for a smoother transition
	}
	
	public void UpdateStreams()
	{
		if(_items.Count <= 0)
			return;

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
			s.Flow();
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
}
