using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Icon : MonoBehaviour {
	public void ShowItem(Item item) {
		this.GetComponent<Image>().sprite = item.GetItemIcon();
	}

	public void Remove() {
		Destroy(this.gameObject);
	}
}
