using UnityEngine;
using System.Collections.Generic;

public class SourceManager : IManager<Source>, PhaseEventListener
{
	public Source SpawnSource(Source.SourceType type)
	{
		Source source = SourceFactory.CreateSource(type);
		
		RegisterElement(source);

		return source;
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
