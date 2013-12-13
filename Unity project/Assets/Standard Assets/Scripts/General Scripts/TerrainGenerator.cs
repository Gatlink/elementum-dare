using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour 
{
	public string terrainFilePath = @"Assets\HeightMaps\heightMap_1.hmap";

	private const int CUBE_SIZE = 50;

	private int[,] heightMatrix;
	private int width;
	private int length;

	// Use this for initialization
	void Start () 
	{
		heightMatrix = BuildMatrixFromTextInput();

		width = heightMatrix.GetLength(0);
		length = heightMatrix.GetLength(1);

		Debug.Log(width + "-" + length);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	int[,] BuildMatrixFromTextInput ()
	{
		int[,] tmpMatrix = {};

		string[] lines = System.IO.File.ReadAllLines(terrainFilePath);
		
		for( int lineNb = 0;  lineNb < lines.Length; ++lineNb)
		{
			string currentLine = lines[lineNb];

			for( int columnNb = 0; columnNb < currentLine.Length; ++columnNb)
			{
				tmpMatrix[lineNb, columnNb] = System.Convert.ToInt32(currentLine[columnNb]);
			}
		}

		return tmpMatrix;
	}
}
