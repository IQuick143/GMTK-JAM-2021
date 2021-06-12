using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class MoveCameraWithMouse : MonoBehaviour
{
	private Vector3 _lastMousePosition;

	public float HorizontalSpeed = 0.25f;
	public float VerticalSpeed = 0.25f;

	private void Update()
	{
		//TODO: Restrict to bounds of map

		if (Input.GetMouseButtonDown(0))
			_lastMousePosition = Input.mousePosition;

		else if (Input.GetMouseButton(0))
		{
			var newMousePosition = Input.mousePosition;

			transform.position += new Vector3(
				x: (_lastMousePosition.x - newMousePosition.x) * HorizontalSpeed, 
				y: 0f, 
				z: (_lastMousePosition.y - newMousePosition.y) * VerticalSpeed
			);

			_lastMousePosition = newMousePosition;
		}
	}
}
