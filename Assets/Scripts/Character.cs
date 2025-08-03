using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	[SerializeField] private int maxHealth = 10;
	[SerializeField] private int curHealth;

	[SerializeField] private List<DieInfo> dice;

	[SerializeField] Image characterImage;
	[SerializeField] private SpritePack sprites;
	[SerializeField] private AnimationCurve hurtCurve;
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private DieSelectButton[] selectors;

	public List<DieInfo> Dice => dice;

	public bool IsDead => curHealth <= 0;

	public void Setup(List<DieInfo> dice, SpritePack spritePack, int health)
	{
		this.dice = dice;
		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i].SetInfo(dice[i]);
		}

		sprites = spritePack;
		characterImage.sprite = sprites.attacking;

		maxHealth = health;
		curHealth = health;
		healthBar.SetInitialValue(maxHealth);
	}

	public void TakeDamage(int damage)
	{
		curHealth = Mathf.Max(0, curHealth - damage);

		if ((float)curHealth/maxHealth <= 0.5f)
		{
			// character is badly damaged
			characterImage.sprite = sprites.damaged;
		}

		LeanTween.color(characterImage.rectTransform, Color.red, 0.5f).setEase(hurtCurve);
		healthBar.SetValue(curHealth);
	}
}
