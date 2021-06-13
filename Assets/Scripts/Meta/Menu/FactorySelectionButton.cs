using UnityEngine;
using UnityEngine.UI;

public class FactorySelectionButton : MonoBehaviour {
	[SerializeField]
	private Image background;
	[SerializeField]
	private Image display;
	[SerializeField]
	private new TMPro.TMP_Text name;
	[SerializeField]
	private TMPro.TMP_Text price;
	[SerializeField]
	private Color selectedColour = Color.white;
	[SerializeField]
	private Color unSelectedColour = Color.clear;

	public int index = 0;

	public void SetInfo(Connectable connectable) {
		this.display.sprite = connectable.Icon;
		this.name.text = connectable.factoryname;
		this.price.text = "$" + connectable.price;
	}

	public void Clicked() {
		GameManager.menu.ChooseFactory(index);
		background.color = selectedColour;
	}

	public void Unselect() {
		background.color = unSelectedColour;
	}
}