using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : Entity {
	private Connection connection;
	private HexDirection direction;
	[SerializeField]
	private List<Renderer> renderers;

	public void SetConnection(Connection connection, HexDirection direction) {
		this.connection = connection;
		this.direction = direction;
		this.transform.rotation = Quaternion.AngleAxis(direction.ToDegrees(), Vector3.up);
		foreach (var renderer in renderers) {
			renderer.material.color = connection.Stream.GetColour();
		}
	}

	void OnDestroy() {
		connection.Disconnect();
	}
}
