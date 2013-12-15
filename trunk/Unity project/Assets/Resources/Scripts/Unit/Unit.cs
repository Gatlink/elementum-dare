using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public enum Teams
	{
		Totem,
		Monstre
	}

	public Teams Team = Teams.Totem;
	public int Hitpoints= 2;
	public int Moves = 2;
	public Bloc CurrentBloc {get; set;}

	void Start ()
	{

	}

	void Update ()
	{
		Debug.DrawRay(transform.position, transform.forward * 10f);
	}

	public void MoveToBloc(Bloc bloc)
	{
		Vector3 position = bloc.transform.position;
		position.y += bloc.collider.bounds.size.y;
		transform.position = position;
		CurrentBloc = bloc;
		bloc.WelcomeUnit(this);
	}

	public void FaceYourOpponent()
	{
		if (Team == Teams.Monstre)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.Rotate(transform.up, 180);
		}
	}
}
