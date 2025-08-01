using UnityEngine;

public class RollManager : MonoBehaviour
{
    [SerializeField] private Character player;
    private Die playerDie;

    [SerializeField] private Character enemy;
	private Die enemyDie;

    public void SetPlayerDie(Die die) => playerDie = die;
    public void SetEnemyDie(Die die) => enemyDie = die;

    public void Roll()
    {
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
}
