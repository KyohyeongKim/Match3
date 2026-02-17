using UnityEngine;
using EnumDef;

public static class MatchHelper
{
	public static bool Match3(Vector2Int coord, Block[,] blocks)
	{
		 BlockColor color = blocks[coord.x, coord.y].BlockData.color;

		 bool horizontal = CheckHorizontal(coord, color, blocks);
		 bool vertical = CheckVertical(coord, color, blocks);
		 return horizontal || vertical; 
	}

	private static bool CheckHorizontal(Vector2Int coord, BlockColor color, Block[,] blocks)
	{
		int count = 1;
		for (int i = 1; i < 3; i++)
		{
			if (coord.y + i >= blocks.GetLength(0))
				break;

			if (blocks[coord.x, coord.y + i].BlockData.color != color)
				break;
			 
			count++;
		}
		 
		for (int i = 1; i < 3; i++)
		{
			if (coord.y - i < 0)
				break;

			if (blocks[coord.x, coord.y - i].BlockData.color != color)
				break;
			 
			count++;
		}
		
		Debug.Log($"세로 매치 갯수 : {count}개");
		return count >= 3;
	}

	private static bool CheckVertical(Vector2Int coord, BlockColor color, Block[,] blocks)
	{
		int count = 1;
		for (int j = 1; j < 3; j++)
		{
			if (coord.x + j >= blocks.GetLength(1))
				break;
			 
			if (blocks[coord.x + j, coord.y].BlockData.color != color)
				break;
			 
			count++;
		}
		 
		for (int j = 1; j < 3; j++)
		{
			if (coord.x - j < 0)
				break;
			 
			if (blocks[coord.x - j, coord.y].BlockData.color != color)
				break;
			 
			count++;
		}
		 
		Debug.Log($"가로 매치 갯수 : {count}개");
		return count >= 3;
	}
}
