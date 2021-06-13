using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {
	[SerializeField]
	private Sprite missingTexture;
	[SerializeField]
	private Sprite rawResource;
	[SerializeField]
	private Sprite money;
	public Sprite GetItemIcon(Item item) {
		switch (item) {
			case Item.RawResource: return rawResource;
			case Item.Money: return money;
			default: {
				Debug.LogWarning($"{item} is missing a texture!");
				return missingTexture;
			}
		}
	}
}
