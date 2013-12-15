using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public enum Teams
	{
		Totem,
		Monster
	}

	public Teams Team = Teams.Totem;
	public int Damages = 0;
	public int Moves = 2;
	public Bloc CurrentBloc {get; set;}

	void Start ()
	{
		// Change default outline color to red if team Monster.
		// Monsters face left at start, Totems right.
		if (Team == Teams.Monster)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.rotation.SetFromToRotation(transform.forward, Vector3.left);
		}
		else
			transform.rotation.SetFromToRotation(transform.forward, Vector3.right);

	}

	void Update ()
	{
	
	}
}
