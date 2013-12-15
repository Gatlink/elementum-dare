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
		
		BlocIndex sourceIndex = bloc.indexInMap;
		sourceIndex.z += 1;
		
		Vector3 pos = Map.IndexToPosition(sourceIndex);
		
		Source source = SourceFactory.CreateSource(type);
		source.gameObject.transform.position = pos;
		
		source.SetOnBloc(bloc);
		bloc.ReceiveSource(source);
		
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
