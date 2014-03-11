using UnityEngine;
using System.Collections;

public class SourceInfo : MonoBehaviour {

	public Source.SourceType type;
	public int generate;
	public int duration;
	public Mesh mesh;
	public Material material;

	// Use this for initialization
	void Awake ()
	{
		SourceFactory.RegisterSourceInfo(this);
	}
}
