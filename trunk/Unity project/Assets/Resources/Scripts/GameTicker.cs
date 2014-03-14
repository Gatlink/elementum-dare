using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameTicker
{
	private static int _phase = 0;
	private static int _nextTotemIdx = 0;
	private static int _nextMonsterIdx = 0;
	private static List<PhaseEventListener> _phaseListeners = new List<PhaseEventListener>();

	public static bool GameEnded
	{
		get { return Unit.Monsters.Count() == 0 || Unit.Totems.Count() == 0; }
	}

	public static void RegisterListener(PhaseEventListener listener)
	{
		if(!_phaseListeners.Contains(listener))
			_phaseListeners.Add(listener);
	}

	public static void UnregisterListener(PhaseEventListener listener)
	{
		if (_phaseListeners.Contains(listener))
			_phaseListeners.Remove(listener);
	}

	public static void EndPhase()
	{
		// Actually removes the falgged sources
		SourceManager.Instance().RemoveDeadSources();

		// Update sources
		SourceManager.Instance().UpdateSources();

		// Update streams
		StreamManager.Instance().UpdateStreams();
		foreach(PhaseEventListener l in _phaseListeners)
		{
			l.onEndPhase(_phase);
		}
	}

	public static void StartNewPhase()
	{
		// Remove 'dead' sources
		SourceManager.Instance().FlagDeadSources();
		foreach(PhaseEventListener l in _phaseListeners)
		{
			l.onStartNewPhase(_phase);
		}

		// Remove dead bodies
		Unit.CleanDeadUnits();

		Selector.Selected = GetNextUnit();
	}

	private static Unit GetNextUnit()
	{
		Unit unit;
		if (Selector.Selected == null || Selector.Selected.Team == Unit.ETeam.Monster)
		{
			_nextTotemIdx %= Unit.Totems.Count();
			unit = Unit.Totems.ToArray()[_nextTotemIdx];
			_nextTotemIdx += 1;
		}
		else
		{
			_nextMonsterIdx %= Unit.Monsters.Count();
			unit = Unit.Monsters.ToArray ()[_nextMonsterIdx];
			_nextMonsterIdx += 1;
		}

		return unit;
	}
}

public interface PhaseEventListener
{
	void onEndPhase(int phase);
	void onStartNewPhase(int phase);
}