using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : Entity {
	private Connection connection;

	public Wire(Connection connection) {
		this.connection = connection;
	}

	public override GameObject GetPrefab() {
		return GameManager.prefab.WirePrefab;
	}

	public override void Delete() {
		connection.Disconnect();
	}
}
