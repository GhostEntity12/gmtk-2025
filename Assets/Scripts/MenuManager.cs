using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] RectTransform characterPicker;
	[SerializeField] CanvasGroup fade;
	public void OpenCharacterPicker()
	{
		LeanTween.moveY(characterPicker, 0, 0.5f).setEaseOutBack();
	}

	public void StartGame()
	{
		LeanTween.alphaCanvas(fade, 1, 1f).setOnComplete(() => SceneManager.LoadScene(1));
	}

	public void LockButton(Button b)
	{
		b.interactable = false;
	}
}
