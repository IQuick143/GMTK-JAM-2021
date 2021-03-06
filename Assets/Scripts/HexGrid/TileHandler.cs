using System;
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
	private Renderer groundRenderer;
	private GameObject visualObject;

	private Color originalColor;

	public Entity entity {get; private set;}

	public bool IsEmpty
	{
		get
		{
			return entity == null;
		}
	}
		
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
	private bool showing_tooltip = false;

	public void SetGeometry(int x, int y, float radius) {
		this.x = x;
		this.y = y;
		this.radius = radius;
		if (this.y % 2 == 1) {
			Color c = this.groundRenderer.material.color;
			this.groundRenderer.material.color = new Color(c.r / 2, c.g / 2, c.b / 2);
		}
	}

	public IEnumerable<TileHandler> GetNeighbours() {
		return this.manager.tiles.GetNeighbours(this.x, this.y);
	}

	public void SetObject(GameObject prefab) {
		if (this.entity != null) {
			Destroy(this.visualObject);
		}
		this.visualObject = Instantiate(prefab);
		this.entity = this.visualObject.GetComponent<Entity>();
		this.visualObject.transform.position = transform.position;
		this.visualObject.transform.SetParent(this.transform);
	}

	public void DeleteObject() {
		if (this.entity != null) {
			Destroy(this.visualObject);
			this.entity = null;
		}
	}

	public void OnMouseEnter() {
		originalColor = this.groundRenderer.material.color;
		this.groundRenderer.material.color = Color.red;
	}

	public void OnMouseExit() {
		this.groundRenderer.material.color = originalColor;
	}

	public void OnLMB() {
		this.groundRenderer.material.color = (this.y % 2 == 1)?Color.gray:Color.white;
		if (IsEmpty) {
			if (GameManager.menu.selectedFactory != null && GameManager.Instance.Currency > 1+GameManager.menu.selectedFactory.GetComponent<Connectable>().price) {
				SetObject(GameManager.menu.selectedFactory);
				GameManager.Instance.Currency -= GameManager.menu.selectedFactory.GetComponent<Connectable>().price;
			}
		} else if (this.entity.GetType() == typeof(Connectable)) {
			this.manager.BeginConnecting(this.x, this.y);
		}
	}

	public void OnRMB() {
		DeleteObject();
	}

	// Update is called once per frame
	void Update() {
		if (_hover) {
			if (!showing_tooltip &&this.entity != null && this.entity.GetType() == typeof(Connectable)) {
				showing_tooltip = true;
				GameManager.menu.ShowConnectableTooltip((Connectable) this.entity);
			}
		} else {
			if (showing_tooltip) {
				showing_tooltip = false;
				GameManager.menu.HideTooltip();
			}
		}
	}
}
