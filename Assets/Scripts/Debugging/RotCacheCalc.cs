using UnityEngine;

/// <summary>
/// Works decently as a starting point.
/// Generates some incorrect numbers.
/// Use RotTest.cs to find the errors and fix them
/// </summary>
public class RotCacheCalc : MonoBehaviour
{
	[SerializeField] Transform[] offsets;
	[SerializeField] Vector3[] offsetRots;

	[ContextMenu("Calculate and Print")]
	private void PrintString()
	{
		string block = "{\n";
		for (int j = 0; j < offsets.Length; j++)
		{
			transform.rotation = offsets[j].rotation;

			for (int i = 0; i < offsets.Length; i++)
			{
				offsetRots[i] = offsets[i].rotation.eulerAngles - transform.rotation.eulerAngles;
				float x = offsetRots[i].x;
				float y = offsetRots[i].y;
				float z = offsetRots[i].z;
				x = x < 0 ? (x + 360) : x == 360 ? 0 : x;
				y = y < 0 ? (y + 360) : y == 360 ? 0 : y;
				z = z < 0 ? (z + 360) : z == 360 ? 0 : z;
				offsetRots[i] = new Vector3(x, y, z);
			}

			string line = "\t{ ";
			for (int i = 0; i < offsets.Length; i++)
			{
				string set = $"new({offsetRots[i].x:000.00}f, {offsetRots[i].y:000.00}f, {offsetRots[i].z:000.00}f), ";
				line += set;
			}
			line += "},\n";
			block += line;
		}
		block += "};";
		Debug.Log(block);
	}
}
