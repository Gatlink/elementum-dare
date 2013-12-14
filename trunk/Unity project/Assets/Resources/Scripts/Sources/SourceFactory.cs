using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SourceFactory
{
	private static int sourceID = 0;

	private static Dictionary<Source.SourceType, SourceInfo> sourceInfoByType = new Dictionary<Source.SourceType, SourceInfo>();
	private static Dictionary<Source.SourceType, string> scriptTypenameByElementType = CreateScriptTypenamesDisctionnary();


	public static GameObject CreateSource(Source.SourceType type, int x = 0, int y = 0, int z = 0)
	{
		SourceInfo source = sourceInfoByType[type];

		if(!source)
		{
			Debug.LogError("Creating a source of an unknown type. [" + type.ToString() + "]");
			return null;
		}

		Vector3 pos = Map.IndexToPosition(x,y,z);

		//TODO implement with type and all
		GameObject sourceObj = CreateObjectFromSourceInfo(source, pos);

		if(!sourceObj)
		{
			Debug.LogError("Error creating the source. [" + type.ToString() + "]");
			return null;
		}

		return sourceObj;
	}

	public static void RegisterSourceInfo(SourceInfo info)
	{
		sourceInfoByType.Add(info.type, info);
	}

	private static GameObject CreateObjectFromSourceInfo(SourceInfo source, Vector3 pos)
	{
		GameObject obj = new GameObject(source.type.ToString() + " Source #" + (++sourceID));
		obj.transform.position = pos;
		obj.transform.rotation = Quaternion.identity;

		//Need a mesh filter and a mesh renderer for the source's mesh rendering
		MeshFilter filter = obj.AddComponent("MeshFilter") as MeshFilter;
		filter.mesh = Object.Instantiate(source.mesh) as Mesh;

		/*MeshRenderer renderer = */obj.AddComponent("MeshRenderer")/* as MeshRenderer*/;

		//Add a box collider
		BoxCollider hitBox = obj.AddComponent("BoxCollider") as BoxCollider;
		hitBox.transform.parent = obj.transform;
		hitBox.transform.position = obj.transform.position;

		//Add proper source script
		Source script = obj.AddComponent(scriptTypenameByElementType[source.type]) as Source;
		script.Initialize(source);

		return obj;
	}

	private static Dictionary<Source.SourceType, string> CreateScriptTypenamesDisctionnary()
	{
		Dictionary<Source.SourceType, string> tmpDictionnary = new Dictionary<Source.SourceType, string>();

		tmpDictionnary.Add (Source.SourceType.Sand, "SandSource");
		tmpDictionnary.Add (Source.SourceType.Lava, "LavaSource");
		tmpDictionnary.Add (Source.SourceType.Water, "WaterSource");
		tmpDictionnary.Add (Source.SourceType.Wind, "WindSource");
		tmpDictionnary.Add (Source.SourceType.Electricity, "ElectricitySource");

		return tmpDictionnary;
	}
}

