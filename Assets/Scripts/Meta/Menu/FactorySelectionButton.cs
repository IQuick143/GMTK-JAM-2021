using UnityEngine;
using UnityEngine.UI;

public class FactorySelectionButton : MonoBehaviour {
	[SerializeField]
	private Image background;
	[SerializeField]
	private Color selectedColour = Color.white;
	[SerializeField]
	private Color unSelectedColour = Color.clear;

	public int index = 0;

	public void Clicked() {
		GameManager.menu.ChooseFactory(index);
		background.color = selectedColour;
	}

	public void Unselect() {
		background.color = unSelectedColour;
	}
}