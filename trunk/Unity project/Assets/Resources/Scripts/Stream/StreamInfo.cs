using UnityEngine;
using System.Collections;

public class StreamInfo : MonoBehaviour {
	
	public Stream.StreamType type;
	public Mesh mesh;
	public Material material;

	public float granularity;
	public int treshold;
	public float flatFactor;
	public float slopeFactor;
	public int erosion;

	// Use this for initialization
	void Awake ()
	{
		StreamFactory.RegisterStreamInfo(this);
	}
}
