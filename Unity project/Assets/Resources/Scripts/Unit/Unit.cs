using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour, PhaseEventListener
{
	// All spawned units
	public static List<Unit> Units = new List<Unit>();

#region  Teams

	public enum ETeam
	{
		Totem,
		Monster
	}

#endregion

#region Max Attributes

	public const int HitPointsMax= 2;
	public const int MovesMax = 2;

#endregion

#region Attributes

	public ETeam Team = ETeam.Totem;
	public Source.SourceType SourceType;
	public Bloc.BlocType BlocType;

	public int _hitPoints = HitPointsMax;
	private int _moves;

#endregion

#region Movement

	#region variables

	public float MoveSpeed = 150;
	public float RotateSpeed = 400;

	private Quaternion _rotateTo;
	private Vector3 _relOriginForwardRay;
	private Dictionary<Bloc, Bloc> _paths = new Dictionary<Bloc, Bloc>();
	private List<Bloc> _currentPath = new List<Bloc>();
	private IEnumerable<Bloc> _accessibleBlocs;

	#endregion

	#region Properties

	public Bloc Target
	{
		get
		{
			return _currentPath.FirstOrDefault();
		}
		
		set
		{
			if (!value || _moves == 0 || (_accessibleBlocs != null && !_accessibleBlocs.Contains(value)))
			{
				_currentPath.Clear();
				return;
			}

			Bloc bloc = value;
			while (bloc != _bloc)
			{
				_currentPath.Insert(0, bloc);
				bloc = _paths[bloc];
			}
			ResetMovement();
		}
	}
	
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

	#endregion

	#region Methods

	public void FaceYourOpponent()
	{
		if (Team == ETeam.Monster)
		{
			GetComponent<Selectable>().OutlineColor = Color.red;
			transform.Rotate(transform.up, 180);
		}
	}

	public void MoveToBloc(Bloc bloc)
	{
		BlocIndex positionIndex = bloc.indexInMap;
		positionIndex.z += 1;
		transform.position = Map.IndexToPosition(positionIndex);
		CurrentBloc = bloc;
	}

	private void Move() {
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
			_moves = _moves - Target.FlatDistance(_bloc);
			MoveToBloc(Target);
			_currentPath.Remove(Target);
			AccessibleBlocs = null;
			ResetMovement();
		}
	}
	
	public void UpdateAccessibleBlocs()
	{
		AccessibleBlocs = GetAccessibleBlocs(_bloc, _moves);
	}

	public void ResetMovement()
	{
		Vector3 moveDir = GetMoveDirection();
		_rotateTo = Quaternion.LookRotation(moveDir);
		
		// Compute the ray to check for collision in front of the unit
		_relOriginForwardRay = collider.bounds.center - transform.position;
		_relOriginForwardRay.y = _relOriginForwardRay.y - collider.bounds.size.y/2 - 4.9f;
	}
	
	private Vector3 GetMoveDirection()
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
			
			_paths[blocs[i]] = start;
			
			IEnumerable<Bloc> directNeighbours = GetAccessibleBlocs(blocs[i], distance - 1);
			if (directNeighbours != null)
				neighbours.AddRange(directNeighbours);
		}
		
		neighbours.ForEach(delegate (Bloc bloc) {
			if (!blocs.Contains(bloc))
				blocs.Add(bloc);
		});
		
		return new HashSet<Bloc>(blocs);
	}

	#endregion

#endregion

#region Monobehaviour

	void Update ()
	{
		if (Selector.Selected == collider)
		{
			if (_accessibleBlocs == null)
				UpdateAccessibleBlocs();
		}

		if (Target != null)
			Move();
	}

#endregion

#region Source/Bloc

	public Source CreateSource()
	{
		return SourceManager.Instance().SpawnSource(SourceType);
	}

	public Bloc CreateBloc()
	{
		return BlocFactory.CreateBloc(BlocType);
	}

#endregion

#region PhaseEventListener

	public void onEndPhase(int phase)
	{
		AccessibleBlocs = null;
		Target = null;
	}

	public void onStartNewPhase(int phase)
	{
		_moves = MovesMax;

//		if (_bloc.IsUnderLava || _bloc.IsElectrified || _bloc.Type == Bloc.BlocType.UpgradedPlant)
//			_hitPoints -= 1;

//		if (_hitPoints <= 0)
//		{
//			Units.Remove(this);
//			_bloc.WelcomeUnit(null);
//			GameObject.Destroy(this);
//		}

		// if (_bloc.HasWindBlowing)
	}

#endregion

}
