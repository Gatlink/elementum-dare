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
		if (!File.Exists(terrainFilePath))
		{
			Debug.Log ("File does not exist. [" + terrainFilePath +"]");
			System.Environment.Exit(-1);
		}

		//Start by reading the whole file
		string[] lines = File.ReadAllLines(terrainFilePath);

		if(lines.Length == 0)
		{
			Debug.Log ("Heightmap file is empty.");
			System.Environment.Exit(-1);
		}

		//Get the dimensions from the first line
		if(!ReadMapDimensions(lines, _width, _length))
		{
			Debug.Log ("Error reading heightmap dimensions.");
			System.Environment.Exit(-1);
		}

		if(!BuildHeightMatrix(lines, _heightMatrix))
		{
			Debug.Log ("Error building the heightmap matrix.");
			System.Environment.Exit(-1);
		}

		if(!BuildElementMatrix(lines, _elementMatrix))
		{
			Debug.Log ("Error building the heightmap matrix.");
			System.Environment.Exit(-1);
		}

		Debug.Log(_width + "-" + _length);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	bool ReadMapDimensions(ref string[] lines, ref int width, ref int length)
	{
		string dimensions = lines[0];

		if(dimensions.Length < 3)
		{
			Debug.Log("Dimension's line is ill formated (empty or not enough parameters).");
			return false;
		}

		width = System.Convert.ToInt32(dimensions[0]);
		length = System.Convert.ToInt32(dimensions[2]);

		return true;
	}

	bool BuildHeightMatrix(ref string[] lines, ref int[,] matrix)
	{
		//Skip dimensions line and first line jump
		int start = 2;
		for( int lineNb = start;  lineNb < start + _width; ++lineNb)
		{
			string currentLine = lines[lineNb];

			for( int columnNb = 0; columnNb < _length; ++columnNb)
			{
				matrix[lineNb, columnNb] = System.Convert.ToInt32(currentLine[columnNb]);
			}
		}

		return true;
	}

	bool BuildElementMatrix(ref string[] lines, ref string[,] matrix)
	{

	}
}
