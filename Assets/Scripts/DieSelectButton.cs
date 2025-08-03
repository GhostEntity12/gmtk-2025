using UnityEngine;

public class DieSelectButton : DiePreview
{
	[SerializeField] private DieVisualizer visualizer;

	public override void SetInfo(DieInfo info)
	{
		base.SetInfo(info);
		visualizer.SetInfo(info);
	}

	public void SpawnDie()
	{
		RollManager.Instance.SpawnDiePlayer(info);
	}
}
