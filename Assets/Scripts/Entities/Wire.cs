using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : Entity {
	private Connection connection;
	private HexDirection direction;

	public void SetConnection(Connection connection, HexDirection direction) {
		this.connection = connection;
		this.direction = direction;
		this.transform.rotation = Quaternion.AngleAxis(direction.ToDegrees(), Vector3.up);
	}

	void OnDestroy() {
		connection.Disconnect();
	}
}
