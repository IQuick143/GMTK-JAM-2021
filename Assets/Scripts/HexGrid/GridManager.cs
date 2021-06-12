using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	[SerializeField]
	private GameObject tilePrefab;

	public HexArray<TileHandler> tiles {get; private set;}
	private float sqrt_3 = Mathf.Sqrt(3);
	private float radius = 1/Mathf.Sqrt(3);

	private Vector2Int mouseoverCoordinates = new Vector2Int(-1,-1);

	// Start is called before the first frame update
	void Start() {
		int size_x = 100;
		int size_y = 100;
		tiles = new HexArray<TileHandler>(size_x, size_y);
		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_y; y++) {
				tiles[x,y] = Instantiate(tilePrefab).GetComponent<TileHandler>();
				tiles[x,y].manager = this;
				tiles[x,y].SetGeometry(x, y, radius);
			}
		}
	}

	void Update() {
		var old_mouseover = this.mouseoverCoordinates;

		var xz_coords = GameManager.input.GetMouseXZIntersect();
		if (xz_coords == Vector2.positiveInfinity) this.mouseoverCoordinates = new Vector2Int(-1,-1);
		else this.mouseoverCoordinates = GetCoordinateFromXZ(xz_coords);

		if (old_mouseover != this.mouseoverCoordinates) {
			if (this.tiles.IsInBounds(old_mouseover)) this.tiles[old_mouseover].hover = false;
			if (this.tiles.IsInBounds(this.mouseoverCoordinates)) this.tiles[this.mouseoverCoordinates].hover = true;
			Debug.Log(this.mouseoverCoordinates);
		}
	}

	///<summary>
	///	Converts a point in the XZ plane to a corresponding TileManager, returns null if out of bounds
	///</summary>
	public TileHandler GetTileFromXZ(Vector2 position) {
		Vector2Int coords = GetCoordinateFromXZ(position);
		if (this.tiles.IsInBounds(coords.x,coords.y)) return this.tiles[coords.x,coords.y];
		else return null;
	}

	///<summary>
	///	Converts a point in the XZ plane to a corresponding offset coordinate
	///</summary>
	public Vector2Int GetCoordinateFromXZ(Vector2 position) {
		// DO NOT FUCKING TOUCH THIS
		// THIS IS A SACRED PLACE
		// A FOREST OF WRONG LABELS AND SQUIGGLY PATHS
		// IT LEADS TO THE CORRECT DESINATION SO DON'T """FIX""" IT OKAY

		//Convert to axial coordinates
		position /= this.radius;
		float q = position.y / sqrt_3 - position.x / 3;
		float r = 2 * position.x / 3;

		//Convert axial to cube and round
		Vector3Int cube = cube_coordinate_hex_round(new Vector3(r, -q-r, q));

		//Convert cube to offset
		var col = cube.x;
		var row = cube.z + cube.x/2 + (cube.x % 2);
		return new Vector2Int(col,row);
	}

	///<summary>
	///	Converts a point in the XZ plane to a corresponding offset coordinate
	///</summary>
	public Vector2Int GetCoordinateFromXZ(Vector3 position) {
		return GetCoordinateFromXZ(new Vector2(position.x, position.z));
	}

	public static Vector3Int cube_coordinate_hex_round(Vector3 cube_coordinate) {
		var rx = Mathf.RoundToInt(cube_coordinate.x);
		var ry = Mathf.RoundToInt(cube_coordinate.y);
		var rz = Mathf.RoundToInt(cube_coordinate.z);

		//Do not question this formula
		var x_diff = Mathf.Abs(rx - cube_coordinate.x);
		var y_diff = Mathf.Abs(ry - cube_coordinate.y);
		var z_diff = Mathf.Abs(rz - cube_coordinate.z);

		if (x_diff > y_diff && x_diff > z_diff) {
			rx = -ry-rz;
		} else if (y_diff > z_diff) {
			ry = -rx-rz;
		} else {
			rz = -rx-ry;
		}

		return new Vector3Int(rx, ry, rz);
	}
}
