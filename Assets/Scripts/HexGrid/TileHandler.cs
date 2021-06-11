using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour {
	private static float sqrt_3 = Mathf.Sqrt(3);
	// Radius is also the sidelength of the hexagon, edit as necessary
	private float radius = 1/sqrt_3;
	private int x=0,y=0;

	public GridManager manager;
	private GameObject visualObject;

	private Entity entity;

	// Start is called before the first frame update
	void Start() {
		
	}

	// Makes the TileHandler calculate the world coordinates to place itself at
	public void SetCoordinate(int x, int y) {
		this.x = x;
		this.y = y;

		int half_offset = this.x % 2;

		this.transform.position = new Vector3(this.x * 1.5f * radius, 0f, (this.y - half_offset / 2f) * radius * sqrt_3);
	}

	public IEnumerable<TileHandler> GetNeighbours() {
		return this.manager.tiles.GetNeighbours(this.x, this.y);
	}

	public void SetObject(Entity entity) {
		this.entity = entity;
		this.UpdateVisuals();
	}

	public void UpdateVisuals() {
		// DELETE VISUAL OBJECT
		Destroy(visualObject);

		if (entity != null) {
			// ADD A VISUAL OBJECT BASED ON 
			const float Y_OFFSET = 1f;

			Instantiate(entity.GetPrefab(), transform.position + Vector3.up * Y_OFFSET, Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update() {
		
	}
}
