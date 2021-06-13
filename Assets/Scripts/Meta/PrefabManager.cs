using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
	[Header("Static objects")]
	public GameObject MountainEntityPrefab;
	public GameObject RiverEntityPrefab;

	[Header("Factory objects")]
	public GameObject MetalFactory;
	public GameObject MetalComponentFactory;
	public GameObject PlankFactory;
	public GameObject FurnitureFactory;
	public GameObject FineFurnitureFactory;
	public GameObject BasicToolFactory;
	public GameObject HeaterFactory;

	[Header("Dynamic objects")]
	public List<GameObject> ResourcePrefabs;
	public GameObject MarkedPrefab;

	[Header("Wire objects")]
	public GameObject WirePrefab;
	public GameObject WirePreviewPrefab;
	public GameObject WireGhostPrefab;

}
