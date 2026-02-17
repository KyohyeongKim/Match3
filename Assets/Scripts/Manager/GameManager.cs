using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	
	[SerializeField] private Board board;
	public Board Board => board;
	
	private void Awake()
	{
		instance = this;

		Init();
	}

	private void Init()
	{
		board.CreateBoard(8, 8);

		BlockController blockController = FindFirstObjectByType<BlockController>();
		if (blockController != null)
			blockController.CreateBlocks(8, 8);
	}
}
