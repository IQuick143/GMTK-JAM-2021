using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Item {
	Wood,
	Planks,
	Furniture,
	FineFurniture,
	Ore,
	Metal,
	MetalComponents,
	
	BasicTools,
	RawResource,
	FactoryProduct,
	Money
}

public static class ItemExtensions {
	public static int GetPrice(this Item item) {
		switch (item) {
			case Item.RawResource: return 0;
			case Item.FactoryProduct: return 1;
			case Item.Money: return 1;
			default: return 0;
		}
	}
	public static UnityEngine.Color GetColour(this Item item) {
		switch (item) {
			case Item.RawResource: return UnityEngine.Color.gray;
			case Item.FactoryProduct: return UnityEngine.Color.cyan;
			default: return UnityEngine.Color.white;
		}
	}
}