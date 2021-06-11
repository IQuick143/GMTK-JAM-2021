﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Mountain : Entity
{
    public override GameObject GetPrefab()
    {
        return GameManager.Instance.MountainEntityPrefab;
    }
}