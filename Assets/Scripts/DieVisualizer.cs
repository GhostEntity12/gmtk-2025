using System.Collections.Generic;
using UnityEngine;

public class DieVisualizer : MonoBehaviour
{
	[SerializeField] private DieInfo dieInfo;
	[SerializeField] private List<DieFace> faces;
	[SerializeField] MeshRenderer r;

	public DieInfo DieInfo => dieInfo;
	public List<DieFace> Faces => faces;

	public void SetInfo(DieInfo d)
	{
		dieInfo = d;
		for (int i = 0; i < (int)d.Type; i++)
		{
			faces[i].SetValue(dieInfo.FaceValues[i]);
		}
		// Set material and color
		//renderer.material = 
	}

	public void SetFaces(params int[] faceValues)
	{
		if (faceValues.Length != faces.Count)
		{
			Debug.LogError("Mismatched faces and faceValues");
			return;
		}

		for (int i = 0; i < faces.Count; i++)
		{
			faces[i].SetValue(faceValues[i]);
		}
	}
}
