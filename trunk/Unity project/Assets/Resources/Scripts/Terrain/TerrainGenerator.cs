using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class TerrainGenerator : MonoBehaviour 
{
	public string mapFilePath = @"Assets/Resources/HeightMaps/heightMap_1.hmap";
	
	private const int DIMENSIONS_LINE = 0;
	private const int SECTIONS_LINE_JUMP = 1;

	private int[,] _heightMatrix;
	private string[,] _elementsMatrix;
	private int _width;
	private int _length;

	private static Dictionary<string, Bloc.BlocType> _stringToElementType = CreateElementsDictionary();
	private static Dictionary<string, Unit.ETeam> _stringToTeams = CreateTeamsDictionary();

	// Use this for initialization
	void Start () 
	{
		GenerateMatrixes();
		ParameterMap();

		if(!FactoriesReady())
		{
			Debug.Log("Factories not ready. A reference object may be missing or is not registering properly unsing Awake()");
		}

		FillMap();		
	}
	
	// Update is called once per frame
	void Update ()
	{}

	private void GenerateMatrixes()
	{
		if (!File.Exists(mapFilePath))
		{
			Debug.Log ("File does not exist. [" + mapFilePath +"]");
			return;
		}
		
		//Start by reading the whole file
		string[] lines = File.ReadAllLines(mapFilePath);
		
		if(lines.Length == 0)
		{
			Debug.Log ("Heightmap file is empty.");
			return;
		}
		
		//Get the dimensions from the first line
		if(!ReadMapDimensions(lines, ref _width, ref _length))
		{
			Debug.Log ("Error reading heightmap dimensions.");
			return;
		}
		
		_heightMatrix = new int[_width, _length];
		_elementsMatrix = new string[_width, _length];
		
		if(!BuildHeightMatrix(lines, ref _heightMatrix))
		{
			Debug.Log ("Error building the heightmap matrix.");
			return;
		}
		
		if(!BuildElementMatrix(lines, ref _elementsMatrix))
		{
			Debug.Log ("Error building the heightmap matrix.");
			return;
		}
		
		Debug.Log("Terrain matrixes successfully generated.");
	}

	private bool ReadMapDimensions(string[] lines, ref int width, ref int length)
	{
		string dimensions = lines[DIMENSIONS_LINE];

		if(dimensions.Length < 3)
		{
			Debug.Log("Dimension's line is ill formated (empty or not enough parameters).");
			return false;
		}

		string[] result = dimensions.Split (new char[]{','}, 2, System.StringSplitOptions.RemoveEmptyEntries);

		if(result.Length != 2)
		{
			Debug.Log("Dimension's line is ill formated (incorrect number of parameters).");
			return false;
		}

		width = System.Convert.ToInt32(result[0]);
		length = System.Convert.ToInt32(result[1]);

		return true;
	}

	private bool BuildHeightMatrix(string[] lines, ref int[,] matrix)
	{
		//Skip dimensions line and first line jump
		int start = DIMENSIONS_LINE + SECTIONS_LINE_JUMP + 1;
		int end = start + _width;

		if(lines.Length < end)
		{
			Debug.Log("Heightmap dimensions do not match dimensions specified in file.");
			return false;
		}

		for( int lineNb = start;  lineNb < end; ++lineNb)
		{
			string[] result = lines[lineNb].Split (new char[]{' '}, _length, System.StringSplitOptions.RemoveEmptyEntries);

			for( int columnNb = 0; columnNb < _length; ++columnNb)
			{
				matrix[lineNb-start, columnNb] = System.Convert.ToInt32(result[columnNb]);
			}
		}

		return true;
	}

	private bool BuildElementMatrix(string[] lines, ref string[,] matrix)
	{
		//Skip dimensions line, height map and line jumps
		int start = DIMENSIONS_LINE + 2*SECTIONS_LINE_JUMP + _width + 1;
		int end = start + _width;

		if(lines.Length < end)
		{
			Debug.Log("Elementstmap dimensions do not match dimensions specified in file.");
			return false;
		}
		
		for( int lineNb = start;  lineNb < end; ++lineNb)
		{
			string[] result = lines[lineNb].Split (new char[]{' '}, _length, System.StringSplitOptions.RemoveEmptyEntries);
			
			for( int columnNb = 0; columnNb < _length; ++columnNb)
			{
				matrix[lineNb-start, columnNb] = result[columnNb];
			}
		}

		return true;
	}

	private void ParameterMap()
	{
		Map.Initialize(_width, _length);

		Debug.Log("Map successfully initialized.");
	}

	private void FillMap()
	{
		for(int x = 0; x < _width; ++x)
		{
			for(int y = 0; y < _length; ++y)
			{
				for(int count = 0; count < (_heightMatrix[x,y] / 10); ++count)
				{
					Map.InsertBloc(x, y, BlocFactory.CreateBloc());
				}
				string key = _elementsMatrix[x,y];
				Bloc.BlocType type = _stringToElementType[key];

				Bloc bloc = BlocFactory.CreateBloc(type);
				Map.InsertBloc(x, y, bloc);

				if (_stringToTeams.ContainsKey(key))
				{
					Unit unit = UnitFactory.CreateUnit(_stringToTeams[key], Bloc.BlocType.Ice, Source.SourceType.Electricity);
					unit.MoveToBloc(bloc);
					unit.FaceYourOpponent();
				}
			}
		}

		Debug.Log("Map successfully filled. Map is ready for use.");
	}

	private static Dictionary<string, Bloc.BlocType> CreateElementsDictionary()
	{
		Dictionary<string, Bloc.BlocType> tmpDictionnary = new Dictionary<string, Bloc.BlocType>();
		
		tmpDictionnary.Add("G", Bloc.BlocType.Earth);
		tmpDictionnary.Add("R", Bloc.BlocType.Rock);
		tmpDictionnary.Add("I", Bloc.BlocType.Ice);
		tmpDictionnary.Add("M", Bloc.BlocType.Metal);
		tmpDictionnary.Add("P", Bloc.BlocType.Plant);
		tmpDictionnary.Add("1", Bloc.BlocType.Earth);
		tmpDictionnary.Add("2", Bloc.BlocType.Earth);
		
		return tmpDictionnary;
	}

	private static Dictionary<string, Unit.ETeam> CreateTeamsDictionary()
	{
		var dict = new Dictionary<string, Unit.ETeam>();

		dict.Add("1", Unit.ETeam.Totem);
		dict.Add("2", Unit.ETeam.Monster);

		return dict;
	}

	private static bool FactoriesReady()
	{
		return BlocFactory.IsReady()
			&& SourceFactory.IsReady()
			&& StreamFactory.IsReady();
	}
}
