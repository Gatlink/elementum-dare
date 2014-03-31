using UnityEngine;
using System.Collections.Generic;

public class StreamUtils 
{
	public class StreamVolumeComparer : IComparer<Stream>
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
	
	public class StreamBufferComparer : IComparer<Stream>
	{
		public int Compare(Stream left, Stream right)
		{
			int leftAmount = left.Buffer;
			int rightAmount = right.Buffer;
			
			if(leftAmount < rightAmount)
				return 1;
			else if(leftAmount > rightAmount)
				return -1;
			else 
				return 0;
		}
	}
}
