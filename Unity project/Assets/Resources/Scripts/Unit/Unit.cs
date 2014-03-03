using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour, PhaseEventListener {
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
	public float MoveSpeed = 150;
	public float RotateSpeed = 400;
	
	private Quaternion _rotateTo;
	private Bloc _target = null;
	private Vector3 _relOriginForwardRay;
	private Bloc _bloc;
	private int _currentMoves;

	public Bloc Target
	{
		get
		{
			return _target;
		}
		
		set
		{
			_target = value;
			ResetMovement();
		}
	}

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

	private IEnumerable<Bloc> _accessibleBlocs;
	private IEnumerable<Bloc> AccessibleBlocs
	{
		get { return _accessibleBlocs; }
		set
		{
			if (_accessibleBlocs != null)
			{
				foreach (Bloc bloc in _accessibleBlocs)
					bloc.gameObject.renderer.material.color = Color.white;
			}

			_accessibleBlocs = value;

			if (_accessibleBlocs != null)
			{
				foreach (Bloc bloc in _accessibleBlocs)
					bloc.gameObject.renderer.material.color = Color.red;
			}
		}
	}

	void Start ()
	{
		ResetMovement();
	}

	void Update ()
	{
		if (Selector.Selected == collider)
		{
			if (_accessibleBlocs == null)
				UpdateAccessibleBlocs();
		}

		if (Target != null
		    && _accessibleBlocs != null
		    && _accessibleBlocs.Contains(Target))
			Move();
	}

	public void UpdateAccessibleBlocs()
	{
		AccessibleBlocs = GetAccessibleBlocs(_bloc, _currentMoves);
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

	
	void Move() {
		Vector3 vDest = Target.transform.position;
		vDest.y = transform.position.y;
		
		// Face the destination
		if (transform.rotation != _rotateTo)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTo, RotateSpeed * Time.deltaTime);
		else if (transform.position != vDest)
		{
			// Climb up
			Debug.DrawRay(collider.bounds.center, Vector3.down * (5 + collider.bounds.size.y/2), Color.red);
			Debug.DrawRay(transform.position + _relOriginForwardRay, transform.forward * (vDest - transform.position).magnitude, Color.red);
			if (Physics.Raycast(collider.bounds.center, Vector3.down, 5 + collider.bounds.size.y/2)
			    || Physics.Raycast(transform.position + _relOriginForwardRay, transform.forward, (vDest - transform.position).magnitude))
				transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, MoveSpeed * Time.deltaTime);
			// Move toward target
			else
				transform.position = Vector3.MoveTowards(transform.position, vDest, MoveSpeed * Time.deltaTime);
		}
		// Climb down
		else if (!Physics.Raycast(collider.bounds.center, Vector3.down, 0.1f + collider.bounds.size.y/2.0f))
			transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, MoveSpeed * Time.deltaTime);
		// End of the road
		else
		{
			_currentMoves = _currentMoves - Target.FlatDistance(_bloc);
			MoveToBloc(Target);
			Target = null;
			AccessibleBlocs = null;
		}
	}
	
	public void ResetMovement()
	{
		Vector3 moveDir = getMoveDirection();
		_rotateTo = Quaternion.LookRotation(moveDir);
		
		// Compute the ray to check for collision in front of the unit
		_relOriginForwardRay = collider.bounds.center - transform.position;
		_relOriginForwardRay.y = _relOriginForwardRay.y - collider.bounds.size.y/2 - 4.9f;
	}
	
	private Vector3 getMoveDirection()
	{
		Vector3 direction = (Target ? Target.transform.position : Vector3.zero) - transform.position;
		direction.y = 0;
		return direction.normalized;
	}

	private IEnumerable<Bloc> GetAccessibleBlocs(Bloc start, int distance)
	{
		if (distance <= 0)
			return null;

		List<Bloc> blocs = Map.FetchNeighbors2D(start, 1);
		List<Bloc> neighbours = new List<Bloc>();
		for (int i = 0; i < blocs.Count; ++i)
		{
			if (Math.Abs(blocs[i].indexInMap.z - start.indexInMap.z) > 1
			    || !blocs[i].IsReachable())
			{
				blocs.RemoveAt(i);
				i = i - 1; // compensate for the deletion of an entry
				continue;
			}

			IEnumerable<Bloc> directNeighbours = GetAccessibleBlocs(blocs[i], distance - 1);
			if (directNeighbours != null)
				neighbours.AddRange(directNeighbours);
		}

		blocs.AddRange(neighbours);

		return new HashSet<Bloc>(blocs);
	}

	public void onEndPhase(int phase)
	{
		AccessibleBlocs = null;
		Target = null;
	}

	public void onStartNewPhase(int phase)
	{
		if (_currentMoves < Moves)
			_currentMoves = Moves;
	}
}
