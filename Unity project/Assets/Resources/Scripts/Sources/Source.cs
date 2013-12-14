using UnityEngine;
using System.Collections;

public abstract class Source : MonoBehaviour, PhaseEventListener
{
	public enum SourceType
	{
		Sand,
		Lava,
		Electricity,
		Wind,
		Water
	}

	protected SourceType _type;

	protected int _generate;
	protected int _duration;

	public void Initialize(SourceInfo source)
	{
		_type = source.type;

		_generate = source.generate;
		_duration = source.duration;
	}

	public void onEndPhase(GameTickerEvent e)
	{
		RunSource();
	}

	public void onStartNewPhase(GameTickerEvent e)
	{
		UpdateSourceState();
	}

	protected abstract void RunSource();

	protected void UpdateSourceState()
	{
		--_duration;

		if(_duration == 0)
		{
			//TODO remove source from game
		}
	}
}