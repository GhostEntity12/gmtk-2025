using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup fade;
    [SerializeField] RectTransform decoration;

    public void Trigger()
    {
        LeanTween.alphaCanvas(fade, 1, 1f);
        LeanTween.moveY(decoration, 0f, 0.8f).setEaseOutBack().setDelay(0.8f);
    }
	public void LoadScene(int index)
	{
		SceneManager.LoadScene(index);
	}
}
