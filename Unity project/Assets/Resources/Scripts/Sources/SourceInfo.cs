using UnityEngine;
using System.Collections;

public class SourceInfo : MonoBehaviour {

	public Source.SourceType type;
	public int generate;
	public int duration;
	public Mesh mesh;
	public Material mat;

	// Use this for initialization
	void Start ()
	{
		SourceFactory.RegisterSourceInfo(this);
	}
	
	// Update is called once per frame
	void Update (){}
}
