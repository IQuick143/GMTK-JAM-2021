using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Resource : Connectable
{
	public Resource() : base(
		inputs: new HashSet<Item>(), 
		outputs: new HashSet<Item>() { Item.RawResource }, 
		prefab: GameManager.prefab.ResourcePrefab
	)
	{
	}
}