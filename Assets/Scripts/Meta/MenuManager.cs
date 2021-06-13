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
	private Icon output_icon;
	[SerializeField]
	private GameObject icon_prefab;

	private List<Icon> input_icons = new List<Icon>();

	[Header("Sidebar")]
	[SerializeField]
	private TMP_Text sidebarGeneratingOutputText;
	[SerializeField]
	private TMP_Text sidebarRequiredOutputText;
	[SerializeField]
	private TMP_Text sidebarCurrencyText;

	[SerializeField]
	private string sidebarGeneratingTextFormat = "Output: {0} $/s";

	[SerializeField]
	private string sidebarRequiredTextFormat = "Required: {0} $/s";

	[SerializeField]
	private string sidebarCurrencyTextFormat = "${0}";

	[Header("Factory Selection")]
	[SerializeField]
	public GameObject selectedFactory = null;
	private int selectedFactoryIndex = -1;

	private List<GameObject> availableFactories;
	private List<FactorySelectionButton> factorySelectionButtons;

	[SerializeField]
	private GameObject factorySelectionButtonPrefab;
	[SerializeField]
	private Transform factorySelectionButtonContainer;

	// Start is called before the first frame update
	void Start() {
		this.tooltip.SetActive(false);
		this.tooltipRect = tooltip.GetComponent<RectTransform>();
		this.availableFactories = new List<GameObject>();
		this.factorySelectionButtons = new List<FactorySelectionButton>();
		AddFactories(new List<GameObject>() {
			GameManager.prefab.MetalFactory, GameManager.prefab.PlankFactory
		});
	}

	// Update is called once per frame
	void Update() {

		const int OFFSET = 10;

		//Set tooltip position
		tooltipRect.position = (Vector2)Input.mousePosition;
		tooltipRect.position += Vector3.down * (tooltipRect.sizeDelta.y * 0.5f + OFFSET);
		tooltipRect.position += Vector3.right * (tooltipRect.sizeDelta.x * 0.5f + OFFSET);

		//Update sidebar generation info
		sidebarGeneratingOutputText.text = string.Format(sidebarGeneratingTextFormat, GameManager.Instance.CurrentOutputPerSecond);
		sidebarRequiredOutputText.text = string.Format(sidebarRequiredTextFormat, -1);
		sidebarCurrencyText.text = string.Format(sidebarCurrencyTextFormat, GameManager.Instance.Currency);
	}

	public void AddFactories(List<GameObject> factoriesToAdd) {
		int pre_add_count = this.availableFactories.Count;
		for (int i = 0; i < factoriesToAdd.Count; i++) {
			this.availableFactories.Add(factoriesToAdd[i]);
			var button_object = Instantiate(factorySelectionButtonPrefab, this.factorySelectionButtonContainer);
			this.factorySelectionButtons.Add(button_object.GetComponent<FactorySelectionButton>());
			this.factorySelectionButtons[pre_add_count + i].index = pre_add_count + i;
		}
	}

	public void ChooseFactory(int i) {
		if (selectedFactoryIndex >= 0) factorySelectionButtons[selectedFactoryIndex].Unselect();
		selectedFactory = availableFactories[i];
		selectedFactoryIndex = i;
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

		output_icon.ShowItem(connectable.output);

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
