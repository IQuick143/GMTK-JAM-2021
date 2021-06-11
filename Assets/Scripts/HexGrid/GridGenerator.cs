using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Generates a procedural grid of tile
/// </summary>
[RequireComponent(typeof(GridManager))]
public class GridGenerator : MonoBehaviour
{
	[Range(0f, 1f)]
	public float MountainAmplitudeCutoff = 0.75f;

	public float MountainNoiseScale = 5f;

	[Header("Seeding")]
	public int Seed;
	public bool UseRandomSeed = true;

	//TODO: Make sure this takes place AFTER grid manager
	private void Start()
	{
		if (UseRandomSeed)
			Seed = UnityEngine.Random.Range(0, int.MaxValue);

		var manager = GetComponent<GridManager>();
		var tiles = manager.tiles;

        /*
         * Generate mountains
         */

        for (int x = 0; x < tiles.Width; x++)
        {
            for (int y = 0; y < tiles.Height; y++)
            {
				float seed = (float)Seed / int.MaxValue;
				float xPercent = (float)x / tiles.Width;
				float yPercent = (float)y / tiles.Height;

				float noiseLevel = Mathf.PerlinNoise(
					xPercent * MountainNoiseScale + seed,
					yPercent * MountainNoiseScale + seed
				);

				if (noiseLevel >= MountainAmplitudeCutoff)
					tiles[x, y].SetObject(new Mountain());
			}
        }

	}
}
