using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteractor : MonoBehaviour
{
	public BlockInterface block;
	public RaycastInteractor oppositeFace;
	private Renderer r;

	void Start()
	{
		if (!r) r = GetComponent<Renderer>();
	}

	public void SetTranslucent(Material mat)
	{
		if (!r) r = GetComponent<Renderer>();
		r.material = mat;
	}

	public void ChangeColor(Color newColor)
	{
		r.material.color = newColor;
	}
}
