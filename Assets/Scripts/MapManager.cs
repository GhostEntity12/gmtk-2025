using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private List<RectTransform> fightLocations;
	[SerializeField] private RectTransform playerIcon;
	[SerializeField] private float playerIconYOffset = 120;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			ShowMap();
		}
	}

	public void MovePlayerIcon(int fightIndex)
	{
		LeanTween.move(playerIcon, fightLocations[fightIndex].anchoredPosition + Vector2.up * playerIconYOffset, 0.8f).setEaseInOutCubic();
	}

	void StartFight()
	{
		//GameManager.Instance.LoadFight();
	}

	public void ShowMap()
	{
		LeanTween.moveY(rectTransform, 0, 0.7f).setEaseOutBack();
	}

	public void HideMap()
	{
		LeanTween.moveY(rectTransform, -1100, 0.7f).setEaseInBack();
	}
}
