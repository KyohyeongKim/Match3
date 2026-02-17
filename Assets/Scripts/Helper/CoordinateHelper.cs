using UnityEngine;

public static class CoordinateHelper
{
	public static Vector2 ConvertToWorldPos(Vector2Int coord)
	{
		return new Vector3(coord.x - 3.5f, coord.y - 3.5f, 0f);
	}

	public static Vector2Int ConvertFromIndex(int index)
	{
		int x = index % 8;
		int y = index / 8;
		return new Vector2Int(x, y);
	}

	public static int ConvertToIndex(Vector2Int coordinates)
	{
		int index = coordinates.x + coordinates.y * 8;
		return index;
	}
}
