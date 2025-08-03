using UnityEngine;

public class DiePreview : MonoBehaviour
{
	protected DieInfo info;

	public virtual void SetInfo(DieInfo info)
	{
		this.info = info;
	}

	public void OpenInfobox()
	{
		if (info.Type > 0)
		{
			GameManager.Instance.Infobox.SetDieInfo(info);
			GameManager.Instance.Infobox.Show(transform as RectTransform);
		}
	}

	public void CloseInfobox()
	{
		GameManager.Instance.Infobox.Hide();
	}
}
