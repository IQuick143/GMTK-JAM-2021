using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connectable : Entity {
	public HashSet<Item> inputs {get; private set;}
	public HashSet<Item> outputs {get; private set;}

	public List<Connection> connections = new List<Connection>();

	private GameObject prefab;

	private bool isBeingDeleted = false;

	public Connectable(HashSet<Item> inputs, HashSet<Item> outputs, GameObject prefab) {
		this.inputs = inputs;
		this.outputs = outputs;
		this.prefab = prefab;
	}

	public bool CanConnect(Connectable other) {
		return this.inputs.Overlaps(other.outputs) || this.outputs.Overlaps(other.inputs);
	}
	
	public void Connect(Connection con) {
		connections.Add(con);
	}

	public void Disconnect(Connection con) {
		if (!isBeingDeleted) connections.Remove(con);
	}

	public override GameObject CreateVisualObject() {
		return UnityEngine.GameObject.Instantiate(prefab);
	}

	public override void Delete() {
		isBeingDeleted = true;
		foreach (var connection in connections) {
			connection.Disconnect();
		}
	}
}

public enum Item {
	RawResource,
	FactoryProduct
}
