using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : Entity {
	private Connection connection;
	private HexDirection direction;

	public Wire(Connection connection, HexDirection direction) {
		this.connection = connection;
		this.direction = direction;
	}

	public override GameObject CreateVisualObject() {
		var parent = new GameObject();
		var WireA = GameObject.Instantiate(GameManager.prefab.WirePrefab);
		WireA.transform.SetParent(parent.transform);
		parent.transform.rotation = Quaternion.AngleAxis(direction.ToDegrees(), Vector3.up);
		return parent;
	}

	public override void Delete() {
		connection.Disconnect();
	}
}
