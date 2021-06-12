using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Anything that can be placed on a tile
/// </summary>
public abstract class Entity {
	public abstract GameObject CreateVisualObject();
	public abstract void Delete();
}