using UnityEngine;
using System.Collections.Generic;

public class SourceManager : IManager<Source>, PhaseEventListener
{
	public void SpawnSource(Source.SourceType type)
	{
		if(!Selector.HasTargetSelected("Bloc"))
			return;
		
		Bloc bloc = Selector.Selected.gameObject.GetComponent<Bloc>();
		
		if(bloc == null || !bloc.IsReachable())
			return;
		
		Source source = SourceFactory.CreateSource(type);
		source.Bloc = bloc;
		
		RegisterElement(source);
	}

	public void onEndPhase(GameTickerEvent e)
	{
		foreach(Source source in _items)
		{
			source.RunSource();
		}
	}
	
	public void onStartNewPhase(GameTickerEvent e)
	{
		foreach(Source source in _items)
		{
			source.UpdateSourceState();
		}
	}
	
	//Singleton
	private static SourceManager _instance = new SourceManager();
	
	public static SourceManager Instance()
	{
		return _instance;
	}
	
	private SourceManager()
	{
		GameTicker.RegisterListener(this);
	}
}
