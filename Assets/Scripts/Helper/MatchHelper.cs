using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumDef;

public static class MatchHelper
{
	private static List<Vector2Int> HorizontalMatches(Vector2Int coord, BlockColor color, Block[,] blocks)
	{
		List<Vector2Int> result = new List<Vector2Int>();
		result.Add(new Vector2Int(coord.x, coord.y));
		
		for (int i = 1; i < 3; i++)
		{
			if (coord.y + i >= blocks.GetLength(0))
				break;

			if (blocks[coord.x, coord.y + i].BlockData.color != color)
				break;
			 
			result.Add(new Vector2Int(coord.x, coord.y + i));
		}
		 
		for (int i = 1; i < 3; i++)
		{
			if (coord.y - i < 0)
				break;

			if (blocks[coord.x, coord.y - i].BlockData.color != color)
				break;
			 
			result.Add(new Vector2Int(coord.x, coord.y - i));
		}
		
		Debug.Log($"세로 매치 갯수 : {result.Count}개");
		if (result.Count >= 3)
			return result;

		return new List<Vector2Int>();
	}

	private static List<Vector2Int> VerticalMatches(Vector2Int coord, BlockColor color, Block[,] blocks)
	{
		List<Vector2Int> result = new List<Vector2Int>();
		result.Add(new Vector2Int(coord.x, coord.y));

		for (int j = 1; j < 3; j++)
		{
			if (coord.x + j >= blocks.GetLength(1))
				break;
			 
			if (blocks[coord.x + j, coord.y].BlockData.color != color)
				break;
			 
			result.Add(new Vector2Int(coord.x + j, coord.y));
		}
		 
		for (int j = 1; j < 3; j++)
		{
			if (coord.x - j < 0)
				break;
			 
			if (blocks[coord.x - j, coord.y].BlockData.color != color)
				break;
			 
			result.Add(new Vector2Int(coord.x - j, coord.y));
		}
		 
		Debug.Log($"가로 매치 갯수 : {result.Count}개");
		if (result.Count >= 3)
			return result;

		return new List<Vector2Int>();
	}

	public static List<Vector2Int> GetMatches(Vector2Int coord, Block[,] blocks)
	{
		BlockColor color = blocks[coord.x, coord.y].BlockData.color;
		
		List<Vector2Int> matches = new List<Vector2Int>();
		
		var horizontal = HorizontalMatches(coord, color, blocks);
		var vertical = VerticalMatches(coord, color, blocks);
		
		if (horizontal.Count > 0)
			matches.AddRange(horizontal);
		if (vertical.Count > 0)
			matches.AddRange(vertical);
		
		matches = matches.Distinct().ToList();
		return matches;
	}
}
