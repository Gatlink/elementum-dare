 using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour
{
	public enum StreamType
	{
		None,
		Sand,
		Lava,
		Water
	}
	public static int NB_OF_TYPES = (int)StreamType.Water;
	// must be last of enum

	public StreamType type { get; set;}
	
	private Bloc _bloc;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
}
