using UnityEngine;
using System.Collections.Generic;

public class StreamFactory 
{
	private static int streamID = 0;

	private static Dictionary<Source.SourceType, StreamInfo> streamInfoByType = new Dictionary<Source.SourceType, StreamInfo>();

	public static Stream CreateStream(Source.SourceType type, Bloc onBloc, bool visible = true)
	{
		StreamInfo stream = streamInfoByType[type];
		
		if(!stream)
		{
			Debug.LogError("Creating a stream of an unknown type. [" + type.ToString() + "]");
			return null;
		}

		GameObject streamObj = CreateObjectFromStreamInfo(stream, visible);
		streamObj.tag = "Stream";
		streamObj.layer = LayerMask.NameToLayer("Streams");
		streamObj.transform.parent = onBloc.gameObject.transform;
		
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

	private static GameObject CreateObjectFromStreamInfo(StreamInfo stream, bool visible)
	{
		GameObject streamObj = new GameObject("Stream #" + streamID++);
		
		//Need a mesh filter and a mesh renderer for the stream's mesh rendering
		MeshFilter filter = streamObj.AddComponent("MeshFilter") as MeshFilter;
		filter.mesh = Object.Instantiate(stream.mesh) as Mesh;
		
		MeshRenderer renderer = streamObj.AddComponent("MeshRenderer") as MeshRenderer;
		renderer.material = Object.Instantiate(stream.material) as Material;
		renderer.enabled = visible;
		
		//Add a box collider
		MeshCollider hitBox = streamObj.AddComponent("MeshCollider") as MeshCollider;
		hitBox.transform.parent = streamObj.transform;
		
		//Add proper stream script
		string streamScript = "FluidStream";
		if(stream.type == Source.SourceType.Electricity)
			streamScript = "ElectricityStream";
		else if (stream.type == Source.SourceType.Wind)
			streamScript = "WindStream";

		streamObj.AddComponent(streamScript);
		
		return streamObj;
	}

	public static bool IsReady()
	{
		return streamInfoByType.Count >= Source.NB_OF_TYPES;
	}

	public static StreamInfo GetStreamSettingsByType(Source.SourceType type)
	{
		return streamInfoByType[type];
	}
}
