using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private int health = 5;

	[SerializeField] private List<Die> dice;
	
	[SerializeField] private Sprite spriteDefault;
	[SerializeField] private Sprite spriteDamage;

	public void TakeDamage(int damage)
	{
		health -= damage;
		
		if (health < 0)
		{
			// character is dead
		}
	}
}
