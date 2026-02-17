using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Block : MonoBehaviour
{
	public int index;
	public BlockData BlockData { get; private set; }
	
	SpriteRenderer _spriteRenderer;

	Action<int> _onClickedAction;

	public void Init(int index, BlockData blockData, Action<int> onClickedAction)
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		this.index = index;
		BlockData = blockData;
		_onClickedAction = onClickedAction;
		
		// Position = CoordinateHelper.ConvertFromIndex(index);
		
		if (blockData.sprite == null)
			Debug.LogError("BlockData Sprite is null");
		else
		{
			_spriteRenderer.sprite = blockData.sprite;
		}
	}

	private void OnMouseUpAsButton()
	{
		_onClickedAction?.Invoke(index);
	}

	public void SetPosition(Vector3 pos, bool animate = false)
	{
		if (animate)
			transform.DOMove(pos, 0.5f);
		else
			transform.localPosition = pos;
	}

	public void SetDark(bool dark)
	{
		_spriteRenderer.color = dark ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white; 
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}