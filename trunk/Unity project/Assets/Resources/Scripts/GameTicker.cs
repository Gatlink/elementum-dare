using UnityEngine;
using System.Collections.Generic;

public struct GameTickerEvent
{
	public GameTickerEvent(int t, int p)
	{
		turn = t;
		phase = p;
	}

	public int turn {get; set;}
	public int phase {get; set;}
}

public class GameTicker
{
	private static int _turn = 0;
	private static int _phase = 0;

	private static List<PhaseEventListener> _phaseListeners = new List<PhaseEventListener>();
	private static List<TurnEventListener> _turnListeners = new List<TurnEventListener>();

	public static void RegisterListener(PhaseEventListener listener)
	{
		if(!_phaseListeners.Contains(listener))
			_phaseListeners.Add(listener);
	}

	public static void RegisterListener(TurnEventListener listener)
	{
		if(!_turnListeners.Contains(listener))
			_turnListeners.Add(listener);
	}

	public static void EndPhase()
	{
		GameTickerEvent gte = new GameTickerEvent(_turn, _phase);

		foreach(PhaseEventListener l in _phaseListeners)
		{
			l.onEndPhase(gte);
		}
	}

	public static void EndTurn()
	{
		GameTickerEvent gte = new GameTickerEvent(_turn, _phase);
		
		foreach(TurnEventListener l in _turnListeners)
		{
			l.onEndTurn(gte);
		}
	}

	public static void StartNewPhase()
	{
		GameTickerEvent gte = new GameTickerEvent(_turn, _phase);
		
		foreach(PhaseEventListener l in _phaseListeners)
		{
			l.onStartNewPhase(gte);
		}
	}

	public static void StartNewTurn()
	{
		GameTickerEvent gte = new GameTickerEvent(_turn, _phase);
		
		foreach(TurnEventListener l in _turnListeners)
		{
			l.onStartNewTurn(gte);
		}
	}
}

public interface PhaseEventListener
{
	void onEndPhase(GameTickerEvent e);
	void onStartNewPhase(GameTickerEvent e);
}

public interface TurnEventListener
{
	void onEndTurn(GameTickerEvent e);
	void onStartNewTurn(GameTickerEvent e);
}