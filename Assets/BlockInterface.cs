using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInterface : MonoBehaviour
{
	public List<BlockInterface> connectedBlocks;
	public List<RaycastInteractor> interactors;
	//public new Transform transform;
	public float size;

	public void Start()
	{
		//transform = GetComponent<Transform>();
	}

	public void SetAsPreview(Material mat)
	{
		foreach (RaycastInteractor ri in interactors)
		{
			ri.SetTranslucent(mat);
			ri.gameObject.layer = 2;
		}
	}

	public bool IsOverlapping()
	{
		Vector3 size = new Vector3(1.207f, 1.207f, 1.207f);
		bool hit = Physics.CheckBox(transform.position, size, transform.rotation, 10);
		if(hit)
		{
			Debug.Log("hit");
		}
		return false;
	}

	public bool CanRemove()
	{
		return connectedBlocks.Count <= 1;
	}

	public void AddDependancy(BlockInterface bi)
	{
		connectedBlocks.Add(bi);
	}

	public void RemoveDependancy(BlockInterface bi)
	{
		connectedBlocks.Remove(bi);
	}
}