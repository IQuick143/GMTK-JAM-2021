using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Holds a hexagonal grid of Ts
/// </summary>
/// <typeparam name="T"></typeparam>
public class HexArray<T> : IEnumerable<T>
{
	private T[,] _internal;

	/// <summary>
	/// Gets the width of the grid
	/// </summary>
	public int Width { get; }

	/// <summary>
	/// Gets the height of the grid
	/// </summary>
	public int Height { get; }

	/// <summary>
	/// Creates a new hex-array with the provided dimensions
	/// </summary>
	/// <param name="sizeX"></param>
	/// <param name="sizeY"></param>
	public HexArray(int sizeX, int sizeY)
	{
		Width = sizeX;
		Height = sizeY;
		_internal = new T[sizeX, sizeY];
	}

	/// <summary>
	/// Gets or sets a tile
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public T this[int x, int y]
	{
		get
		{
			return _internal[x, y];
		}
		set
		{
			_internal[x, y] = value;
		}
	}

	/// <summary>
	/// Gets the coordinates that border the provided tile
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public IEnumerable<Vector2Int> GetNeighbourCooridnates(int x, int y, bool includeSelf = false)
	{
		var output = new List<Vector2Int>();

		output.Add(new Vector2Int(x - 1, y));
		output.Add(new Vector2Int(x + 1, y));

		if (includeSelf)
			output.Add(new Vector2Int(x, y));

		if (y % 2 == 0)
		{
			output.Add(new Vector2Int(x, y - 1));
			output.Add(new Vector2Int(x + 1, y - 1));

			output.Add(new Vector2Int(x, y + 1));
			output.Add(new Vector2Int(x + 1, y + 1));
		}
		else
		{
			output.Add(new Vector2Int(x - 1, y - 1));
			output.Add(new Vector2Int(x, y - 1));

			output.Add(new Vector2Int(x - 1, y + 1));
			output.Add(new Vector2Int(x, y + 1));
		}

		return output.Where(i => IsInBounds(i.x, i.y));
	}

	/// <summary>
	/// Gets the tiles that neighbors the provided coordinate
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public IEnumerable<T> GetNeighbours(int x, int y, bool includeSelf = false)
	{
		return GetNeighbourCooridnates(x, y, includeSelf).Select(i => this[i.x, i.y]);
	}

	/// <summary>
	/// Gets the position of the provided tile
	/// </summary>
	/// <param name="tile"></param>
	/// <returns></returns>
	public Vector2Int? GetTilePosition(T tile)
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if (_internal[x, y].Equals(tile))
					return new Vector2Int(x, y);
			}
		}

		return null;
	}

	/// <summary>
	/// Gets whether the provided position is in-bounds of the grid
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public bool IsInBounds(int x, int y) 
	{
		return x > 0 && y > 0 && x < Width && y > Height;
	}

	/// <summary>
	/// <inheritdoc />
	/// </summary>
	/// <returns></returns>
	public IEnumerator<T> GetEnumerator()
	{
		return _internal.Cast<T>().GetEnumerator();
	}

	/// <summary>
	/// <inheritdoc />
	/// </summary>
	/// <returns></returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
