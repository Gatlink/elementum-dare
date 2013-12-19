using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SourceFactory
{
	private static int sourceID = 0;

	private static Dictionary<Source.SourceType, SourceInfo> sourceInfoByType = new Dictionary<Source.SourceType, SourceInfo>();
	private static Dictionary<Source.SourceType, string> scriptTypenameByElementType = CreateScriptTypenamesDictionnary();

	private static GameObject sourceNode = new GameObject("Sources") ;
	// Commodity, to assemble all sources under an object's hierarchy node in the editor

	public static Source CreateSource(Source.SourceType type)
	{
		SourceInfo source = sourceInfoByType[type];

		if(!source)
		{
			Debug.LogError("Creating a source of an unknown type. [" + type.ToString() + "]");
			return null;
		}

		GameObject sourceObj = CreateObjectFromSourceInfo(source);
		sourceObj.tag = "Source";
		sourceObj.layer = LayerMask.NameToLayer("Sources");
		sourceObj.transform.parent = sourceNode.transform;

		if(!sourceObj)
		{
			Debug.LogError("Error creating the source. [" + type.ToString() + "]");
			return null;
		}

		return sourceObj.GetComponent<Source>();
	}

	public static void RegisterSourceInfo(SourceInfo info)
	{
		sourceInfoByType.Add(info.type, info);
	}

	private static GameObject CreateObjectFromSourceInfo(SourceInfo source)
	{
		GameObject obj = new GameObject(" Source #" + (sourceID++) + " ("+source.type.ToString()+")");

		//Need a mesh filter and a mesh renderer for the source's mesh rendering
		MeshFilter filter = obj.AddComponent("MeshFilter") as MeshFilter;
		filter.mesh = Object.Instantiate(source.mesh) as Mesh;

		MeshRenderer renderer = obj.AddComponent("MeshRenderer") as MeshRenderer;
		renderer.material = Object.Instantiate(source.material) as Material;

		//Add a box collider
		MeshCollider hitBox = obj.AddComponent("MeshCollider") as MeshCollider;
		hitBox.transform.parent = obj.transform;

		//Add proper source script
		Source script = obj.AddComponent(scriptTypenameByElementType[source.type]) as Source;
		script.Initialize(source);

		return obj;
	}

	private static Dictionary<Source.SourceType, string> CreateScriptTypenamesDictionnary()
	{
		Dictionary<Source.SourceType, string> tmpDictionnary = new Dictionary<Source.SourceType, string>();

		tmpDictionnary.Add(Source.SourceType.Sand, "SandSource");
		tmpDictionnary.Add(Source.SourceType.Lava, "LavaSource");
		tmpDictionnary.Add(Source.SourceType.Water, "WaterSource");
		tmpDictionnary.Add(Source.SourceType.Wind, "WindSource");
		tmpDictionnary.Add(Source.SourceType.Electricity, "ElectricitySource");

		return tmpDictionnary;
	}

	public static bool IsReady()
	{
		return sourceInfoByType.Count >= Source.NB_OF_TYPES;
	}
}

