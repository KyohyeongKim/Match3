using EnumDef;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BlockData", menuName = "Board/BlockData", order = 1)]
public class BlockData : ScriptableObject
{
	public BlockColor color;
	public Sprite sprite;
}