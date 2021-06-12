using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class MoveCameraWithMouse : MonoBehaviour
{
	private Vector3 _clickedWorldPosition;
	private Vector3 _clickedCameraPosition;
	private Camera _cam;

	public float HorizontalSpeed = 0.25f;
	public float VerticalSpeed = 0.25f;

	private void Awake()
	{
		_cam = Camera.main;
	}

	private void Update()
	{
		//TODO: Restrict to bounds of map

		if (Input.GetMouseButtonDown(0))
		{
			_clickedWorldPosition = GameManager.input.GetMouseXZIntersect();
			_clickedCameraPosition = transform.position;
		}
		else if (Input.GetMouseButton(0))
		{


			var newWorldPosition = GameManager.input.GetMouseXZIntersect();

			transform.position = _clickedWorldPosition - new Vector3(
				x: (_clickedWorldPosition.x - newWorldPosition.x) * HorizontalSpeed,
				y: 0f,
				z: (_clickedWorldPosition.y - newWorldPosition.y) * VerticalSpeed
			);
		}
	}
}
