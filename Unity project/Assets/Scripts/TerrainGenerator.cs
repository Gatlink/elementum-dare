using UnityEngine;
using System.Collections;
using System.IO;

public class TerrainGenerator : MonoBehaviour 
{
	public string terrainFilePath = @"Assets\HeightMaps\heightMap_1.hmap";

	private const int CUBE_SIZE = 10;

	private int[,] _heightMatrix;
	private string[,] _elementsMatrix;
	private int _width;
	private int _length;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("STARTING");

		if (!File.Exists(terrainFilePath))
		{
			Debug.Log ("File does not exist. [" + terrainFilePath +"]");
			return;
		}

		//Start by reading the whole file
		string[] lines = File.ReadAllLines(terrainFilePath);

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
		
		Debug.Log(_width + "-" + _length);

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

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private bool ReadMapDimensions(string[] lines, ref int width, ref int length)
	{
		string dimensions = lines[0];

		if(dimensions.Length < 3)
		{
			Debug.Log("Dimension's line is ill formated (empty or not enough parameters).");
			return false;
		}

		width = 8;//System.Convert.ToInt32(new string(dimensions[0]));
		length = 8;//System.Convert.ToInt32(new string(dimensions[2]));

		return true;
	}

	private bool BuildHeightMatrix(string[] lines, ref int[,] matrix)
	{
		//Skip dimensions line and first line jump
		int start = 2;
		int end = start + _width;

		if(lines.Length < end)
		{
			Debug.Log("Heightmap dimensions do not match dimensions specified in file.");
			return false;
		}

		for( int lineNb = start;  lineNb < end; ++lineNb)
		{
			string currentLine = lines[lineNb];

			if(currentLine.Length < _length)
			{
				Debug.Log("Heightmap dimensions do not match dimensions specified in file.");
				return false;
			}

			for( int columnNb = 0; columnNb < _length; ++columnNb)
			{
				matrix[lineNb, columnNb] = System.Convert.ToInt32(currentLine[columnNb]);
			}
		}

		return true;
	}

	private bool BuildElementMatrix(string[] lines, ref string[,] matrix)
	{
		return true;
	}
}
