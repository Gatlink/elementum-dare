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

	private Bloc _bloc;
	public Bloc CurrentBloc
	{
		get { return _bloc; }
		set
		{
			if (_bloc != null)
				_bloc.WelcomeUnit(null);

			_bloc = value;

			if (_bloc != null)
				_bloc.WelcomeUnit(this);
		}
	}

	void Start ()
	{

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

	public void FaceYourOpponent()
	{
		if (Team == Teams.Monstre)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.Rotate(transform.up, 180);
		}
	}
}
