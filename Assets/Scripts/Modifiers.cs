using System;
using System.Linq;
using Modifiers;

namespace Modifiers
{
	public enum ColorModifiers
	{
		None, Red, Green, Blue
	}
	public enum MaterialModifiers
	{
		None, Ruby, Emerald, Sapphire
	}
}

public class ColorModifier
{
	public static int Process(Die d, int roll, bool winner)
	{
		return d.Color switch
		{
			ColorModifiers.None => roll,
			ColorModifiers.Red => winner ? roll + 2 : roll,
			ColorModifiers.Blue => winner ? roll : roll + 2,
			ColorModifiers.Green => throw new NotImplementedException(),
			_ => throw new NotImplementedException(),
		};
	}
}


public class MaterialModifier
{
	public static int Process(Die die, int roll)
	{
		return die.Material switch
		{
			MaterialModifiers.None => roll,
			_ => throw new NotImplementedException(),
		};
	}
}

/// <summary>
/// Explodes if lowest value
/// </summary>
//public class GoldMaterialModifier
//{
//	public int Process(Die die, int roll)
//	{
//		int lowestValue = die.Faces.Min(f => f.Value);
//		int runningTotal = roll;

//		if (roll == lowestValue)
//		{
//			int explodecount = die.Sides;
//			bool explode;
//			do
//			{
//				int newRoll = die.RandomFace.Value;
//				runningTotal += newRoll;


//				explode = (newRoll == lowestValue);
//				explodecount--;
//			} while (explodecount > 0 && explode);
//		}

//		return runningTotal;
//	}
//}
