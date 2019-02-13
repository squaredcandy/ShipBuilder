using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
	public Camera cam;
	private Transform camTf;
	private bool lmbDown;
	private bool mmbDown;

	private RaycastHit hit;
	private Vector2 mousePos;
	private Ray ray;
	private bool raycastHit;

	private RaycastInteractor lastInteractor;
	private RaycastInteractor currentInteractor;
	private Transform currentInteractorTf;

	public Transform parent;
	public GameObject buildingBlockPrefab;

	private GameObject newGO;
	private BlockInterface newBI;

	public Material previewMaterial;
	private GameObject previewGO;
	private BlockInterface previewBI;
	private Transform previewTf;

	public Color placeableColor;
	public Color unplaceableColor;
	public Color defaultColor;

	void Start()
	{
		camTf = cam.transform;
		GeneratePreviewBuildingBlock();
	}

	public void GeneratePreviewBuildingBlock()
	{
		if(!buildingBlockPrefab.GetComponent<BlockInterface>())
		{
			Debug.Log("Prefab doesn't have a Block Interface Script");
			return;
		}
		if(previewGO)
		{
			Destroy(previewGO);
		}
		previewGO = Instantiate(buildingBlockPrefab, transform);
		previewTf = previewGO.transform;
		previewBI = previewGO.GetComponent<BlockInterface>();
		previewGO.layer = 2;
		previewBI.SetAsPreview(previewMaterial);
	}

	void Update()
	{
		lmbDown = Input.GetMouseButtonDown(0);
		mmbDown = Input.GetMouseButtonDown(2);
		Raycast();
		if (lmbDown) UpdatePlaceStatus();
		if (mmbDown) UpdateRemoveStatus();
		else UpdateHoverStatus();
	}

	void Raycast()
	{
		mousePos = Input.mousePosition;
		ray = cam.ScreenPointToRay(mousePos);
		// We only raycast to colliders on layer 10: BuildingBlock
		const int LAYERMASK = 10;
		raycastHit = Physics.Raycast(ray, out hit, LAYERMASK);
		//Debug.DrawRay(ray.origin, ray.direction, Color.red, 100, true);
	}

	void UpdateRemoveStatus()
	{
		if(currentInteractor)
		{
			if(currentInteractor.block.CanRemove())
			{
				// Remove the dependances on each connected block
				foreach (BlockInterface bi in 
					currentInteractor.block.connectedBlocks)
				{
					bi.RemoveDependancy(currentInteractor.block);
				}
				Destroy(currentInteractor.transform.parent.gameObject);
				currentInteractor = null;
			}
		}
		else
		{
			Debug.Log("Remove the other connected blocks first");
		}
	}

	Vector3 CalculateSpawnPosition()
	{
		Vector3 bck = currentInteractorTf.forward * -1;
		Vector3 orgPos = currentInteractorTf.position;
		Vector3 spawnPos = orgPos + (bck * previewBI.size);
		return spawnPos;
	}

	void UpdatePlaceStatus()
	{
		if(currentInteractor)
		{
			Vector3 spawnPos = CalculateSpawnPosition();

			newGO = Instantiate(buildingBlockPrefab, spawnPos,
				currentInteractorTf.rotation, parent);
			newBI = newGO.GetComponent<BlockInterface>();

			currentInteractor.block.AddDependancy(newBI);
			newBI.AddDependancy(currentInteractor.block);

		}
	}

	void UpdatePreviewPosition()
	{
		previewTf.position = CalculateSpawnPosition();
		previewTf.rotation = currentInteractorTf.rotation;
	}

	// Intercept the hover status and check if we can actually place it down
	void SendHoverStatus(ref RaycastInteractor interactor, bool hovered)
	{
		if(!hovered)
		{
			interactor.ChangeColor(defaultColor);
		}
		else
		{
			if(previewBI.IsOverlapping())
			{
				interactor.ChangeColor(unplaceableColor);
				// Disable the preview
				previewGO.SetActive(false);
			}
			else
			{
				interactor.ChangeColor(placeableColor);
				previewGO.SetActive(true);
			}
		}
	}

	void UpdateHoverStatus()
	{
		if (raycastHit)
		{
			currentInteractor = hit.collider.GetComponent<RaycastInteractor>();
			if (!currentInteractor) return;
			currentInteractorTf = currentInteractor.transform;

			UpdatePreviewPosition();

			// First Update
			if (lastInteractor == null && currentInteractor)
			{
				SendHoverStatus(ref currentInteractor, true);
			}
			// If the last interactor is different
			else if (lastInteractor && currentInteractor != lastInteractor)
			{
				// Disable the last one
				SendHoverStatus(ref lastInteractor, false);
				// Enable the new one
				SendHoverStatus(ref currentInteractor, true);
			}
		}
		else
		{
			if (currentInteractor)
			{
				SendHoverStatus(ref currentInteractor, false);
				currentInteractor = null;
			}

			previewGO.SetActive(false);
		}
		// Set the last interactor for the next frame
		lastInteractor = currentInteractor;
	}
}
