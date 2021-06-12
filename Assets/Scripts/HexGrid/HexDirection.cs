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

	///<summary>
	///Returns the CW rotation from the Z+ axis
	///</summary>
	public static float ToDegrees(this HexDirection direction)
	{
		switch (direction) {
			case HexDirection.UpLeft: return 240f;
			case HexDirection.UpRight: return 300f;
			case HexDirection.Right: return 0f;
			case HexDirection.DownRight: return 60f;
			case HexDirection.DownLeft: return 120f;
			case HexDirection.Left: return 180f;
		}
		// Should not happen but compiler is angwy
		return 0f;
	}
}