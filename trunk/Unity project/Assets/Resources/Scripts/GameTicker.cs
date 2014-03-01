using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameTicker
{
	private static int _phase = 0;
	private static int _nextUnitIdx = 0;
	private static List<PhaseEventListener> _phaseListeners = new List<PhaseEventListener>();

	public static void RegisterListener(PhaseEventListener listener)
	{
		if(!_phaseListeners.Contains(listener))
			_phaseListeners.Add(listener);
	}

	public static void EndPhase()
	{
		//Update sources
		SourceManager.Instance().UpdateSources();

		//Update streams
		StreamManager.Instance().UpdateStreams();
		foreach(PhaseEventListener l in _phaseListeners)
		{
			l.onEndPhase(_phase);
		}
	}

	public static void StartNewPhase()
	{
		//Remove 'dead' sources
		SourceManager.Instance().CleanDeadSources();
		foreach(PhaseEventListener l in _phaseListeners)
		{
			l.onStartNewPhase(_phase);
		}
		Selector.Selected = GetNextUnit().collider;
	}

	private static Unit GetNextUnit()
	{
		Unit[] units = Unit.Units.ToArray();

		Unit nextUnit = units[_nextUnitIdx];
		do
		{
			_nextUnitIdx = (_nextUnitIdx + 1) % units.Length;
		} while (nextUnit.Team == units[_nextUnitIdx].Team);
		
		return nextUnit;
	}
}

public interface PhaseEventListener
{
	void onEndPhase(int _phase);
	void onStartNewPhase(int _phase);
}