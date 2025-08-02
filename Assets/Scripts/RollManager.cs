using UnityEngine;

public class RollManager : MonoBehaviour
{
	[SerializeField] private Character player;
	[SerializeField] private Die playerDie;

	[SerializeField] private Character enemy;
	private Die enemyDie;
	[SerializeField] int tfi;

	[SerializeField] bool doRotate = true;

	public void SetPlayerDie(Die die) => playerDie = die;
	public void SetEnemyDie(Die die) => enemyDie = die;

	private void Start()
	{
		playerDie = GetComponent<Die>();
	}

	[ContextMenu("Roll")]
	public void Roll()
	{
		// Record an animation for both dice
		playerDie.ApplyPhysics();
		playerDie.Recorder.RecordSimulation();
		DieFace topFace = playerDie.GetTopFace();
		if (doRotate)
		{
			int targetFaceIndex = tfi;// Random.Range(0, playerDie.Faces.Count);
			Debug.Log($"Rolled {topFace.Value}. Cheating Roll to {playerDie.Faces[targetFaceIndex].Value}");
			playerDie.TransformToTarget(playerDie.Faces.IndexOf(topFace), targetFaceIndex);
		}
		playerDie.Recorder.PlaybackSimulation();

		//int playerRoll = playerDie.Roll();
		//int enemyRoll = enemyDie.Roll();

		//if (playerRoll > enemyRoll)
		//{
		//    // Enemy takes damage
		//    enemy.TakeDamage(Mathf.Max(0, ColorModifier.Process(playerDie, playerRoll, true) - ColorModifier.Process(enemyDie, enemyRoll, false)));

		//    // Play player attacks animation
		//}
		//else if (playerRoll < enemyRoll)
		//{
		//    // Player takes damage
		//    player.TakeDamage(Mathf.Max(0, ColorModifier.Process(enemyDie, playerRoll, true) - ColorModifier.Process(playerDie, enemyRoll, false)));

		//    // Play enemy attacks animation
		//}
		//else
		//{
		//    // Neither takes damage

		//    // Play tie animation
		//}
	}

	[ContextMenu("Roll")]
	public void RollAndRecord(Die d, int? targetFace = null)
	{
		d.ApplyPhysics();
		d.Recorder.RecordSimulation();

		// Rig dice if a targetFace is set
		if (targetFace != null)
		{
			//Debug.Log($"Rolled {d.GetTopFace().Value}. Cheating Roll to {d.Faces[rotAllTo].Value}");
			d.TransformToTarget(d.Faces.IndexOf(d.GetTopFace()), (int)targetFace);
		}

		d.Recorder.PlaybackSimulation();
	}

	[ContextMenu("Replay Roll")]
	void ReplayRoll()
	{
		playerDie.recorder.PlaybackSimulation();
	}
}
