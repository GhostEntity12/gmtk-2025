using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Infobox : MonoBehaviour
{
    RectTransform rectTransform;
    bool isVisible;

    [SerializeField] TextMeshProUGUI titleTextMesh;
    [SerializeField] TextMeshProUGUI detailsTextMesh;

    string placeholder = "Does something very important that is detailed here";

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void Show(RectTransform targetRectTransform)
    {
        // If it's already open somewhere else for whatever reason, immediately scale it to zero
        if (isVisible)
        {
            rectTransform.localScale = Vector3.zero;
        }
        rectTransform.position = targetRectTransform.position;
        LeanTween.scale(rectTransform, Vector3.one, 0.3f).setEaseOutBack();
        isVisible = true;
    }

	public void SetDieInfo(Die d)
    {
        titleTextMesh.text = $"{(d.Material!= Modifiers.MaterialModifiers.None ? d.Material + " ": "")}d{d.Faces.Count}{(d.Color != Modifiers.ColorModifiers.None ? $"with {d.Color} numbers" : "")}";
        string faceString = string.Join(", ", d.Faces.OrderBy(f => f.Value).Select(f => f.Value));
        detailsTextMesh.text = $"<b>Sides:</b> {faceString}\n<b>{d.Material}:</b>{placeholder}\n<b>{d.Color}:</b>{placeholder}";
    }

    public void Hide()
	{
		LeanTween.scale(rectTransform, Vector3.one, 0.3f).setEaseInBack().setOnComplete(() => isVisible = false);
	}
}
