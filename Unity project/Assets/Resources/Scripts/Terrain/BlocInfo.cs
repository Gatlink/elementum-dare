using UnityEngine;
using System.Collections;

public class BlocInfo : MonoBehaviour {
	
	public Bloc.BlocType type;
	public Mesh mesh;
	public Material material;
	
	// Use this for initialization
	void Awake ()
	{
		BlocFactory.Instance.RegisterBlocInfo(this);
	}
}

