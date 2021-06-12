using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Mountain : Entity {
	public override GameObject CreateVisualObject() {
		return GameObject.Instantiate(GameManager.prefab.MountainEntityPrefab);
	}

	public override void Delete() {
		
	}
}