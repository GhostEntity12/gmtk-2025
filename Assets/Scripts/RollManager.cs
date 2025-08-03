using UnityEngine;

public class RollManager : Singleton<RollManager>
{

	bool playerDieRolling = false;
	bool enemyDieRolling = false;

	[SerializeField] private Character player;
	private Die playerDie;

	[SerializeField] private Character enemy;
	private Die enemyDie;

	public void SetPlayerDie(Die die) => playerDie = die;
	public void SetEnemyDie(Die die) => enemyDie = die;

	[SerializeField] Die d6Prefab;
	[SerializeField] Die d8Prefab;

	[SerializeField] Transform playerDieSpawn;
	[SerializeField] Transform enemyDieSpawn;

	[SerializeField] DiePreview playerPreview;
	[SerializeField] DiePreview enemyPreview;

	private void Start()
	{
		playerDie = GetComponent<Die>();
	}

	public void SpawnDiePlayer(DieInfo info)
	{
		// Destroy the old die
		if (playerDie)
		{
			playerDie.Recorder.OnPlaybackFinished -= OnPlayerDieFinish;
			Destroy(playerDie.gameObject);
		}
		// Spawn the new die
		playerDie = Instantiate(
			info.Type switch
			{
				Die.DieType.D6 => d6Prefab,
				Die.DieType.D8 => d8Prefab,
				_ => throw new System.Exception("Invalid die to spawn"),
			}, playerDieSpawn);
		// Set the values
		playerDie.SetData(info);
		playerPreview.SetInfo(info);
		playerDie.Recorder.OnPlaybackFinished += OnPlayerDieFinish;
	}

	private void SpawnDieEnemy(DieInfo info)
	{
		// Destroy the old die
		if (enemyDie)
		{
			Destroy(enemyDie.gameObject);
			enemyDie.Recorder.OnPlaybackFinished -= OnEnemyDieFinish;
		}
		// Spawn the new die
		enemyDie = Instantiate(
			info.Type switch
			{
				Die.DieType.D6 => d6Prefab,
				Die.DieType.D8 => d8Prefab,
				_ => throw new System.Exception("Invalid die to spawn"),
			}, enemyDieSpawn);
		// Set the values
		enemyDie.SetData(info);
		enemyPreview.SetInfo(info);
		enemyDie.Recorder.OnPlaybackFinished += OnEnemyDieFinish;
	}

	public void EnemyPickDie()
	{
		int random = Random.Range(0, 3);
		SpawnDieEnemy(enemy.Dice[random]);
	}

	[ContextMenu("Roll")]
	public void Roll()
	{
		// Mark both dice as rolling
		playerDieRolling = true;
		enemyDieRolling = true;

		RollAndRecord(playerDie, true);
		RollAndRecord(enemyDie, true);
	}

	void OnPlayerDieFinish()
	{
		playerDieRolling = false;
		if (!enemyDieRolling)
		{
			CheckResults();
		}
	}

	void OnEnemyDieFinish()
	{
		enemyDieRolling = false;
		if (!playerDieRolling)
		{
			CheckResults();
		}
	}

	void CheckResults()
	{
		Debug.Log(playerDie.GetTopFace().Value);
		int playerRoll = playerDie.GetTopFace().Value;
		int enemyRoll = enemyDie.GetTopFace().Value;

		int delta = Mathf.Abs(playerRoll - enemyRoll);
		Debug.Log(delta);

		if (playerRoll > enemyRoll)
		{
			// Enemy takes damage
			enemy.TakeDamage(Mathf.Max(0, ColorModifier.Process(playerDie, playerRoll, true) - ColorModifier.Process(enemyDie, enemyRoll, false)));
		}
		else if (playerRoll < enemyRoll)
		{
			// Player takes damage
			player.TakeDamage(Mathf.Max(0, ColorModifier.Process(enemyDie, enemyRoll, true) - ColorModifier.Process(playerDie, playerRoll, false)));
		}

		// Check for deaths
		if (player.IsDead)
		{
			GameManager.Instance.LoseGame();
		}
		else if (enemy.IsDead)
		{
			GameManager.Instance.WinRound();
		}
		else
		{
			EnemyPickDie();
		}
	}

	private void RollAndRecord(Die d)
	{
		d.ApplyPhysics();
		d.Recorder.RecordSimulation();
		d.Recorder.PlaybackSimulation();
	}

	private void RollAndRecord(Die d, int? targetFace = null)
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

	private void RollAndRecord(Die d, bool doRandom)
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
