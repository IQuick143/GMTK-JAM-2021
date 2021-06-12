using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item
{
	public static readonly Item RawResource = new Item(0);
	public static readonly Item FactoryProduct = new Item(1);

	public int WorthPerSecond { get; }

	private Item(int worthPerSecond)
	{
		WorthPerSecond = worthPerSecond;
	}

}
