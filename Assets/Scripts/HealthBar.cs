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

    private void Update()
	{
        fillImage.fillAmount = Mathf.Lerp(targetFillStart, targetFillEnd, 30f);
	}

	public void SetMaxHealth(int maxHealth) => this.maxHealth = maxHealth;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void SetValue(int health)
    {
        textMesh.text = health.ToString();
        targetFillStart = fillImage.fillAmount;
        targetFillEnd = (float)health / maxHealth;
	}
}
