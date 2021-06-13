using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Connectable : Entity {
	public HashSet<Item> inputs {get; private set;}
	public Item output {get; private set;}

	public List<Connection> connections = new List<Connection>();

	private GameObject prefab;

	private bool isBeingDeleted = false;

	public Connectable(HashSet<Item> inputs, Item output, GameObject prefab) {
		this.inputs = inputs;
		this.output = output;
		this.prefab = prefab;
	}

	public bool CanConnect(Connectable other) {
		return this.inputs.Contains(other.output) || other.inputs.Contains(this.output);
	}

	public Item? GetCompatiblePorts(Connectable other) {
		if (this.inputs.Contains(other.output)) return other.output;
		if (other.inputs.Contains(this.output)) return this.output;
		return null;
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
