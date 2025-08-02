using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieFace : MonoBehaviour
{
    private int value;
    [SerializeField] private List<TextMeshPro> textMeshes;

    /// <summary>
    /// y-height of the face. Used to calculate which face is pointing up.
    /// </summary>
    public float YPos => transform.position.y;

    /// <summary>
    /// Value of the face of the die
    /// </summary>
    public int Value => value;

    /// <summary>
    /// Store the value of the face and change the TextMeshes to reflect the value.
    /// </summary>
    /// <param name="v"></param>
    public void SetValue(int v)
    {
        value = v;
        foreach (TextMeshPro textMesh in textMeshes)
        {
			textMesh.text = value.ToString();
		}
    }
}
