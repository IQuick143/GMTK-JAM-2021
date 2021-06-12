using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Connection {
	private Connectable from;
	private Connectable to;
	private List<TileHandler> wires;
	private bool disconnecting = false;
	private GameObject startingConnection;
	public Item Stream;

	public Connection(List<Vector2Int> wireCoordinates, List<TileHandler> wireTiles, Vector3 fromCoordinate, Connectable from, Connectable to) {
		this.from = from;
		this.to = to;
		this.wires = wireTiles;
		Stream = from.inputs
			.Intersect(to.outputs)
			.Concat(from.outputs
				.Intersect(to.inputs)
			).First();

		for (int i = 0; i < wireTiles.Count; i++) {
			var offset = GridManager.offset_to_axial(wireCoordinates[i+2]) - GridManager.offset_to_axial(wireCoordinates[i+1]);
			HexDirection dir = HexDirection.Right;
			if (offset.x == 0 && offset.y == -1) {
				dir = HexDirection.UpLeft;
			} else if (offset.x == 0 && offset.y == 1) {
				dir = HexDirection.DownRight;
			} else if (offset.x == 1 && offset.y == -1) {
				dir = HexDirection.UpRight;
			} else if (offset.x == 1 && offset.y == 0) {
				dir = HexDirection.Right;
			} else if (offset.x == -1 && offset.y == 1) {
				dir = HexDirection.DownLeft;
			} else if (offset.x == -1 && offset.y == 0) {
				dir = HexDirection.Left;
			}

			Wire wire = new Wire(this, dir);
			this.wires[i].SetObject(wire);
		}

		var first_offset = GridManager.offset_to_axial(wireCoordinates[1]) - GridManager.offset_to_axial(wireCoordinates[0]);

		HexDirection first_dir = HexDirection.Right;
		if (first_offset.x == 0 && first_offset.y == -1) {
			first_dir = HexDirection.UpLeft;
		} else if (first_offset.x == 0 && first_offset.y == 1) {
			first_dir = HexDirection.DownRight;
		} else if (first_offset.x == 1 && first_offset.y == -1) {
			first_dir = HexDirection.UpRight;
		} else if (first_offset.x == 1 && first_offset.y == 0) {
			first_dir = HexDirection.Right;
		} else if (first_offset.x == -1 && first_offset.y == 1) {
			first_dir = HexDirection.DownLeft;
		} else if (first_offset.x == -1 && first_offset.y == 0) {
			first_dir = HexDirection.Left;
		}

		startingConnection = new GameObject();
		var fake_wire = GameObject.Instantiate(GameManager.prefab.WirePrefab);
		fake_wire.transform.SetParent(startingConnection.transform);
		startingConnection.transform.rotation = Quaternion.AngleAxis(first_dir.ToDegrees(), Vector3.up);
		startingConnection.transform.position = fromCoordinate;

		from.Connect(this);
		to.Connect(this);
	}

	/// <summary>
	/// Destroys the connection and related objects, the Connection object is not usable after this call.
	/// </summary>
	public void Disconnect() {
		// To ensure idempotency
		if (disconnecting) {
			return;
		}
		disconnecting = true;

		from?.Disconnect(this);
		to?.Disconnect(this);

		foreach (var wire_tile in this.wires) {
			wire_tile.DeleteObject();
		}
		Object.Destroy(startingConnection);
	}



	/// <summary>
	/// Tells a Connectable what is the other endpoint of the connection
	/// </summary>
	public Connectable Other(Connectable that) {
		if (that == from) return to;
		if (that == to) return from;
		throw new System.ArgumentException("Cannot call other on a connectable that is not connected by this connection.");
	}
}
