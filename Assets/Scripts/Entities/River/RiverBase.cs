using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class RiverBase : Entity
{
	public override GameObject GetPrefab()
	{
		return GameManager.prefab.RiverEntityPrefab;
	}

	public override void Delete()
	{
		
	}
}