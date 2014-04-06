using UnityEngine;
using System.Collections.Generic;

public class StreamUtils 
{
	public class StreamVolumeComparer : Comparer<Stream>
	{
		private bool _desc;

		public StreamVolumeComparer(bool desc = false)
		{ 
			_desc = desc;
		}

		public override int Compare(Stream left, Stream right)
		{
			if(_desc)
				return CompareDesc(left, right);
			else
				return CompareAsc(left, right);
		}

		public int CompareAsc(Stream left, Stream right)
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

		public int CompareDesc(Stream left, Stream right)
		{
			int leftAmount = left.GetVolume();
			int rightAmount = right.GetVolume();
			
			if(leftAmount < rightAmount)
				return -1;
			else if(leftAmount > rightAmount)
				return 1;
			else 
				return 0;
		}
	}
	
	public class StreamBufferComparer : Comparer<Stream>
	{
		private bool _desc;
		
		public StreamBufferComparer(bool desc = false)
		{ 
			_desc = desc;
		}

		public override int Compare(Stream left, Stream right)
		{
			if(_desc)
				return CompareDesc(left, right);
			else
				return CompareAsc(left, right);
		}

		public int CompareAsc(Stream left, Stream right)
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

		public int CompareDesc(Stream left, Stream right)
		{
			int leftAmount = left.Buffer;
			int rightAmount = right.Buffer;
			
			if(leftAmount < rightAmount)
				return -1;
			else if(leftAmount > rightAmount)
				return 1;
			else 
				return 0;
		}
	}
}
