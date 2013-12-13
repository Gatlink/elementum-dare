using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour 
{
	public string terrainFilePath = @"Assets\HeightMaps\heightMap_1.hmap";

	private const int CUBE_SIZE = 10;

	private int[,] _heightMatrix;
	private int _width;
	private int _length;

	// Use this for initialization
	void Start () 
	{
		//Start by reading the whole file
		string[] lines = System.IO.File.ReadAllLines(terrainFilePath);

		//Get the dimensions from the first line
		if(!ReadMapDimensions(lines, _width, _length))
		{
			Debug.Log ("Error reading heightmap dimensions.");
			System.Environment.Exit(-1);
		}

		if(!BuildMatrixFromTextInput(lines, _heightMatrix))
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

	bool ReadMapDimensions(string[] lines, int width, int length)
	{
		System.IO.FileStream stream = System.IO.File.OpenRead(terrainFilePath);
	}

	bool BuildMatrixFromTextInput(string[] lines, int[,] matrix)
	{
		for( int lineNb = 0;  lineNb < lines.Length; ++lineNb)
		{
			string currentLine = lines[lineNb];

			for( int columnNb = 0; columnNb < currentLine.Length; ++columnNb)
			{
				matrix[lineNb, columnNb] = System.Convert.ToInt32(currentLine[columnNb]);
			}
		}

		return true;
	}
}
