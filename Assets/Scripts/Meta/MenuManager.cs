using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour {
	[Header("Tooltip")]
	[SerializeField]
	private GameObject tooltip;
	private RectTransform tooltipRect;
	[SerializeField]
	private TMP_Text text;
	[SerializeField]
	private GameObject recipeArea;
	[SerializeField]
	private GameObject inputs;
	[SerializeField]
	private GameObject outputs;
	[SerializeField]
	private GameObject icon_prefab;

	private List<Icon> input_icons = new List<Icon>();

	// Start is called before the first frame update
	void Start() {
		this.tooltip.SetActive(false);
		this.tooltipRect = tooltip.GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update() {

		const int OFFSET = 10;

		tooltipRect.position = (Vector2)Input.mousePosition;
		tooltipRect.position += Vector3.down * (tooltipRect.sizeDelta.y * 0.5f + OFFSET);
		tooltipRect.position += Vector3.right * (tooltipRect.sizeDelta.x * 0.5f + OFFSET);
	}

	public void HideTooltip() {
		this.tooltip.SetActive(false);
	}

	public void ShowTextTooltip(string text) {

		this.tooltip.SetActive(true);
		this.text.text = text;
		this.tooltip.SetActive(false);
	}

	public void ShowConnectableTooltip(Connectable connectable) {
		DeleteIcons();
		foreach (Item item in (Item[]) System.Enum.GetValues(typeof(Item))) {
			if (connectable.inputs.Contains(item)) input_icons.Add(CreateIcon(item, inputs.transform));
		}

		this.tooltip.SetActive(true);
		this.text.text = connectable.ToString();
		this.tooltip.SetActive(true);
	}

	private Icon CreateIcon(Item item) {
		Icon icon = Instantiate(icon_prefab).GetComponent<Icon>();
		icon.ShowItem(item);
		return icon;
	}

	private Icon CreateIcon(Item item, Transform parent) {
		Icon icon = Instantiate(icon_prefab, parent).GetComponent<Icon>();
		icon.ShowItem(item);
		return icon;
	}

	private void DeleteIcons() {
		foreach (Icon icon in input_icons) {
			icon.Remove();
		}
		input_icons.Clear();
	}
}
