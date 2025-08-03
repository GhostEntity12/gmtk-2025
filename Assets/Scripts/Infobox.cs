using System.Linq;
using TMPro;
using UnityEngine;

public class Infobox : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    bool isVisible;

    [SerializeField] TextMeshProUGUI titleTextMesh;
    [SerializeField] TextMeshProUGUI detailsTextMesh;

    string placeholder = "Does something very important that is detailed here";

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

	public void SetDieInfo(DieInfo d)
    {
        titleTextMesh.text = $"{(d.Material!= Modifiers.MaterialModifiers.None ? d.Material + " ": "")}d{(int)d.Type}{(d.Color != Modifiers.ColorModifiers.None ? $" with {d.Color} numbers" : "")}";
        string faceString = string.Join(", ", d.FaceValues.OrderBy(v => v));
        detailsTextMesh.text = $"<b>Sides:</b> {faceString}\n<b>{d.Material}: </b>{placeholder}\n<b>{d.Color}: </b>{placeholder}";
    }

    public void Hide()
	{
		LeanTween.scale(rectTransform, Vector3.zero, 0.3f).setEaseInBack().setOnComplete(() => isVisible = false);
	}
}
