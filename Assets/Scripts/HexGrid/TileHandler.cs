using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour {
	private static float sqrt_3 = Mathf.Sqrt(3);
	// Radius is also the sidelength of the hexagon, edit as necessary
	private float radius = 1/sqrt_3;
	private int x=0,y=0;

	public GridManager manager;
	[SerializeField]
	private MeshRenderer groundRenderer;
	private GameObject visualObject;

	private Entity entity;

	private bool _hover = false;
	public bool hover {
		get {return _hover;}
		set {
			if (value != _hover) {
				_hover = value;
				if (_hover) {
					OnMouseEnter();
				} else {
					OnMouseExit();
				}
			}
		}
	}

	// Start is called before the first frame update
	void Start() {
		
	}

	// Makes the TileHandler calculate the world coordinates to place itself at
	public void SetGeometry(int x, int y, float radius) {
		this.x = x;
		this.y = y;
		this.radius = radius;

		int half_offset = this.x % 2;

		this.transform.position = new Vector3(this.x * 1.5f * radius, 0f, (this.y - half_offset / 2f) * radius * sqrt_3);

		if (this.y % 2 == 1) {
			this.groundRenderer.material.color = Color.gray;
		}
	}

	public IEnumerable<TileHandler> GetNeighbours() {
		return this.manager.tiles.GetNeighbours(this.x, this.y);
	}

	public void SetObject(Entity entity) {
		this.entity = entity;
		this.UpdateVisuals();
	}

	public void DeleteObject() {
		if (this.entity != null) {
			entity.Delete();
			this.entity = null;
		}
		UpdateVisuals();
	}

	public void UpdateVisuals() {
		if (visualObject != null) Destroy(visualObject);

		if (entity != null) {
			const float Y_OFFSET = 1f;
			this.visualObject = Instantiate(entity.GetPrefab(), transform.position + Vector3.up * Y_OFFSET, Quaternion.identity);
			this.visualObject.transform.SetParent(this.transform);
		}
	}

	public void OnMouseEnter() {
		this.groundRenderer.material.color = Color.red;
	}

	public void OnMouseExit() {
		this.groundRenderer.material.color = (this.y % 2 == 1)?Color.gray:Color.white;
	}

	// Update is called once per frame
	void Update() {
		
	}
}
