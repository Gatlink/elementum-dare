using UnityEngine;
using System.Collections.Generic;

public class StreamFactory 
{
	private static int streamID = 0;

	private static Dictionary<Stream.StreamType, StreamInfo> streamInfoByType = new Dictionary<Stream.StreamType, StreamInfo>();

	private static GameObject streamNode = new GameObject("Streams") ;
	// Commodity, to assemble all streams under an object's hierarchy node in the editor

	public static Stream CreateStream(Stream.StreamType type)
	{
		StreamInfo stream = streamInfoByType[type];
		
		if(!stream)
		{
			Debug.LogError("Creating a stream of an unknown type. [" + type.ToString() + "]");
			return null;
		}

		GameObject streamObj = CreateObjectFromStreamInfo(stream);
		streamObj.tag = "Stream";
		streamObj.layer = LayerMask.NameToLayer("Streams");
		streamObj.transform.parent = streamNode.transform;
		
		if(!streamObj)
		{
			Debug.LogError("Error creating the stream. [" + type.ToString() + "]");
			return null;
		}
		
		return streamObj.GetComponent<Stream>();
	}

	public static void RegisterStreamInfo(StreamInfo info)
	{
		streamInfoByType.Add(info.type, info);
	}

	private static GameObject CreateObjectFromStreamInfo(StreamInfo stream)
	{
		GameObject streamObj = new GameObject("Stream #" + streamID++);
		
		//Need a mesh filter and a mesh renderer for the stream's mesh rendering
		MeshFilter filter = streamObj.AddComponent("MeshFilter") as MeshFilter;
		filter.mesh = Object.Instantiate(stream.mesh) as Mesh;
		
		MeshRenderer renderer = streamObj.AddComponent("MeshRenderer") as MeshRenderer;
		renderer.material = Object.Instantiate(stream.material) as Material;
		
		//Add a box collider
		MeshCollider hitBox = streamObj.AddComponent("MeshCollider") as MeshCollider;
		hitBox.transform.parent = streamObj.transform;
		
		//Add proper stream script
		string streamScript = "FluidStream";
		if(stream.type == Stream.StreamType.Electricity)
			streamScript = "ElectricityStream";
		else if (stream.type == Stream.StreamType.Wind)
			streamScript = "WindStream";

		Stream script = streamObj.AddComponent(streamScript) as Stream;
		script.Initialize(stream);
		
		return streamObj;
	}

	public static bool IsReady()
	{
		return streamInfoByType.Count >= Stream.NB_OF_TYPES;
	}
}
