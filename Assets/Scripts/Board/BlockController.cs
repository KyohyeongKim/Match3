using UnityEngine;
using UnityEngine.Serialization;

public class BlockController : MonoBehaviour
{
	[SerializeField]
	GameObject blockPrefab;
	
	[SerializeField]
	BlockData[] blockDatas;

	private Block[,] _blocks;
	
	private Block _clickedBlock;

	public void CreateBlocks(int width, int height)
	{
		_blocks = new Block[width, height];

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				GameObject go = Instantiate(blockPrefab, transform);
				
				BlockData data = blockDatas[Random.Range(0, blockDatas.Length)];
				Block block = go.GetComponent<Block>();
				
				Vector2Int coord = new Vector2Int(j, i);
				block.SetPosition(CoordinateHelper.ConvertToWorldPos(coord));
				int index = CoordinateHelper.ConvertToIndex(coord);
				block.Init(index, data, OnClickedBlock);
				
				_blocks[j, i] = block;
			}
		}
	}

	private void OnClickedBlock(int index)
	{
		Debug.Log("블럭 클릭! index : " + index);
		
		if (_clickedBlock == null)
		{
			Vector2Int coordinate = CoordinateHelper.ConvertFromIndex(index);
			_clickedBlock = _blocks[coordinate.x, coordinate.y];
			_clickedBlock.SetDark(true);
		}
		else if (index == _clickedBlock.index)
		{
			_clickedBlock.SetDark(false);
			_clickedBlock = null;
		}
		else if (IsNeighbor(_clickedBlock.index, index) == false)
		{
			Debug.Log("인접한 블럭이 아니므로 무효");
			_clickedBlock.SetDark(false);
			_clickedBlock = null;
		}
		else
		{
			Debug.Log($"블럭 위치 변경, 첫번째 : {_clickedBlock.index}, 두번째 : {index}");
			
			Vector2Int firstCoord = CoordinateHelper.ConvertFromIndex(_clickedBlock.index);
			Vector2Int secondCoord = CoordinateHelper.ConvertFromIndex(index);
			Vector2 pos = CoordinateHelper.ConvertToWorldPos(secondCoord);
			
			Block secondBlock = _blocks[secondCoord.x, secondCoord.y];
			secondBlock.SetPosition(_clickedBlock.transform.localPosition, true);
			_clickedBlock.SetPosition(pos, true);
			
			_blocks[firstCoord.x, firstCoord.y] = secondBlock;
			_blocks[secondCoord.x, secondCoord.y] = _clickedBlock;
			
			int tempIndex = _clickedBlock.index;
			_clickedBlock.index = index;
			secondBlock.index = tempIndex;
			
			_clickedBlock.SetDark(false);
			_clickedBlock = null;
			
			bool checkFirstBlock = MatchHelper.Match3(firstCoord, _blocks);
			bool checkSecondBlock = MatchHelper.Match3(secondCoord, _blocks);
			
			if (checkFirstBlock || checkSecondBlock)
				Debug.Log("매치 성공");
		}
	}

	private bool IsNeighbor(int clickedIndex, int newIndex)
	{
		bool result = newIndex == clickedIndex + 1 || newIndex == clickedIndex - 1 ||
		              newIndex == clickedIndex + 8 || newIndex == clickedIndex - 8;

		return result;
	}
}
