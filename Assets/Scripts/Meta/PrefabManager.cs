using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
	[Header("Static objects")]
	public GameObject MountainEntityPrefab;
	public GameObject RiverEntityPrefab;

	[Header("Dynamic objects")]
	public GameObject FactoryPrefab;
	public List<GameObject> ResourcePrefabs;
	public GameObject MarkedPrefab;

	[Header("Wire objects")]
	public GameObject WirePrefab;
	public GameObject WirePreviewPrefab;
	public GameObject WireGhostPrefab;
}
