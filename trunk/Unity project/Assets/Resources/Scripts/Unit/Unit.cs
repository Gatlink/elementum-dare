using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	public enum ETeam
	{
		Totem,
		Monster
	}

	public static List<Unit> Units = new List<Unit>();

	public ETeam Team = ETeam.Totem;
	public int Hitpoints= 2;
	public int Moves = 2;
	public Source.SourceType SourceType;
	public Bloc.BlocType BlocType;

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
		BlocIndex positionIndex = bloc.indexInMap;
		positionIndex.z += 1;
		transform.position = Map.IndexToPosition(positionIndex);
		CurrentBloc = bloc;
	}

	public void FaceYourOpponent()
	{
		if (Team == ETeam.Monster)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.Rotate(transform.up, 180);
		}
	}

	public void CreateSource(Bloc target)
	{
		SourceManager.Instance().SpawnSource(SourceType);
	}
}
