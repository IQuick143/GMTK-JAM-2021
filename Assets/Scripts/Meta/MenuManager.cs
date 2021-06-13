using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour {
	[Header("Tooltip")]
	[SerializeField]
	private GameObject tooltip;
	private Connectable currentlyDisplayedConnectable = null;
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
	private TMP_Text sidebarSalesText;
	[SerializeField]
	private TMP_Text sidebarCostText;
	[SerializeField]
	private TMP_Text sidebarProfitText;
	[SerializeField]
	private TMP_Text sidebarCurrencyText;

	[SerializeField]
	private string salesTextFormat = "Sales: {0} $/s";

	[SerializeField]
	private string CostTextFormat = "Cost: {0} $/s";

	[SerializeField]
	private string profitTextFormat = "<b>Profit: {0} $/s</b>";

	[SerializeField]
	private string sidebarCurrencyTextFormat = "${0}";

	[SerializeField]
	private TMP_Text gameOver;

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
		sidebarSalesText.text = string.Format(salesTextFormat, GameManager.Instance.CurrentOutputPerSecond);
		sidebarCostText.text = string.Format(CostTextFormat, (int)GameManager.Instance.CurrentCost);
		sidebarProfitText.text = string.Format(profitTextFormat, GameManager.Instance.CurrentOutputPerSecond - (int)GameManager.Instance.CurrentCost);
		sidebarCurrencyText.text = string.Format(sidebarCurrencyTextFormat, Mathf.Max(0, GameManager.Instance.Currency));

		if (GameManager.Instance.GameOver)
			gameOver.gameObject.SetActive(true);

		if (GameManager.Instance.CurrentOutputPerSecond < (int)GameManager.Instance.CurrentCost)
			sidebarProfitText.color = Color.red;
		else
			sidebarProfitText.color = Color.black;
	}

	public void AddFactories(List<GameObject> factoriesToAdd) {
		int pre_add_count = this.availableFactories.Count;
		for (int i = 0; i < factoriesToAdd.Count; i++) {
			this.availableFactories.Add(factoriesToAdd[i]);
			var button_object = Instantiate(factorySelectionButtonPrefab, this.factorySelectionButtonContainer);
			this.factorySelectionButtons.Add(button_object.GetComponent<FactorySelectionButton>());
			this.factorySelectionButtons[pre_add_count + i].index = pre_add_count + i;
			this.factorySelectionButtons[pre_add_count + i].SetInfo(factoriesToAdd[i].GetComponent<Connectable>());
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
		currentlyDisplayedConnectable = connectable;
		DeleteIcons();
		switch (connectable.type) {
			case Connectable.Type.Factory: {
				foreach (Item item in (Item[]) System.Enum.GetValues(typeof(Item))) {
					if (connectable.inputs.Contains(item)) input_icons.Add(CreateIcon(item, inputs.transform));
				} break;
			}
			case Connectable.Type.Market: {
				input_icons.Add(CreateIcon(GameManager.sprite.Anything, inputs.transform));
				break;
			}
		}

		output_icon.ShowItem(connectable.output);

		this.tooltip.SetActive(true);
		this.text.text = connectable.ToString();
		this.tooltip.SetActive(true);
	}

	public void UpdateConnectableTooltip() {
		ShowConnectableTooltip(currentlyDisplayedConnectable);
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

	private Icon CreateIcon(Sprite sprite, Transform parent) {
		Icon icon = Instantiate(icon_prefab, parent).GetComponent<Icon>();
		icon.ShowSprite(sprite);
		return icon;
	}

	private void DeleteIcons() {
		foreach (Icon icon in input_icons) {
			icon.Remove();
		}
		input_icons.Clear();
	}
}
