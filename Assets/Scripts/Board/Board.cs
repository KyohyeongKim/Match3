using System.Collections.Generic;
using EnumDef;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
	private int _width;
	private int _height;
	
	[SerializeField]
	GameObject squarePrefab;
	
	public void CreateBoard(int width, int height)
	{
		_width = width;
		_height = height;
		
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				GameObject go = Instantiate(squarePrefab, transform);
				go.transform.localPosition = new Vector3(j - 3.5f, i - 3.5f, 0);
			}
		}
	}
}
