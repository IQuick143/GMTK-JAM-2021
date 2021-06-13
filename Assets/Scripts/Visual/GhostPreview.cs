using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPreview : MonoBehaviour {
	[SerializeField]
	private Renderer[] renderers;
	public bool valid = true;

	public Color BaseColor;
	public Color ValidColor;
	public Color InvalidColor;

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		foreach (var i in renderers)
			i.material.color = Color.Lerp(BaseColor, valid ? ValidColor : InvalidColor, 0.6f + 0.5f * Mathf.Sin(3 * Time.realtimeSinceStartup));
	}
}
