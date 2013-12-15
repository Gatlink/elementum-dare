using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public enum Teams
	{
		Totem,
		Monstre
	}

	public Teams Team = Teams.Totem;
	public int Damages = 0;
	public int Moves = 2;
	public Bloc CurrentBloc {get; set;}

	void Start ()
	{
		// Change default outline color to red if team Monster.
		// Monsters face left at start, Totems right.
		if (Team == Teams.Monstre)
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

	public void MoveToBloc(Bloc bloc)
	{
		Vector3 position = bloc.transform.position;
		position.y += bloc.collider.bounds.size.y;
		transform.position = position;
		CurrentBloc = bloc;
	}
}
