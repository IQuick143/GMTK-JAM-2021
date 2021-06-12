using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	[SerializeField]
	private GameObject tilePrefab;

	public HexArray<TileHandler> tiles {get; private set;}
	private float sqrt_3 = Mathf.Sqrt(3);
	private float radius = 1/Mathf.Sqrt(3);

	private Vector2 lastMouseDownPosition;
	private Vector2Int mouseoverCoordinates = new Vector2Int(-1,-1);
	private float MOUSE_DRAG_THRESHOLD = 10f;

	private bool connecting = false;
	private List<Vector2Int> connectionPoints = new List<Vector2Int>();
	private List<GameObject> connectionPreview = new List<GameObject>();
	private GhostPreview connectionHeadGhost;

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
				tiles[x,y].transform.position = GetPositionFromCoordinate(new Vector2Int(x,y));
			}
		}
	}

	void Update() {
		bool LMB = Input.GetMouseButtonUp(0) && Vector2.Distance(Input.mousePosition, lastMouseDownPosition) < MOUSE_DRAG_THRESHOLD;
		bool RMB = Input.GetMouseButtonUp(1) && Vector2.Distance(Input.mousePosition, lastMouseDownPosition) < MOUSE_DRAG_THRESHOLD;

		var old_mouseover = this.mouseoverCoordinates;

		var xz_mouse_coords = GameManager.input.GetMouseXZIntersect();
		if (xz_mouse_coords == Vector2.positiveInfinity) this.mouseoverCoordinates = new Vector2Int(-1,-1);
		else this.mouseoverCoordinates = GetCoordinateFromXZ(xz_mouse_coords);

		if (old_mouseover != this.mouseoverCoordinates) {
			if (this.tiles.IsInBounds(old_mouseover)) this.tiles[old_mouseover].hover = false;
			if (this.tiles.IsInBounds(this.mouseoverCoordinates)) this.tiles[this.mouseoverCoordinates].hover = true;
		}

		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			lastMouseDownPosition = Input.mousePosition;

		if (connecting) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				CancelConnecting();
			} else {
				// Find direction closest to cursor
				Vector2Int last_point = this.connectionPoints[this.connectionPoints.Count - 1];
				var neighbours = this.tiles.GetNeighbourCooridnates(last_point.x, last_point.y);
				Vector2Int closest = Vector2Int.zero;
				float sqrdistance = float.MaxValue;
				foreach (var neighbour in neighbours) {
					float dist = (new Vector3(xz_mouse_coords.x, 0f, xz_mouse_coords.y) - GetPositionFromCoordinate(neighbour)).sqrMagnitude;
					if (dist < sqrdistance) {
						closest = neighbour;
						sqrdistance = dist;
					}
				}

				Vector3 last_pos = GetPositionFromCoordinate(last_point);
				Vector3 next_pos = GetPositionFromCoordinate(closest);
				Vector3 direction = next_pos - last_pos;
				Vector3 midpoint = (next_pos + last_pos)/2f;
				connectionHeadGhost.transform.position = midpoint;
				connectionHeadGhost.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

				bool canPlace = false;
				bool canConnect = false;
				if (this.tiles[closest].entity == null) canPlace = !this.connectionPoints.Contains(closest);
				else if (this.tiles[closest].entity.GetType() == typeof(Connectable)) {
					var start_factory = this.tiles[connectionPoints[0]].entity as Connectable;
					var end_factory = this.tiles[closest].entity as Connectable;
					canConnect = canPlace = start_factory.CanConnect(end_factory);
					// Prevent direct connections
					canPlace &= this.connectionPoints.Count > 1;
				}

				connectionHeadGhost.valid = canPlace;

				if (canPlace && LMB) {
					this.connectionPoints.Add(closest);
					this.connectionPreview.Add(Instantiate(GameManager.prefab.WirePreviewPrefab, midpoint, Quaternion.LookRotation(direction, Vector3.up)));
					if (canConnect) {
						FinishConnecting();
					}
				}
			}
		} else if (this.tiles.IsInBounds(this.mouseoverCoordinates)) {
			if (LMB) {
				 this.tiles[this.mouseoverCoordinates].OnLMB();
			}
			if (RMB) {
				 this.tiles[this.mouseoverCoordinates].OnRMB();
			}
		}
	}

	public void BeginConnecting(int x, int y) {
		this.connecting = true;

		connectionHeadGhost = Instantiate(GameManager.prefab.WireGhostPrefab).GetComponent<GhostPreview>();
		connectionPoints.Clear();
		connectionPoints.Add(new Vector2Int(x,y));
	}

	public void CancelConnecting() {
		this.connecting = false;
		
		foreach (var preview_object in connectionPreview) {
			Destroy(preview_object);
		}
		Destroy(connectionHeadGhost.gameObject);
		connectionPoints.Clear();
		connectionPreview.Clear();
	}

	public void FinishConnecting() {
		this.connecting = false;

		var connectionTiles = new List<TileHandler>(connectionPoints.Count - 2);
		for (int i = 1; i < connectionPoints.Count - 1; i++) {
			connectionTiles.Add(this.tiles[connectionPoints[i]]);
		}

		Connectable A = this.tiles[connectionPoints[0]].entity as Connectable;
		Connectable B = this.tiles[connectionPoints[connectionPoints.Count - 1]].entity as Connectable;

		new Connection(connectionPoints, connectionTiles, this.tiles[connectionPoints[0]].transform.position, A, B);

		foreach (var preview_object in connectionPreview) {
			Destroy(preview_object);
		}
		Destroy(connectionHeadGhost.gameObject);
		connectionPoints.Clear();
		connectionPreview.Clear();
	}

	///<summary>
	///	Converts a point in the XZ plane to a corresponding TileHandler, returns null if out of bounds
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
		// nvm it was wrong but I fixed it
		// maybe

		//Convert to axial coordinates
		position /= this.radius;
		float q = position.y / sqrt_3 - position.x / 3;
		float r = 2 * position.x / 3;

		//Convert axial to cube and round
		Vector3Int cube = cube_coordinate_hex_round(new Vector3(r, -q-r, q));

		//Convert cube to offset
		var col = cube.x;
		var row = cube.z + cube.x/2 + (cube.x % 2);
		return new Vector2Int(row, col);
	}

	///<summary>
	///	Converts a point in the XZ plane to a corresponding offset coordinate
	///</summary>
	public Vector2Int GetCoordinateFromXZ(Vector3 position) {
		return GetCoordinateFromXZ(new Vector2(position.x, position.z));
	}


	///<summary>
	///	Converts an offset coordinate to the center of that tile in the XZ
	///</summary>
	public Vector3 GetPositionFromCoordinate(Vector2Int coordinate) {
		int half_offset = coordinate.y % 2;
		return new Vector3(coordinate.y * 1.5f * radius, 0f, (coordinate.x - half_offset / 2f) * radius * sqrt_3);
	}

	public static Vector2Int offset_to_axial(Vector2Int offset) {
		return new Vector2Int(offset.x - (offset.y + (offset.y&1)) / 2, offset.y);
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
