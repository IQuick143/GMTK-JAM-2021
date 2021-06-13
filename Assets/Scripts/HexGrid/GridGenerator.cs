using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using SysRandom = System.Random;

/// <summary>
/// Generates a procedural grid of tile
/// </summary>
[RequireComponent(typeof(GridManager))]
public class GridGenerator : MonoBehaviour
{
	private SysRandom RNG;

	[Header("Mountain Generation")]
	[Range(0f, 1f)]
	public float MountainAmplitudeCutoff = 0.75f;
	public float MountainNoiseScale = 5f;

	[Header("River Generation")]
	public int MinRiverCount = 2;
	public int MaxRiverCount = 2;

	public int MinRiverLength = 8;
	public int MaxRiverLength = 20;

	[Range(1, 5)]
	public int RiverMaxRotationAngle = 2;
	public int RiverMinRotationFrequency = 3;
	public int RiverMaxRotationFrequency = 8;

	[Header("Resource Generation")]
	public int ResourceCount = 10;


	[Header("Seeding")]
	public int Seed;
	public bool UseRandomSeed = true;

	private void Start()
	{
		if (UseRandomSeed)
			Seed = UnityEngine.Random.Range(0, int.MaxValue);

		RNG = new SysRandom(Seed);

		var manager = GetComponent<GridManager>();
		var tiles = manager.tiles;

		/*
		 * Generate rivers
		 */

		int riverCount = RNG.Next(MinRiverCount, MaxRiverCount);

		for (int riverIndex = 0; riverIndex < riverCount; riverIndex++)
		{
			//Pick a random starting point
			Vector2Int start;

			do
			{
				start = new Vector2Int(RNG.Next(tiles.Width), RNG.Next(tiles.Height));
			} while (!tiles[start.x, start.y].IsEmpty);


			//Create path
			int riverLength = RNG.Next(MinRiverLength, MaxRiverLength);
			int stepsToDirectionChange = RNG.Next(RiverMinRotationFrequency, RiverMaxRotationFrequency);
			int x = start.x;
			int y = start.y;
			var direction = (HexDirection)RNG.Next(0, typeof(HexDirection).GetEnumValues().Length);

			for (int riverPosition = 0; riverPosition < riverLength; riverPosition++)
			{
				//Place tile
				tiles[x, y].SetObject(new RiverBase());

				//Rotate river
				if (--stepsToDirectionChange == 0)
				{
					stepsToDirectionChange = RNG.Next(RiverMinRotationFrequency, RiverMaxRotationFrequency);

					
					int rotateAmount;

					do
					{
						rotateAmount = RNG.Next(-RiverMaxRotationAngle, RiverMaxRotationAngle + 1);
					} while (rotateAmount == 0);

					direction = direction.RotateCW(rotateAmount);
					tiles.GetNeighbourCoordinate(x, y, direction);
				}

				//Calculate next point
 				Vector2Int? nextPoint = tiles.GetNeighbourCoordinate(x, y, direction);


				//Reached a blocked location. Stop
				if (!nextPoint.HasValue || !tiles[nextPoint.Value.x, nextPoint.Value.y].IsEmpty)
					break;

				x = nextPoint.Value.x;
				y = nextPoint.Value.y;
			}
		}

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

				if (noiseLevel >= MountainAmplitudeCutoff && tiles[x, y].IsEmpty)
					tiles[x, y].SetObject(new Mountain());
			}
		}

		/*
		 * Generate resources
		 */

		for (int i = 0; i < ResourceCount; i++)
			GenerateRandomResource(manager);

		/*
		 * Generate market
		 */

		{
			int x, y;

			do
			{
				x = RNG.Next(tiles.Width);
				y = RNG.Next(tiles.Height);
			} while (!tiles[x, y].IsEmpty);

			manager.market = new Connectable(
				inputs: new HashSet<Item>() { Item.FactoryProduct },
				output: Item.Money,
				prefab: GameManager.prefab.MarkedPrefab
			);

			tiles[x, y].SetObject(manager.market);
		}
	}

	/// <summary>
	/// Generates a random resource in a random location
	/// </summary>
	/// <param name="manager"></param>
	private void GenerateRandomResource(GridManager manager)
	{
		//TODO: Make sure this is balanced

		int x, y;

		do
		{
			x = RNG.Next(manager.tiles.Width);
			y = RNG.Next(manager.tiles.Height);
		} while (!manager.tiles[x, y].IsEmpty);

		manager.tiles[x, y].SetObject(new Connectable(
			inputs: new HashSet<Item>(),
			output: Item.RawResource,
			prefab: GameManager.prefab.ResourcePrefab
		));
	}
}
