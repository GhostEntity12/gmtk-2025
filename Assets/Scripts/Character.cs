using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	[SerializeField] private int maxHealth = 5;
	[SerializeField] private int health;

	[SerializeField] private List<Die> dice;

	[SerializeField] Image characterImage;
	[SerializeField] private Sprite spriteDefault;
	[SerializeField] private Sprite spriteDamage;
	[SerializeField] AnimationCurve hurtCurve;

	private void Start()
	{
		health = maxHealth;
	}

	[ContextMenu("Test Damage")]
	void td() => TakeDamage(0);

	public void TakeDamage(int damage)
	{
		health -= damage;

		if ((float)health/maxHealth <= 0.5f)
		{
			// character is badly damaged
			characterImage.sprite = spriteDamage;
		}

		else if (health <= 0)
		{
			// character is dead
		}
		LeanTween.color(characterImage.rectTransform, Color.red, 0.5f).setEase(hurtCurve);
	}
}
