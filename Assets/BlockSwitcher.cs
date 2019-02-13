using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSwitcher : MonoBehaviour
{
	public ObjectPicker op;
	public Text text;
	public List<string> blockNames;
	public List<GameObject> blockPrefabs;
	int currentIdx = 0;

	public bool CheckBlockLists()
	{
		if (blockNames.Count != blockPrefabs.Count)
		{
			Debug.Log("Mismatching block names/block prefabs");
			return false;
		}
		if (blockNames.Count == 0 || blockPrefabs.Count == 0)
		{
			Debug.Log("Add blocks to be able to switch");
			return false;
		}
		return true;
	}

	public void Start()
	{
		if (!CheckBlockLists())
		{
			text.text = "";
		}
		else
		{
			UpdateBlock();
		}
		
	}

	public void SwitchBlock()
	{
		if (!CheckBlockLists()) return;
		currentIdx = (currentIdx + 1) % blockNames.Count;
		UpdateBlock();
	}

	void UpdateBlock()
	{
		text.text = blockNames[currentIdx];
		op.buildingBlockPrefab = blockPrefabs[currentIdx];
		op.GeneratePreviewBuildingBlock();
	}
}
