using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connectable : Entity {
	public HashSet<Item> inputs {get; private set;}
	public HashSet<Item> outputs {get; private set;}

	public List<Connection> connections;

	public bool CanConnect(Connectable other) {
		return this.inputs.Overlaps(other.outputs) || this.outputs.Overlaps(other.inputs);
	}
	
	public void Connect(Connection con) {
		
	}

	public void Disconnect(Connection con) {
		
	}

	public override GameObject GetPrefab() {
		throw new System.NotImplementedException();
	}

	public override void Delete() {
		foreach (var connection in connections) {
			connection.Disconnect();
		}
		throw new System.NotImplementedException();
	}
}

public enum Item {

}
