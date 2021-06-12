using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents a "cardinal" direction in a hexagonal grid
/// </summary>
public enum HexDirection
{
	UpLeft,
	UpRight,
	Right,
	DownRight,
	DownLeft,
	Left
}

public static class HexDirectionExtensions
{
	public static HexDirection RotateCW(this HexDirection direction, int amount = 1)
	{
		if (amount < 0)
			return RotateCCW(direction, -amount);

		return (HexDirection)(((int)direction + amount) % 6);
	} 

	public static HexDirection RotateCCW(this HexDirection direction, int amount = 1)
	{
		if (amount < 0)
			return RotateCW(direction, -amount);

		return (HexDirection)(((int)direction + 5 * amount) % 6);
	}
}