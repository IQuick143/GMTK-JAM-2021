using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Connectable : Entity {
	public List<Item> inputs {get {return _inputs;} private set {_inputs = value;}}
	public Dictionary<Item, List<Connectable>> input_dependencies {get; private set;}
	public Item output {get {return _output;} private set {_output = value;}}
	public Connectable output_connected {get; private set;}

	public List<Connection> connections;

	private bool isBeingDeleted = false;

	public bool active {get; private set;}

	[SerializeField]
	private List<Item> _inputs;
	[SerializeField]
	private Item _output;

	[SerializeField]
	public string factoryname;

	[SerializeField]
	public int price;
	[SerializeField]
	public Type type;

	[SerializeField]
	public Sprite Icon;

	void Awake() {
		this.connections = new List<Connection>();
		this.input_dependencies = new Dictionary<Item, List<Connectable>>();
		if (type == Type.Market) {
			foreach (Item item in System.Enum.GetValues(typeof(Item))) {
				input_dependencies[item] = new List<Connectable>();
			}
		} else foreach (Item item in this.inputs) {
			input_dependencies[item] = new List<Connectable>();
		}
		UpdateActivation();
	}

	public bool CanConnect(Connectable other) {
		return (this.type == Type.Market || this.inputs.Contains(other.output)) && other.output_connected == null || (other.type == Type.Market || other.inputs.Contains(this.output)) && this.output_connected == null;
	}

	public Item? GetCompatiblePorts(Connectable other) {
		if (this.inputs.Contains(other.output)) return other.output;
		if (other.inputs.Contains(this.output)) return this.output;
		return null;
	}
	
	public void Connect(Connection con) {
		connections.Add(con);
		if (con.Stream == this.output) {
			output_connected = con.Other(this);
		} else {
			input_dependencies[con.Stream].Add(con.Other(this));
		}
		UpdateActivation();
	}

	public void Disconnect(Connection con) {
		if (!isBeingDeleted) {
			connections.Remove(con);
			if (con.Stream == this.output) {
				output_connected = null;
			} else {
				input_dependencies[con.Stream].Remove(con.Other(this));
			}
			UpdateActivation();
		}
	}

	void OnDestroy() {
		isBeingDeleted = true;
		foreach (var connection in connections) {
			connection.Disconnect();
		}
	}

	public void UpdateActivation() {
		bool was_active = this.active;
		bool new_active = true;
		foreach (var item in this.input_dependencies.Values) {
			new_active &= item.Any(c => c.active);
		}
		if (this.active != new_active) {
			this.active = new_active;
			if (this.output_connected != null) this.output_connected.UpdateActivation();
		}
	}

	public override string ToString() {
		switch (type) {
			case Type.Factory: return $"{this.factoryname} ({(active?"Active":"Inactive")})";
			case Type.Market: return $"{this.factoryname}";
			case Type.Source: return $"{this.factoryname}";
		}
		return $"{this.factoryname} ({(active?"Active":"Inactive")})";
	}

	public enum Type {
		Factory,
		Source,
		Market
	}
}
