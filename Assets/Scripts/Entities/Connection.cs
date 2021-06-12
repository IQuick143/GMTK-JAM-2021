using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {
	private Connectable A;
	private Connectable B;
	private List<TileHandler> wires;
	private bool disconnecting = false;

	/// <summary>
	/// Destroys the connection and related objects, the Connection object is not usable after this call.
	/// </summary>
	public void Disconnect() {
		// To ensure idempotency
		if (disconnecting) {
			return;
		}
		disconnecting = true;

		if (A != null) A.Disconnect(this);
		if (B != null) B.Disconnect(this);

		foreach (var wire_tile in this.wires) {
			wire_tile.DeleteObject();
		}
	}



	/// <summary>
	/// Tells a Connectable what is the other endpoint of the connection
	/// </summary>
	public Connectable Other(Connectable that) {
		if (that == A) return B;
		if (that == B) return A;
		throw new System.ArgumentException("Cannot call other on a connectable that is not connected by this connection.");
	}
}
