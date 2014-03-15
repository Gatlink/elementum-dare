using UnityEngine;
using System.Collections.Generic;

public abstract class Source : MonoBehaviour
{
	public enum SourceType
	{
		Sand,
		Lava,
		Electricity,
		Wind,
		Water
	}
	public static int NB_OF_TYPES = (int)SourceType.Water;
	// must be last of enum

	public SourceType _type;

	protected int _generate;
	protected int _duration;

	private const float TOTAL_ANIM_TIME = 1.5f;
	private float _animTime = -1.0f;

	protected Bloc _bloc;
	public Bloc Bloc
	{
		get { return _bloc; }
		set 
		{
			if (_bloc != null)
				_bloc.ReceiveSource(null);

			if (value != null)
			{
				BlocIndex sourceIndex = value.indexInMap;
				sourceIndex.z += 1;
				transform.position = Map.IndexToPosition(sourceIndex);
				value.ReceiveSource(this);
				_bloc = value;
			}
			else
				_bloc = null;
		}
	}

	public void Initialize(SourceInfo source)
	{
		_type = source.type;

		_generate = source.generate;
		_duration = source.duration;

		_animTime = -1.0f;
	}

	public abstract void RunSource();
	public abstract void KillSource();

	public bool UpdateSourceState()
	{
		--_duration;

		return _duration <= 0;
	}

	public static SourceType GetRandomSourceType()
	{
		SourceType begin = Source.SourceType.Sand;
		SourceType end = Source.SourceType.Water;

		float startRange = (int)begin - 0.49f;
		float endRange = (int)end + 0.49f;

		int rand = (int)System.Math.Round(Random.Range(startRange, endRange));

		return (SourceType) rand;
	}

	public new string ToString()
	{
		return gameObject.name;
	}
	
	public void Die()
	{
		KillSource();
		_animTime = TOTAL_ANIM_TIME; //trigger death animation
	}

	void Update()
	{
		if(_animTime <= 0.0f)
			return;

		float dt = Time.deltaTime;
		float trans = (Bloc.GetBlocSize().y / TOTAL_ANIM_TIME) * dt;

		transform.Translate(-Vector3.up * trans);
		_animTime -= dt;

		if(_animTime <= 0.0f)
		{
			Object.DestroyImmediate(gameObject);
		}
	}
}