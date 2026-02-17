using UnityEngine;

public class Square : MonoBehaviour
{
	SpriteRenderer _spriteRenderer;

	public void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		// _spriteRenderer.sprite
	}
}
