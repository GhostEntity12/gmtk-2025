using System.Collections.Generic;
using UnityEngine;

public class RotTest : MonoBehaviour
{
	/*
	 * TODO:
	 * Doesn't actually currently need a grid of dice, just needs a line of dice
	 * Look into automating it and making it work off of a grid later.
	 */

	[SerializeField] int faceCount = 6;

	[SerializeField] int rotAllTo = 0;

	[SerializeField] List<Die> allRot;

	private Vector3[,] transformationArray;

	private void Start()
	{
		transformationArray = faceCount switch
		{
			//4 => Transformations.d4,
			6 => Transformations.d6,
			8 => Transformations.d8,
			_ => throw new System.ArgumentException("Improper number of sides on die")
		};
		FixPositions();
		SetRows();
	}


	[ContextMenu("Set Columns")]
	void SetColumns()
	{
		int cache = rotAllTo;
		rotAllTo = 0;
		RotateAllTo();
		for (int i = 0; i < allRot.Count; i++)
		{
			allRot[i].transform.GetChild(0).Rotate(transformationArray[0, i / faceCount]);
		}
		rotAllTo = cache;
	}

	// Currently unneeded
	[ContextMenu("Set Rows")]
	void SetRows()
	{
		int cache = rotAllTo;
		rotAllTo = 0;
		RotateAllTo();
		for (int i = 0; i < allRot.Count; i++)
		{
			allRot[i].transform.GetChild(0).Rotate(transformationArray[0, i % faceCount], Space.Self);
		}
		rotAllTo = cache;
	}

	/// <summary>
	/// Forces the dice back into a grid
	/// </summary>
	[ContextMenu("Fix Positions")]
	void FixPositions()
	{
		for (int i = 0; i < allRot.Count; i++)
		{
			allRot[i].transform.localPosition = new(i / faceCount * 2, allRot[i].transform.position.y, i % faceCount * 2);
		}
	}

	/// <summary>
	/// Rotates all dice to a given face.
	/// </summary>
	[ContextMenu("Rotate All Dice")]
	void RotateAllTo()
	{
		for (int i = 0; i < allRot.Count; i++)
		{
			allRot[i].transform.GetChild(0).Rotate(transformationArray[allRot[i].Faces.IndexOf(allRot[i].GetTopFace()), rotAllTo], Space.Self);
		}
	}

	/// <summary>
	/// Physically rolls all dice to a given face and stores the animation.
	/// If rotAllTo is < 0, each die's face is chosen randomly.
	/// </summary>
	[ContextMenu("Roll")]
	public void RollAndRecord()
	{
		// Record an animation for both dice
		for (int i = 0; i < allRot.Count; i++)
		{
			Die d = allRot[i];
			d.ApplyPhysics();
			d.Recorder.RecordSimulation();

			// Rig dice if rotAllTo is set
			if (rotAllTo > 0)
			{
				//Debug.Log($"Rolled {d.GetTopFace().Value}. Cheating Roll to {d.Faces[rotAllTo].Value}");
				d.TransformToTarget(d.Faces.IndexOf(d.GetTopFace()), rotAllTo);
			}

			d.Recorder.PlaybackSimulation();
		}
	}
}
