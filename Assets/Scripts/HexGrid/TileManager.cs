using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
	[SerializeField]
	private GameObject tilePrefab = null;

	// Start is called before the first frame update
	void Start() {
		for (int x = 0; x < 100; x++) {
			for (int y = 0; y < 100; y++) {
				Instantiate(tilePrefab).GetComponent<TileHandler>().SetCoordinate(x, y);
			}
		}
	}

	// Update is called once per frame
	void Update() {
		
	}
}
