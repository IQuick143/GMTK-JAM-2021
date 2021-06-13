using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Icon : MonoBehaviour {
	public void ShowItem(Item item) {
		this.GetComponent<Image>().sprite = GameManager.sprite.GetItemIcon(item);
	}

	public void Remove() {
		Destroy(this.gameObject);
	}
}
