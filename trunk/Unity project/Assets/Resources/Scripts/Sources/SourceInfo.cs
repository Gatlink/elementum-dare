using UnityEngine;
using System.Collections;

public class SourceInfo : MonoBehaviour {

	public Source.SourceType type;
	public ushort generate;
	public ushort duration;
	public Mesh mesh;
	public Material material;

	// Use this for initialization
	void Awake ()
	{
		SourceFactory.RegisterSourceInfo(this);
	}
}
