using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	[SerializeField]
	private Camera viewcamera;

	void Start() {
		if (viewcamera == null) {
			viewcamera = Camera.main;
		}
	}

	///<summary>
	/// Computes the intersect of a ray from the mouse with the XZ plane.
	/// Returns a point at infinity when ray does not exist.
	///</summary>
	public Vector2 GetMouseXZIntersect() {
		var ray = this.viewcamera.ScreenPointToRay(Input.mousePosition);
		// Calculate where the ray hits y = 0
		if (ray.direction.y >= 0f) {
			return Vector2.positiveInfinity;
		}

		float distance = -ray.origin.y / ray.direction.y;

		Vector3 intersect = ray.origin + ray.direction * distance;
		Debug.DrawLine(ray.origin, intersect, Color.red);
		return new Vector2(intersect.x, intersect.z);
	}
}
