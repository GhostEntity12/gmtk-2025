using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMesh;
	[SerializeField] private Image fillImage;
	private int maxHealth;
	private float targetFillEnd;
	private float targetFillStart;
	private float speed = 12f;
	private float time = 1;

	private void Update()
	{
		if (time < 1)
		{
			fillImage.fillAmount = Mathf.Lerp(targetFillStart, targetFillEnd, time);
			time += speed * Time.deltaTime;
		}
	}

	public void SetInitialValue(int health)
	{
		maxHealth = health;
		textMesh.text = health.ToString();
		fillImage.fillAmount = 1;
	}

	public void SetValue(int health)
	{
		textMesh.text = health.ToString();
		targetFillStart = fillImage.fillAmount;
		targetFillEnd = (float)health / maxHealth;
		time = 0;
	}
}
