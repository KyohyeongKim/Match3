using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

		var matches = MatchHelper.GetMatches(_blocks);
		while (matches.Count > 0)
		{
			Debug.Log("매치 생성된 블럭들의 정보 수정");
			
			for (int i = 0; i < matches.Count; i++)
			{
				BlockData data = blockDatas[Random.Range(0, blockDatas.Length)];
				Block block = _blocks[matches[i].x, matches[i].y];
				int index = CoordinateHelper.ConvertToIndex(matches[i]);
				block.Init(index, data, OnClickedBlock);
			}
			
			matches = MatchHelper.GetMatches(_blocks);
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
			StartCoroutine(ProceedMatches(index));
		}
	}

	private bool IsNeighbor(int clickedIndex, int newIndex)
	{
		bool result = newIndex == clickedIndex + 1 || newIndex == clickedIndex - 1 ||
		              newIndex == clickedIndex + 8 || newIndex == clickedIndex - 8;

		return result;
	}

	private IEnumerator ProceedMatches(int index)
	{
		Debug.Log($"블럭 위치 변경, 첫번째 : {_clickedBlock.index}, 두번째 : {index}");
			
		Vector2Int firstCoord = CoordinateHelper.ConvertFromIndex(_clickedBlock.index);
		Vector2Int secondCoord = CoordinateHelper.ConvertFromIndex(index);
			
		ChangeBlocks(firstCoord, secondCoord, index);
			
		_clickedBlock.SetDark(false);
		_clickedBlock = null;
		
		yield return new WaitForSeconds(1f);

		List<Vector2Int> matches = MatchHelper.GetMatches(_blocks);

		while (matches.Count > 0)
		{
			Debug.Log("매치 성공");
			for (int i = 0; i < matches.Count; i++)
			{
				int idx = i;
				_blocks[matches[idx].x, matches[idx].y].Destroy();
				_blocks[matches[idx].x, matches[idx].y] = null;
			}
			
			yield return StartCoroutine(Fall());
			yield return StartCoroutine(FillBlocks());
			
			yield return new WaitForSeconds(0.5f);
			matches = MatchHelper.GetMatches(_blocks);
		}
	}
	
	private void ChangeBlocks(Vector2Int firstCoord, Vector2Int secondCoord, int index)
	{
		Vector2 pos = CoordinateHelper.ConvertToWorldPos(secondCoord);
			
		Block secondBlock = _blocks[secondCoord.x, secondCoord.y];
		secondBlock.SetPosition(_clickedBlock.transform.localPosition, true);
		_clickedBlock.SetPosition(pos, true);

		_blocks[firstCoord.x, firstCoord.y] = secondBlock;
		_blocks[secondCoord.x, secondCoord.y] = _clickedBlock;

		int tempIndex = _clickedBlock.index;
		_clickedBlock.index = index;
		secondBlock.index = tempIndex;
	}

	private IEnumerator Fall()
	{
		for (int i = 1; i < _blocks.GetLength(1); i++)
		{
			for (int j = 0; j < _blocks.GetLength(0); j++)
			{
				if (_blocks[j, i] == null)
					continue;

				int targetY = -1;
				for (int idx = i; idx > 0; idx--)
				{
					if (_blocks[j, idx - 1] == null)
						targetY = idx - 1;
				}

				if (targetY < 0)
					continue;
				
				_blocks[j, i].SetPosition(CoordinateHelper.ConvertToWorldPos(new Vector2Int(j, targetY)), true);
				_blocks[j, targetY] = _blocks[j, i];
				_blocks[j, targetY].index = CoordinateHelper.ConvertToIndex(new Vector2Int(j, targetY));
				_blocks[j, i] = null;
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	private IEnumerator FillBlocks()
	{
		for (int i = 0; i < _blocks.GetLength(1); i++)
		{
			for (int j = 0; j < _blocks.GetLength(0); j++)
			{
				if (_blocks[j, i] == null)
				{
					GameObject go = Instantiate(blockPrefab, transform);
				
					BlockData data = blockDatas[Random.Range(0, blockDatas.Length)];
					Block block = go.GetComponent<Block>();
				
					Vector2Int createCoord = new Vector2Int(j, 10);
					block.SetPosition(CoordinateHelper.ConvertToWorldPos(createCoord));
					
					Vector2Int pos = new Vector2Int(j, i);
					block.SetPosition(CoordinateHelper.ConvertToWorldPos(pos), true);
					
					int index = CoordinateHelper.ConvertToIndex(pos);
					block.Init(index, data, OnClickedBlock);
				
					_blocks[j, i] = block;
					yield return new WaitForSeconds(0.1f);
				}
			}
		}
	}
}
