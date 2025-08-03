using System.Collections.Generic;
using System;
using UnityEngine;
using Modifiers;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	private BodyType playerBodyType;
	private int fightNumber = 0;
	private List<DieInfo> bossDice = new();

	[SerializeField] private Character player;
	[SerializeField] private Character enemy;

	[SerializeField] private Infobox infobox;
	[SerializeField] private MapManager mapManager;
	[SerializeField] private RollManager rollManager;
	public Infobox Infobox => infobox;

	[Header("Sprites")]
	[SerializeField] SpritePack playerFem;
	[SerializeField] SpritePack playerMasc;

	[SerializeField] SpritePack enemyFem;
	[SerializeField] SpritePack enemyMasc;

	[SerializeField] SpritePack bossFem;
	[SerializeField] SpritePack bossMasc;


	private void Start()
	{
		CharacterPicker picker = FindAnyObjectByType<CharacterPicker>();
		if (picker)
		{
			playerBodyType = picker.Hero;
		}
		player.Setup(ChooseEnemyDice(), playerBodyType == BodyType.Masc ? playerMasc : playerFem, 15);
	}

	public void WinRound()
	{
		// Won last fight
		if (fightNumber == 4)
		{
			// Save the winning dice as the new boss dice
			bossDice = player.Dice;

			// Take note of the new bodytype for the next round
			GameObject go = new("CharacterPicker");
			go.AddComponent<CharacterPicker>().ChooseHero((BodyType)((int)(playerBodyType + 1) % 2));

			// Show win screen
			return;
		}

		fightNumber++;
		mapManager.ShowMap();
		LeanTween.delayedCall(1.5f, () => mapManager.MovePlayerIcon(fightNumber));
	}

	public void LoseGame()
	{

	}

	public void LoadFight()
	{
		enemy.Setup(ChooseEnemyDice(), fightNumber switch
		{
			4 => playerBodyType == BodyType.Masc ? bossFem : bossMasc,
			_ => UnityEngine.Random.value < 0.5f ? enemyFem : enemyMasc,
		}, 10 + (fightNumber * 2));

		rollManager.EnemyPickDie();
	}

	List<DieInfo> ChooseEnemyDice()
	{
		List<DieInfo> enemyDice = new()
		{
			GoblinDice.GoblinD6[UnityEngine.Random.Range(0, GoblinDice.GoblinD6.Count)],
			GoblinDice.GoblinD6[UnityEngine.Random.Range(0, GoblinDice.GoblinD6.Count)],
			GoblinDice.GoblinD8[UnityEngine.Random.Range(0, GoblinDice.GoblinD8.Count)]
		};
		switch (fightNumber)
		{
			case 1:
				// 2nd fight, one die has a random color
				enemyDice[UnityEngine.Random.Range(0, 3)].ApplyColor((ColorModifiers)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ColorModifiers)).Length));
				break;
			case 2:
				// 3nd fight, one die has a random material
				enemyDice[UnityEngine.Random.Range(0, 3)].ApplyMaterial((MaterialModifiers)UnityEngine.Random.Range(0, Enum.GetValues(typeof(MaterialModifiers)).Length));
				break;
			case 3:
				// 4nd fight, one die has a random material, and one die has a random color
				enemyDice[UnityEngine.Random.Range(0, 3)].ApplyColor((ColorModifiers)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ColorModifiers)).Length));
				enemyDice[UnityEngine.Random.Range(0, 3)].ApplyMaterial((MaterialModifiers)UnityEngine.Random.Range(0, Enum.GetValues(typeof(MaterialModifiers)).Length));
				break;
			case 4:
				return bossDice;
			default:
				break;
		}
		return enemyDice;
	}
}

[System.Serializable]
public struct SpritePack
{
	public Sprite attacking;
	public Sprite damaged;
}

public class GoblinDice
{
	public static readonly List<DieInfo> GoblinD6 = new()
	{
		new DieInfo(Die.DieType.D6, new int[] { 1, 2, 3, 4, 5, 6 }),
		new DieInfo(Die.DieType.D6, new int[] { 1, 3, 3, 3, 5, 6 }),
		new DieInfo(Die.DieType.D6, new int[] { 1, 1, 1, 5, 6, 7 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 2, 4, 4, 4, 6 }),
		new DieInfo(Die.DieType.D6, new int[] { 3, 3, 3, 3, 4, 4 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 0, 5, 5, 6, 6 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 1, 3, 3, 5, 7 }),
		new DieInfo(Die.DieType.D6, new int[] { 1, 1, 2, 3, 4, 8 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 0, 0, 4, 10, 10 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 0, 0, 5, 6, 6 }),
		new DieInfo(Die.DieType.D6, new int[] { 1, 1, 1, 4, 4, 9 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 2, 3, 4, 5, 7 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 4, 4, 4, 4, 4 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 1, 1, 5, 5, 6 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 1, 1, 6, 6, 7 }),
		new DieInfo(Die.DieType.D6, new int[] { 0, 3, 3, 3, 3, 8 }),
	};
	public static readonly List<DieInfo> GoblinD8 = new()
	{
		new DieInfo(Die.DieType.D8, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }),
		new DieInfo(Die.DieType.D8, new int[] { 1, 2, 4, 4, 4, 4, 6, 8 }),
		new DieInfo(Die.DieType.D8, new int[] { 1, 1, 1, 1, 5, 6, 7, 8 }),
		new DieInfo(Die.DieType.D8, new int[] { 4, 4, 4, 4, 4, 5, 5, 5 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 0, 3, 5, 5, 6, 6, 8 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 1, 3, 3, 5, 5, 7, 9 }),
		new DieInfo(Die.DieType.D8, new int[] { 1, 1, 2, 3, 4, 6, 8, 10 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 0, 0, 4, 4, 10, 10, 12 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 0, 0, 0, 7, 7, 8, 8 }),
		new DieInfo(Die.DieType.D8, new int[] { 1, 1, 1, 2, 6, 6, 9, 12 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 2, 3, 5, 7, 11, 13, 17 }),
		new DieInfo(Die.DieType.D8, new int[] { 1, 1, 2, 3, 5, 8, 13, 21 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 1, 1, 2, 5, 5, 6, 8 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 1, 1, 2, 6, 6, 7, 9 }),
		new DieInfo(Die.DieType.D8, new int[] { 0, 4, 4, 4, 4, 5, 8, 10 }),
	};
}