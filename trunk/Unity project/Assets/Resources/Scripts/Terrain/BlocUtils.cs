using UnityEngine;
using System.Collections.Generic;

public class BlocUtils 
{
	public class StreamVolumeComparer : Comparer<Bloc>
	{
		private Source.SourceType _checkType;
		private bool _desc;
		
		public StreamVolumeComparer(Source.SourceType type, bool desc = false)
		{ 
			_checkType = type;
			_desc = desc;
		}
		
		public override int Compare(Bloc left, Bloc right)
		{
			if(_desc)
				return CompareDesc(left, right);
			else
				return CompareAsc(left, right);
		}
		
		public int CompareAsc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType].GetVolume() < right.Streams[_checkType].GetVolume())
				return 1; //right goes after left
			else if (left.Streams[_checkType].GetVolume() > right.Streams[_checkType].GetVolume())
				return -1; //left goes after right
			else
				return 0; //equal
		}
		
		public int CompareDesc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType].GetVolume() > right.Streams[_checkType].GetVolume())
				return 1; //left goes after right
			else if (left.Streams[_checkType].GetVolume() < right.Streams[_checkType].GetVolume())
				return -1; //right goes after left
			else
				return 0; //equal
		}
	}

	public class StreamBufferComparer : Comparer<Bloc>
	{
		private Source.SourceType _checkType;
		private bool _desc;
		
		public StreamBufferComparer(Source.SourceType type, bool desc)
		{ 
			_checkType = type;
			_desc = desc;
		}
		
		public override int Compare(Bloc left, Bloc right)
		{
			if(_desc)
				return CompareDesc(left, right);
			else
				return CompareAsc(left, right);
		}
		
		public int CompareAsc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType].Buffer < right.Streams[_checkType].Buffer)
				return 1; //right goes after left
			else if (left.Streams[_checkType].Buffer > right.Streams[_checkType].Buffer)
				return -1; //left goes after right
			else
				return 0; //equal
		}
		
		public int CompareDesc(Bloc left, Bloc right)
		{
			if(left.Streams[_checkType].Buffer > right.Streams[_checkType].Buffer)
				return 1; //left goes after right
			else if (left.Streams[_checkType].Buffer < right.Streams[_checkType].Buffer)
				return -1; //right goes after left
			else
				return 0; //equal
		}
	}

	public class MasterFluidStreamBufferComparer : Comparer<Bloc>
	{
		private bool _desc;
		
		public MasterFluidStreamBufferComparer(bool desc = false)
		{ 
			_desc = desc;
		}
		
		public override int Compare(Bloc left, Bloc right)
		{
			if(_desc)
				return CompareDesc(left, right);
			else
				return CompareAsc(left, right);
		}
		
		public int CompareAsc(Bloc left, Bloc right)
		{
			FluidStream leftStream = left.Streams.CurrentFluidStream;
			FluidStream rightStream = right.Streams.CurrentFluidStream;

			if(leftStream == null && rightStream == null)
				return 0;
			else
			{
				if(leftStream == null)
					return -1;
				else if(rightStream == null)
					return 1;
				else
				{
					if(leftStream.Buffer < rightStream.Buffer)
						return 1; //right goes after left
					else if (leftStream.Buffer > rightStream.Buffer)
						return -1; //left goes after right
					else
						return 0; //equal
				}
			}
		}
		
		public int CompareDesc(Bloc left, Bloc right)
		{
			FluidStream leftStream = left.Streams.CurrentFluidStream;
			FluidStream rightStream = right.Streams.CurrentFluidStream;
			
			if(leftStream == null && rightStream == null)
				return 0;
			else
			{
				if(leftStream == null)
					return -1;
				else if(rightStream == null)
					return 1;
				else
				{
					if(leftStream.Buffer < rightStream.Buffer)
						return -1; //left goes after right
					else if (leftStream.Buffer > rightStream.Buffer)
						return 1; //right goes after left
					else
						return 0; //equal
				}
			}
		}
	}
}
