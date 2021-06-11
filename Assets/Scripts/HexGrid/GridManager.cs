using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	[SerializeField]
	private GameObject tilePrefab = null;

	public HexArray<TileHandler> tiles {get; private set;}

	// Start is called before the first frame update
	void Start() {
		int size_x = 100;
		int size_y = 100;
		tiles = new HexArray<TileHandler>(size_x, size_y);
		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_y; y++) {
				tiles[x,y] = Instantiate(tilePrefab).GetComponent<TileHandler>();
				tiles[x,y].manager = this;
				tiles[x,y].SetCoordinate(x, y);
			}
		}
	}

	// Update is called once per frame
	void Update() {
		
	}
}
