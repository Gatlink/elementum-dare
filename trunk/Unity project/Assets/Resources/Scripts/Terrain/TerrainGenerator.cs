using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TerrainGenerator : MonoBehaviour 
{
	public string mapFilePath = @"Assets\Resources\HeightMaps\heightMap_1.hmap";

	public Map terrainObject;
	
	private const int DIMENSIONS_LINE = 0;
	private const int SECTIONS_LINE_JUMP = 1;

	private int[,] _heightMatrix;
	private string[,] _elementsMatrix;
	private int _width;
	private int _length;

	private static Dictionary<string, Bloc.BlocType> stringToElementType = CreateElementsDictionnary();
	
	// Use this for initialization
	void Start () 
	{
		GenerateMatrixes();
		ParameterMap();
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
		terrainObject.Initialize(_width, _length);

		Debug.Log("Map successfully initialized.");
	}

	private void FillMap()
	{
		for(int x = 0; x < _width; ++x)
		{
			for(int y = 0; y < _length; ++y)
			{
				for(int count = 0; count < (_heightMatrix[x,y] / 10) - 1; ++count)
				{
					terrainObject.InsertBloc(x, y, BlocFactory.CreateBloc());
				}

				Bloc.BlocType type = stringToElementType[_elementsMatrix[x,y]];
				Debug.Log (type.ToString());
				terrainObject.InsertBloc(x, y, BlocFactory.CreateBloc(type));

				//TODO check for spawn
			}
		}

		Debug.Log("Map successfully filled. Map is ready for use.");
	}

	private static Dictionary<string, Bloc.BlocType> CreateElementsDictionnary()
	{
		Dictionary<string, Bloc.BlocType> tmpDictionnary = new Dictionary<string, Bloc.BlocType>();
		
		tmpDictionnary.Add("G", Bloc.BlocType.Earth);
		tmpDictionnary.Add("R", Bloc.BlocType.Rock);
		tmpDictionnary.Add("I", Bloc.BlocType.Ice);
		tmpDictionnary.Add("M", Bloc.BlocType.Metal);
		tmpDictionnary.Add("P", Bloc.BlocType.Plant);
		tmpDictionnary.Add("S", Bloc.BlocType.Earth);
		
		return tmpDictionnary;
	}
}
