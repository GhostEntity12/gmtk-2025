using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeTester : Singleton<TimeTester>
{
	List<float> times = new();
	List<int> rolls = new();

	public void RegisterRoll(float time, int roll)
	{
		times.Add(time);
		rolls.Add(roll);
	}

	[ContextMenu("Get Info")]
	void GetInfo()
	{
		float max = times.Max();
		float min = times.Min();
		float avg = times.Sum()/times.Count;

		int[] rollFreq = new int[8];
		for (int i = 0; i < 8; i++)
		{
			rollFreq[i] = rolls.Count(r => r == i+1);
		}
		Debug.Log($"Times Recorded: {times.Count}\nMax time: {max}\nMin time: {min}\n Average time: {avg}");
		Debug.Log($"1s: {rollFreq[0]}, 2s: {rollFreq[1]}, 3s: {rollFreq[2]}, 4s: {rollFreq[3]}, 5s: {rollFreq[4]}, 6s: {rollFreq[5]}, 7s: {rollFreq[6]}, 8s: {rollFreq[7]}, ");
	}
}
