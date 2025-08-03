using UnityEngine;

public class RollManager : MonoBehaviour
{
	[SerializeField] private Character player;
	[SerializeField] private Die playerDie;

	[SerializeField] private Character enemy;
	private Die enemyDie;

	public void SetPlayerDie(Die die) => playerDie = die;
	public void SetEnemyDie(Die die) => enemyDie = die;

	private void Start()
	{
		playerDie = GetComponent<Die>();
	}

	[ContextMenu("Roll")]
	public void Roll()
	{
		RollAndRecord(playerDie, true);
		RollAndRecord(enemyDie, true);

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
	public void RollAndRecord(Die d)
	{
		d.ApplyPhysics();
		d.Recorder.RecordSimulation();
		d.Recorder.PlaybackSimulation();
	}

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

	public void RollAndRecord(Die d, bool doRandom)
	{
		if (!doRandom)
		{
			RollAndRecord(d);
			return;
		}

		d.ApplyPhysics();
		d.Recorder.RecordSimulation();
		int targetFaceIndex = Random.Range(0, d.Faces.Count);
		//Debug.Log($"Rolled {d.GetTopFace().Value}. Cheating Roll to {d.Faces[targetFaceIndex].Value}");
		d.TransformToTarget(d.Faces.IndexOf(d.GetTopFace()), targetFaceIndex);

		d.Recorder.PlaybackSimulation();
	}
}
