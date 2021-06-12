using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPreview : MonoBehaviour {
	[SerializeField]
	private new Renderer renderer;
	public bool valid = true;

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		renderer.material.color = Color.Lerp(Color.gray, valid?Color.white:Color.red, 0.6f + 0.5f * Mathf.Sin(3 * Time.realtimeSinceStartup));
	}
}
