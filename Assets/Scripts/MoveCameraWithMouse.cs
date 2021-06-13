using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class MoveCameraWithMouse : MonoBehaviour
{
	private Vector3 _clickedWorldPositionRelative;
	private Vector3 _clickedCameraPosition;
	private Camera _cam;

	public float lerpConstant = 0.75f;

	private void Awake()
	{
		_cam = Camera.main;
		_cam.transform.position = new Vector3(10f, 5f, 10f);
	}

	private void Update()
	{
		//TODO: Restrict to bounds of map

		if (Input.GetMouseButtonDown(0))
		{
			_clickedWorldPositionRelative = Rationalize(CalculateXZIntersectrelative(Input.mousePosition));
			_clickedCameraPosition = transform.position;
		}
		else if (Input.GetMouseButton(0))
		{
			var delta_cam = _clickedWorldPositionRelative - Rationalize(CalculateXZIntersectrelative(Input.mousePosition));

			transform.position += (_clickedCameraPosition + delta_cam - transform.position) * lerpConstant;
		}

		float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

		transform.position += transform.forward * scroll * 5f;

		//Clamp position
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5f, 40f), Mathf.Clamp(transform.position.y, 1f, 20f), Mathf.Clamp(transform.position.z, -5f, 40f));
	}

	private Vector3 Rationalize(Vector2 vec) {
		return new Vector3(Mathf.Clamp(vec.x, -100, 100), 0, Mathf.Clamp(vec.y, -100, 100));
	}

	private Vector3 CalculateXZIntersectrelative(Vector3 mousepos) {
		var ray = this._cam.ScreenPointToRay(Input.mousePosition);
		// Calculate where the ray hits y = 0
		if (ray.direction.y >= 0f) {
			return Vector2.positiveInfinity;
		}

		float distance = -ray.origin.y / ray.direction.y;

		return new Vector2(ray.direction.x, ray.direction.z) * distance;
	}
}
