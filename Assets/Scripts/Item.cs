using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Item {
	RawResource,
	FactoryProduct
}

public static class ItemExtensions {
	public static int GetPrice(this Item item) {
		switch (item) {
			case Item.RawResource: return 0;
			case Item.FactoryProduct: return 1;
			default: return 0;
		}
	}
}