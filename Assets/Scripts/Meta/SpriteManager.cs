using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {
	[SerializeField]
	private Sprite missingTexture;
	[SerializeField]
	private Sprite rawResource;
	public Sprite GetItemIcon(Item item) {
		switch (item) {
			case Item.RawResource: return rawResource;
			default: {
				Debug.LogWarning($"{item} is missing a texture!");
				return missingTexture;
			}
		}
	}
}
