using UnityEngine;
using System.Collections;

public class StreamInfo : MonoBehaviour {
	
	public Stream.StreamType type;
	public Mesh mesh;
	public Material material;
	
	// Use this for initialization
	void Awake ()
	{
		StreamFactory.RegisterStreamInfo(this);
	}
}
