using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieFace : MonoBehaviour
{
    private int value;
    [SerializeField] private List<TextMeshPro> textMeshes;

    public float YPos => transform.position.y;
    public int Value => value;

    public void SetValue(int v)
    {
        value = v;
        foreach (TextMeshPro textMesh in textMeshes)
        {
			textMesh.text = value.ToString();
		}
    }
}
