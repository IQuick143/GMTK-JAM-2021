using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public enum Item {
	Wood,
	Planks,
	Furniture,
	FineFurniture,
	Ore,
	Metal,
	MetalComponents,
	BasicTools,
	Heater,
	Money
}

public static class ItemExtensions {

	public static int GetPrice(this Item item) {
		return item switch
		{
			Item.Wood => 0,
			Item.Planks => 1,
			Item.Furniture => 3,
			Item.FineFurniture => 7,
			Item.Ore => 0,
			Item.Metal => 1,
			Item.MetalComponents => 3,
			Item.BasicTools => 4,
			Item.Heater => 7,
			Item.Money => 0,
			_ => 0
		};
	}

	public static Color GetColour(this Item item) {
		return item switch
		{
			Item.Wood => new Color(0.35f, 0.28f, 0.24f),
			Item.Planks => new Color(0.35f, 0.24f, 0.16f),
			Item.Furniture => new Color(0.49f, 0.26f, 0.11f),
			Item.FineFurniture => new Color(0.24f, 0.67f, 0.09f),
			Item.Ore => new Color(0.44f, 0.53f, 0.67f),
			Item.Metal => new Color(0.29f, 0.5f, 0.82f),
			Item.MetalComponents => new Color(0.20f, 0.5f, 1f),
			Item.BasicTools => new Color(0.06f, 0.42f, 1f),
			Item.Heater => new Color(0.43f, 0.13f, 1f),
			Item.Money => Color.green,
			_ => Color.white
		};
	}

	public static Sprite GetItemIcon(this Item item) {
		var s = GameManager.sprite;

		return item switch
		{
			Item.Wood => s.wood,
			Item.Planks => s.planks,
			Item.Furniture => s.furniture,
			Item.FineFurniture => s.fineFurniture,
			Item.Ore => s.ore,
			Item.Metal => s.metal,
			Item.MetalComponents => s.metalComponents,
			Item.BasicTools => s.basicTools,
			Item.Money => s.money,
			Item.Heater => s.heater,
			_ => GameManager.sprite.missingTexture,
		};
	}
}